﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodItem : MonoBehaviour {


    public enum FoodTypes { eggs, milk, seasoning,  }


    private FoodTypes current_food;

    [SerializeField]
    private SpriteRenderer foodSR;

    [SerializeField]
    private Color DecayColor;

    private bool isDecaying;

    private float countdowntimer;
    private float decaytime = 5f;


	// Use this for initialization
	void Start () {
        StartDecay();
	}
	
	// Update is called once per frame
	void Update () {
		

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
    public void SetFoodType(FoodTypes newtype)
    {
        current_food = newtype;
    }

    //for when food lands on the floor - start the 5 second rule countdown
    public void StartDecay()
    {
        if (!isDecaying)
        {
            isDecaying = true;
            countdowntimer = decaytime;

        }
    }


    //is this food item currently decaying?
    public bool IsCurrentlyDecaying()
    {
        return isDecaying;
    }


    public float GetDecayPercentage()
    {
        return Mathf.Clamp01((decaytime - countdowntimer) / decaytime);
    }

    //do something here when the food goes bad?!
    private void GoBad()
    {

        //remove this script so it doesn't update anymore and just stays as a sprite renderer
        Destroy(this);

        //remove any colliders or whatever here too 
    }


}
