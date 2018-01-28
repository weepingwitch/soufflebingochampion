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
   

    private bool readyToThrow = true;

    private bool holdingFood = false;


    private FoodItem.FoodTypes heldFood = FoodItem.FoodTypes.cheese ;

	// Use this for initialization
	void Start () {
        gc = GameController.instance;
        im = InputManager.instance;

       

        if (playerNum == 0)
        {
            gameObject.layer = 8;
        }
        else
        {
            gameObject.layer = 9;
           
        }

      

    }
	
	// Update is called once per frame
	void Update () {

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
            pushcount -= Time.deltaTime;
            rb2d.velocity += pushDirect;
        }


        if (slideCountdown > 0)
        {
            slideCountdown -= Time.deltaTime;
            rb2d.velocity += slideDirection;
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
        if (readyToThrow)
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
                if (!theFood.isbeingthrown)
                {
                    pickupFood(theFood);
                }
                else
                {

                    Vector3 stunDirect = transform.position - collision.gameObject.transform.position;
                    DoStun(theFood, stunDirect);
                }
           
            }
        }
        if (collision.gameObject.CompareTag("slippery"))
        {
            //Debug.Log("slipping!");

            slideCountdown = .05f;
        }
    }

    //called when hit by a food
    private void DoStun(FoodItem hitfood, Vector3 stunDirect)
    {

        hitfood.doSplat(true);

        if (holdingFood)
        {
            dropOffset = -stunDirect/2f;
            doThrow(Vector2.zero, true);
        }



        pushDirect = stunDirect*8f;
        pushcount = .25f;
       
    }


    private void pickupFood(FoodItem newfood)
    {

        if (holdingFood)
        {
            //lol too bad
        }
        else
        {
            holdingFood = true;
            heldFood = newfood.GetFoodType();
            heldItemImg.sprite = gc.foodSprites[(int)heldFood];
            //Debug.Log(playerNum + " picked up a " + heldFood);
            Destroy(newfood.gameObject);
        }

    }


    private void doThrow(Vector2 throwdirect, bool isdropped = false)
    {
        var thrown = Instantiate(foodBase);
        thrown.transform.position = heldItemImg.transform.position + (Vector3)throwdirect/2f ;
        if (playerNum == 0)
        {
            thrown.layer = 8;
        }
        else
        {
            thrown.layer = 9;
        }
        var foodc = thrown.GetComponent<FoodItem>();
        foodc.SetFoodType(heldFood,isdropped);
        foodc.owner = playerNum;
        if (!isdropped)
        {
            foodc.throwfood(throwdirect * throwStrength);
        }
        else
        {
            thrown.transform.position += dropOffset;
        }
       

        holdingFood = false;
    }



}
