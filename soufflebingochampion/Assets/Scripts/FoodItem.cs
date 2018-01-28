﻿using System.Collections;
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
    private GameObject shardObj, splatObj;

    private bool isDecaying;

    private float countdowntimer;
    private float decaytime = 5f;


    private float throwtime;
    private float throwtimer;
    private Vector2 origvel;

    private bool isGoingIntoOven;


    public int owner;

    private GameController gc;


    public bool isbeingthrown = false;

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
        isbeingthrown = true;


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



        if (isbeingthrown)
        {
            throwtimer -= Time.deltaTime;
            if (throwtimer < 1f)
            {
                rb2d.velocity = Vector2.Lerp(origvel, Vector2.zero, 1-throwtimer);

                if (throwtimer <= 0f)
                {
                    isbeingthrown = false;
                    rb2d.velocity = Vector2.zero;
                    StartDecay();
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
                GoBad();
            }

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

    public void doSplat(bool isChef = false)
    {
        AudioClip splatSound;
        bool doShatter = false;
      

        switch (current_food)
        {
            case FoodTypes.crab:
            case FoodTypes.eggs:
                splatSound = null; //crab egg
                break;
            case FoodTypes.vinegar:
                doShatter = true;
                splatSound = null; // large glass
                break;
            case FoodTypes.salt:
            case FoodTypes.pepper:
            case FoodTypes.paprika:
                doShatter = true;
                splatSound = null; //small glass
                break;
            default:
                splatSound = null; //implement here
                break;
        }

        //play the sound here - splatSound



        if (doShatter)
        {
            
            Color splatColor = Color.white;
            float splatSize = 1f;
            switch(current_food){
                case FoodTypes.vinegar:
                    splatColor = new Color(165f / 255f, 128f / 255f, 89f / 255f, .5f);
                    break;
                case FoodTypes.salt:
                    splatSize = .3f;
                    splatColor.a = .4f;
                    break;
                case FoodTypes.pepper:
                    splatSize = .3f;
                    splatColor = Color.gray;
                    splatColor.a = .4f;
                    break;
                case FoodTypes.paprika:
                    splatSize = .4f;
                    splatColor = Color.red;
                    splatColor.a = .4f;
                    break;
                default:
                    break;
            }

            // instantiage glass shatter
           // var myShatter = Instantiate(shardObj);
           // myShatter.transform.position = transform.position;


            //instantly turn into splat, without playing rotting sound
            GoBad(false);
            foodSR.color = splatColor;
            transform.localScale *= splatSize;

            isChef = false;


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
        }


        gameObject.tag = "slippery";
        foodSR.color = new Color(DecayColor.r, DecayColor.g, DecayColor.b, .5f);
        gameObject.layer = 10;

        //remove this script so it doesn't update anymore and just stays as a sprite renderer
        Destroy(this);


    }


}
