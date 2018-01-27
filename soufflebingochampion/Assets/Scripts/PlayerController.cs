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
    private SpriteRenderer pimg, aimIndicator;
    [SerializeField]
    private GameObject foodBase;


    private Vector2 aim = Vector2.right;

    private float moveSpeed = 4f;
    private float throwStrength = 3f;

    private float throwCountdown;
    private float throwTime = .25f;

   

    private bool readyToThrow = true;

    private bool holdingFood = false;


    private FoodItem.FoodTypes heldFood = FoodItem.FoodTypes.cheese ;

	// Use this for initialization
	void Start () {
        gc = GameController.instance;
        im = InputManager.instance;

        //debug testing
        holdingFood = true;

        if (playerNum == 0)
        {
            gameObject.layer = 8;
        }
        else
        {
            gameObject.layer = 9;
            heldFood = FoodItem.FoodTypes.eggs;
        }

	}
	
	// Update is called once per frame
	void Update () {

        //handle movement
        Vector2 movevect = im.getPlayerMove(playerNum);
        rb2d.velocity = movevect * moveSpeed;
        
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
            if (theFood != null)
            {
                if (!theFood.isbeingthrown)
                {
                    pickupFood(theFood);
                }
                else
                {

                    Vector3 stunDirect = transform.position - collision.gameObject.transform.position;
                    dostun(theFood, stunDirect);
                }
           
            }
        }
    }


    private void dostun(FoodItem hitfood, Vector3 stunDirect)
    {

        if (holdingFood)
        {
            doThrow(Vector2.zero, true);
        }


        Destroy(hitfood.gameObject);

        rb2d.AddForce(stunDirect * 500f);
    }


    private void pickupFood(FoodItem newfood)
    {

        if (holdingFood)
        {

        }
        else
        {
            holdingFood = true;
            heldFood = newfood.GetFoodType();
            Destroy(newfood.gameObject);
        }

    }


    private void doThrow(Vector2 throwdirect, bool isdropped = false)
    {
        var thrown = Instantiate(foodBase);
        thrown.transform.position = transform.position;
        if (playerNum == 0)
        {
            thrown.layer = 8;
        }
        else
        {
            thrown.layer = 9;
        }
        var foodc = thrown.GetComponent<FoodItem>();
        foodc.SetFoodType(heldFood);
        if (!isdropped)
        {
            foodc.throwfood(throwdirect * throwStrength);
        }
       

        holdingFood = false;
    }



}
