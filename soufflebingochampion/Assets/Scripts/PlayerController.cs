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

    public float movespeed = 4f;
    public float throwstrength = 3f;

    private float throwcountdown;
    private float throwtime = .25f;

    

    private bool readytothrow = true;

    private bool holdingfood = false;


    private FoodItem.FoodTypes heldFood = FoodItem.FoodTypes.cheese ;

	// Use this for initialization
	void Start () {
        gc = GameController.instance;
        im = InputManager.instance;

        //debug testing
        holdingfood = true;

	}
	
	// Update is called once per frame
	void Update () {

        //handle movement
        Vector2 movevect = im.getPlayerMove(playerNum);
        rb2d.velocity = movevect * movespeed;
        
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
        if (readytothrow)
        {
           
            if (im.getPlayerShoot(playerNum) && holdingfood)
            {
                Debug.Log(playerNum + " threw a " + heldFood);
                readytothrow = false;
                throwcountdown = throwtime;
                doThrow(aim);
            }
        }
        else
        {
            throwcountdown -= Time.deltaTime;
            if (throwcountdown <= 0)
            {
                readytothrow = true;
            }
        }

	}


    public void doThrow(Vector2 throwdirect)
    {
        var thrown = Instantiate(foodBase);
        thrown.transform.position = transform.position;
        var foodc = thrown.GetComponent<FoodItem>();
        foodc.SetFoodType(heldFood);
       
        foodc.throwfood(throwdirect*throwstrength);
    }



}
