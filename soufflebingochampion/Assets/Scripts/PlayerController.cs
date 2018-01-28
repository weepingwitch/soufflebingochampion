using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


    //should be 0 or 1
    public int playerNum;


    
    private GameController gc;
    private InputManager im;
    [SerializeField]
    private Rigidbody2D rb2d;
    [SerializeField]
    private SpriteRenderer pimg, aimIndicator, heldItemImg;
    [SerializeField]
    private GameObject foodBase;
    [SerializeField]
    private Sprite whisk, spatula;
    [SerializeField]
    private Sprite[] dirtySpriteArray;

    private GameObject dirtySprite;
    private GameObject dirtySprite2;


    private Vector2 aim = Vector2.right;
    private Vector2 slideDirection;

    private float moveSpeed = 4f;
    private float throwStrength = 7f;

    private float throwCountdown;
    private float throwTime = .25f;


    private float slideCountdown;

    private Vector3 dropOffset;

    private Vector2 pushDirect;
    private float pushcount;

    private float dirtyAmt;
   

    private bool readyToThrow = true;

    private bool holdingFood = false;

    private bool canDoAction = true;


    private FoodItem.FoodTypes heldFood = FoodItem.FoodTypes.cheese ;

	// Use this for initialization
	void Start () {
        gc = GameController.instance;
        im = InputManager.instance;

       

        if (playerNum == 0)
        {
            gameObject.layer = 8;
            aimIndicator.sprite = whisk;
        }
        else
        {
            gameObject.layer = 9;
            aimIndicator.sprite = spatula;
           
        }

      

    }
	
	// Update is called once per frame
	void Update () {

        transform.position = (new Vector2(Mathf.Clamp(transform.position.x, -8.5f, 8.5f), Mathf.Clamp(transform.position.y, -4.25f, 4.25f)));


        //handle movement
        Vector2 movevect = im.getPlayerMove(playerNum);
        rb2d.velocity = movevect * moveSpeed;

        if (Mathf.Abs(movevect.magnitude) > .15f)
        {
            slideDirection = movevect.normalized * moveSpeed;
        }

        //handle being pushed
        if (pushcount > 0)
        {
            pimg.color = Color.red;
            pushcount -= Time.deltaTime;
            rb2d.velocity += pushDirect;
            if (pushcount <= 0)
            {
                pimg.color = Color.white;
                canDoAction = true;
            }
        }


        if (slideCountdown > 0)
        {
            slideCountdown -= Time.deltaTime;
            rb2d.velocity += slideDirection;
            pimg.color = Color.gray;
            if (slideCountdown <= 0)
            {
                pimg.color = Color.white;
                canDoAction = true;
            }
        }
        
        
        //handle aiming and sprite direction
        Vector2 aimdirect = im.getPlayerAim(playerNum);
        if (Mathf.Abs(aimdirect.magnitude) > .1f)
        {
            aim = aimdirect.normalized;

                pimg.flipX = (aim.x < 0);
           
        }
        else if (Mathf.Abs(movevect.magnitude) > .1f)
        {
            aim = movevect.normalized;
            pimg.flipX = (aim.x < 0);
        }

        //handle aim indicator
        aimIndicator.transform.localPosition = aim;
        aimIndicator.transform.up = aim;


        //handle currently held item
        if (holdingFood)
        {
            heldItemImg.enabled = true;
            if (pimg.flipX)
            {
                heldItemImg.transform.localPosition = new Vector3(.1f, -.3f, 0);
            }
            else
            {
                heldItemImg.transform.localPosition = new Vector3(-.1f, -.3f, 0);
            }
        }
        else
        {
            heldItemImg.enabled = false;
        }


        //handle throwing
        if (readyToThrow && canDoAction)
        {
           
            if (im.getPlayerShoot(playerNum) && holdingFood)
            {
                //Debug.Log(playerNum + " threw a " + heldFood);
                readyToThrow = false;
                throwCountdown = throwTime;
                doThrow(aim);
            }
        }
        else
        {
            throwCountdown -= Time.deltaTime;
            if (throwCountdown <= 0)
            {
                readyToThrow = true;
            }
        }

        if (dirtyAmt > 0)
        {
            dirtyAmt -= (Time.deltaTime / 8f);
        }

        handleDirtySprites();
        

	}

    private void handleDirtySprites()
    {


        dirtyAmt = Mathf.Clamp(dirtyAmt, 0, 4.5f);
        if (dirtySprite == null)
        {
            dirtySprite = new GameObject("ds1");
            dirtySprite.AddComponent<SpriteRenderer>();
            
            dirtySprite.transform.parent = transform;
        }
        if (dirtySprite2 == null)
        {
            dirtySprite2 = new GameObject("ds2");
            dirtySprite2.AddComponent<SpriteRenderer>();
            
            dirtySprite2.transform.parent = transform;
            
        }
        dirtySprite.transform.position = transform.position;
        dirtySprite2.transform.position = transform.position;
        dirtySprite2.GetComponent<SpriteRenderer>().sortingLayerName = "AIMINDICATOR";
        dirtySprite.GetComponent<SpriteRenderer>().sortingLayerName = "AIMINDICATOR";
        int ds1 = Mathf.FloorToInt(dirtyAmt);
        int ds2 = Mathf.Min(4, Mathf.CeilToInt(dirtyAmt));

        if (ds2 == 4 || ds1 == 4)
        {
            dirtySprite2.SetActive(false);
            dirtySprite.GetComponent<SpriteRenderer>().sprite = dirtySpriteArray[3];
            return;
        }
        if (ds1 <= 0)
        {
            dirtySprite.SetActive(false);
            dirtySprite2.SetActive(true);
            dirtySprite2.GetComponent<SpriteRenderer>().sprite = dirtySpriteArray[ds2];
            dirtySprite2.GetComponent<SpriteRenderer>().color = Color.Lerp(new Color(1f, 1f, 1f, 0f), Color.white, dirtyAmt);


        }
        else
        {
            dirtySprite.SetActive(true);
            dirtySprite2.SetActive(true);
            dirtySprite.GetComponent<SpriteRenderer>().sprite = dirtySpriteArray[ds1];
            dirtySprite.GetComponent<SpriteRenderer>().color = Color.Lerp(new Color(1f, 1f, 1f, 0f), Color.white, dirtyAmt-ds1);
            dirtySprite2.GetComponent<SpriteRenderer>().sprite = dirtySpriteArray[ds2];
            dirtySprite2.GetComponent<SpriteRenderer>().color = Color.Lerp(new Color(1f, 1f, 1f, 0f), Color.white, 1-(ds2-dirtyAmt));




        }




    }

    //this will happen for other players
    private void OnCollisionStay2D(Collision2D collision)
    {

        //Debug.Log(collision.gameObject.tag);
    }


    //this will happen for food
    private void OnTriggerStay2D(Collider2D collision)
    {
       // Debug.Log(collision.gameObject.tag + " t");

        if (collision.gameObject.CompareTag("fooditem"))
        {
            
           
            var theFood = collision.gameObject.GetComponent<FoodItem>();
            //if it is still an active food
            if (theFood != null)
            {
               // Debug.Log("food hit " + playerNum + theFood.owner);
                if (!theFood.isBeingThrown )
                {
                    if (theFood.owner != -1 && theFood.owner != playerNum)
                    {
                     //   Debug.Log("can't stun " + theFood.isBeingThrown + playerNum + theFood.owner + theFood.canBounce);
                    }
                    
                   if (canDoAction)
                    {
                        pickupFood(theFood);
                       // Debug.Log("regular non thrown pickup " + playerNum);
                    }
                    
                }
               else
                {
                 //   Debug.Log("hit by food " + playerNum);
                    if (theFood.owner != playerNum)
                    {
                      //  Debug.Log("stunned " + playerNum);

                        Vector3 stunDirect = transform.position - collision.gameObject.transform.position;
                        DoStun(theFood, stunDirect);
                    }
                    else
                    {
                    //    Debug.Log("can't be hit by own food " + playerNum);
                        if (theFood.owner == playerNum )
                        {
                     //       Debug.Log("food was pickuppable");
                            var success = pickupFood(theFood);
                            if (!success)
                            {
                          //      Debug.Log("calling hit from not picking up");
                                theFood.doBounce();
                            }
                        }
                    }
                    
                }
           
            }
        }
        if (collision.gameObject.CompareTag("slippery"))
        {
            //Debug.Log("slipping!");
            canDoAction = false;
            if (!GetComponent<AudioSource>().isPlaying)
            {
                AudioClip sfx = SoundManager.instance.Slip();
                GetComponent<AudioSource>().PlayOneShot(sfx);
            }
           
            slideCountdown = .05f;
        }
        if (collision.gameObject.CompareTag("shards"))
        {
            canDoAction = false;
            pushcount = .2f;
            pushDirect = Vector2.zero;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("conveyerSpot"))
        {

            Vector2 movevect = collision.gameObject.GetComponent<ConveyerSpot>().moveVect;
            transform.position += (Vector3)movevect * Time.deltaTime;
        }
    }

    //called when hit by a food
    public void DoStun(FoodItem hitFood, Vector3 stunDirect)
    {
        if (hitFood.owner != playerNum)
        {
            dirtyAmt += 1f;
            hitFood.doSplat(true);

            if (holdingFood)
            {
                dropOffset = -stunDirect / 2f;
                doThrow(Vector2.zero, true);
            }


            canDoAction = false;
            pushDirect = stunDirect * 8f;
            pushcount = .25f;

        }
        else
        {
            pickupFood(hitFood);
        }
        
       
    }


    private bool pickupFood(FoodItem newfood)
    {

        if (holdingFood)
        {
            //lol too bad
            return false;
        }
        else
        {
            holdingFood = true;
            heldFood = newfood.GetFoodType();
            heldItemImg.sprite = gc.foodSprites[(int)heldFood];
            //Debug.Log(playerNum + " picked up a " + heldFood);
            Destroy(newfood.gameObject);
            AudioClip sfx = SoundManager.instance.pickup;
            GetComponent<AudioSource>().PlayOneShot(sfx);
            return true;
        }

    }


    private void doThrow(Vector2 throwdirect, bool isdropped = false)
    {
        var thrown = Instantiate(foodBase);
        thrown.transform.position = transform.position + (Vector3)throwdirect * 1.5f;
        
        var foodc = thrown.GetComponent<FoodItem>();
        foodc.SetFoodType(heldFood,isdropped);
        foodc.owner = playerNum;
        if (!isdropped)
        {
            foodc.throwfood(throwdirect * throwStrength);

            AudioClip sfx;
            if (playerNum == 0)
                sfx = SoundManager.instance.P1Throw();
            else
                sfx = SoundManager.instance.P2Throw();
            GetComponent<AudioSource>().PlayOneShot(sfx);

        }
        else
        {
            thrown.transform.position += dropOffset;
        }
       

        holdingFood = false;
    }



}
