using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodItem : MonoBehaviour {

    public enum FoodTypes { flour, butter, eggs, milk, cream, salt, sugar,
        vanilla, nuts, vinegar, oregano, parsley, nutmeg, cinnamon, cheese,
        apple, strawberry, raspberry, lemon, potato, sweetpotato, crab,
        chocolate, pepper, paprika, gummyworms }

    private FoodTypes current_food;

    [SerializeField]
    private SpriteRenderer foodSR;

    [SerializeField]
    private Color DecayColor;

    [SerializeField]
    private Rigidbody2D rb2d;

    [SerializeField]
    private GameObject shardObj;


    [SerializeField]
    private Sprite foodSplat, liquidSplat;

    private bool isDecaying;

    private float countdowntimer;
    private float decaytime = 5f;

    public Color splatColor = Color.white;
    private float throwtime;
    private float throwtimer;
    private Vector2 origvel;

    private bool isGoingIntoOven;
    public bool canBounce = false;
    public bool canStun = false;
    public bool canPickUp = false;


    public int owner = -1;

    private GameController gc;


    public bool isBeingThrown = false;

	// Use this for initialization
	void Start() {
         gc = GameController.instance;
        //SetFoodType(FoodTypes.milk);

        //throwfood(Vector2.left*2f);



	}

    //call this to throw the food
    public void throwfood(Vector2 direct, float duration = 2f)
    {

        rb2d.velocity = direct;
        origvel = direct;
        throwtime = duration;
        throwtimer = throwtime;
        isBeingThrown = true;
        Invoke("makeBounceable", .3f);
        Invoke("makePickUppable", .5f);
        makeStunnable();



    }

    private void makePickUppable()
    {
        canPickUp = true;
    }

    private void makeStunnable()
    {
        canStun = true;
    }

    public void doBounce()
    {
        if (owner == -1)
        {
            return;
        }
      //  Debug.Log("trying to bounce");
        if (isBeingThrown && canBounce)
        {
            Vector3 slightBounce = Random.insideUnitCircle.normalized;
            origvel = -origvel;
            origvel = Vector3.Lerp(origvel, slightBounce, .3f);
            rb2d.velocity = -rb2d.velocity;
            rb2d.velocity = Vector2.Lerp(rb2d.velocity, (Vector2)slightBounce, .3f);
            canBounce = false;
            canStun = true;
            isBeingThrown = true;
            Invoke("makeBounceable", .3f);
        }
    }

    private void makeBounceable()
    {
        canBounce = true;
    }

    public void goIntoOven(Oven newparent)
    {
        rb2d.simulated = false;
        gameObject.tag = "donefood";
        transform.parent = newparent.transform;
        isGoingIntoOven = true;
    }


	// Update is called once per frame
	void Update () {

        

        if (isGoingIntoOven)
        {

            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime*2f);
            if (transform.localPosition.magnitude < .1f)
            {
                transform.parent.GetComponent<Oven>().DoCook(this);



            }



            return;
        }



        if (isBeingThrown)
        {
            throwtimer -= Time.deltaTime;
            if (throwtimer < 1f)
            {
                rb2d.velocity = Vector2.Lerp(origvel, Vector2.zero, 1-throwtimer);

                if (throwtimer <= 0f)
                {
                    isBeingThrown = false;
                    rb2d.velocity = Vector2.zero;
                    StartDecay();
                    foodSR.sortingOrder = 0;
                }

            }



        }



        //process decaying food
        if (isDecaying)
        {
            countdowntimer -= Time.deltaTime;

            //change sprite here i guess
            foodSR.color = Color.Lerp(Color.white, DecayColor, GetDecayPercentage());


            //check to see if it is fully decayed
            if (countdowntimer <= 0)
            {
                doSplat(false,false);
                
            }

        }



        CheckBounds();


	}


    private void CheckBounds()
    {
        
        if (transform.position.x < -8.5f || transform.position.x > 8.5f || transform.position.y > 5f || transform.position.y < -5f)
        {
            doBounce();
        }

    }

  

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("slippery"))
        {
            var otherFood = collision.gameObject.GetComponent<FoodItem>();
            if (otherFood != null && otherFood.IsCurrentlyDecaying())
                return;
           // Debug.Log("calling hit from food trigger");
            var playObj = collision.gameObject.GetComponent<PlayerController>();
            if (playObj != null)
            {
                if (playObj.playerNum == owner)
                {
                   // Debug.Log("hit owner");
                    return;
                }
                if (owner == -1)
                {
                  //  Debug.Log("has no owner");
                    return;
                }
                else
                {
                //    Debug.Log("hit enemy player?!");
                    playObj.DoStun(this, (playObj.transform.position - transform.position));
                }

            }
            else
            {
             //   Debug.Log("hit non-player");
                var otherOven = collision.gameObject.GetComponent<Oven>();
                if (otherOven != null && otherOven.ownerNum == owner)
                {
                    return;
                }
                if (collision.CompareTag("conveyerSpot") && !canBounce)
                {
                    return;
                }
            }
            doBounce();
        }
    }

    //get what the current food item is
    public FoodTypes GetFoodType()
    {
        return current_food;
    }

    //set what type of food the item is
    public void SetFoodType(FoodTypes newtype, bool dropped = false)
    {
        if (gc == null)
            gc = GameController.instance;
        current_food = newtype;
        foodSR.sprite = gc.foodSprites[(int)newtype];

        if (dropped)
        {
            Invoke("becomePickUppable", .5f);
        }

    }

    //used to initialize a food to a random type
    public void SetRandom()
    {
        SetFoodType((FoodTypes)Random.Range(0, System.Enum.GetValues(typeof(FoodItem.FoodTypes)).Length));
    }

    //for when food lands on the floor - start the 5 second rule countdown
    public void StartDecay()
    {
       
        if (!isDecaying)
        {
            doSplat();
            becomePickUppable();

            isDecaying = true;
            countdowntimer = decaytime;

        }
    }

    public void doSplat(bool isChef = false, bool makeSound = true)
    {
        AudioClip splatSound;
        bool doShatter = false;

        foodSR.sortingOrder = 0;
        switch (current_food)
        {
            case FoodTypes.crab:
            case FoodTypes.eggs:
                splatSound = SoundManager.instance.eggCrabImpact; //crab egg
                doShatter = true;
                break;
            case FoodTypes.vinegar:
                doShatter = true;
                splatSound = SoundManager.instance.largeGlassImpact; // large glass
                break;
            case FoodTypes.salt:
            case FoodTypes.pepper:
            case FoodTypes.paprika:
                doShatter = true;
                splatSound = SoundManager.instance.smallGlassImpact; //small glass
                break;
            default:
                splatSound = SoundManager.instance.FoodImpact(); //implement here
                break;
        }

        //play the sound here - splatSound
        if (makeSound )
        GetComponent<AudioSource>().PlayOneShot(splatSound);



        if (doShatter)
        {
            
            
            float splatSize = 1f;
            bool doShards = true;
            switch(current_food){
                case FoodTypes.vinegar:
                    splatColor = new Color(165f / 255f, 128f / 255f, 89f / 255f, .5f);
                    break;
                case FoodTypes.salt:
                    splatSize = .6f;
                    splatColor.a = .4f;
                    break;
                case FoodTypes.pepper:
                    splatSize = .6f;
                    splatColor = Color.gray;
                    splatColor.a = .4f;
                    break;
                case FoodTypes.paprika:
                    splatSize = .8f;
                    splatColor = Color.red;
                    splatColor.a = .4f;
                    break;
                default:
                    doShards = false;
                    break;
            }

            // instantiage glass shatter
            if (doShards)
            {
                var myShatter = Instantiate(shardObj);
                myShatter.transform.position = transform.position;
            }
           


            //instantly turn into splat, without playing rotting sound
            GoBad(false);
            foodSR.color = splatColor;
            transform.localScale *= splatSize;

           


        }
        if (countdowntimer < 0)
        {
            GoBad(true);
        }
        
        if (isChef)
        {
            foodSR.enabled = false;
            rb2d.simulated = false;
            
            Invoke("goAway", 5f);

        }

    }



    private void goAway()
    {
        Destroy(gameObject);
    }


    //is this food item currently decaying?
    public bool IsCurrentlyDecaying()
    {
        return isDecaying;
    }

    //return how much (0f-1f) the food has decayed
    public float GetDecayPercentage()
    {
        return Mathf.Clamp01((decaytime - countdowntimer) / decaytime);
    }

    private void becomePickUppable()
    {
        gameObject.layer = 10;
    }

    //do something here when the food goes bad?!
    private void GoBad(bool doRottenSound = true)
    {
        

        if (doRottenSound)
        {
            // play rotten sound here
            
            

            foodSR.sprite = foodSplat;
            foodSR.color = new Color(DecayColor.r, DecayColor.g, DecayColor.b, .5f);
            

        }
        else
        {
            foodSR.sprite = liquidSplat;
            rb2d.simulated = false;
            foodSR.color = splatColor;
            
        }


        gameObject.tag = "slippery";
        
        
        gameObject.layer = 10;

        //remove this script so it doesn't update anymore and just stays as a sprite renderer
        Destroy(this);


    }


}
