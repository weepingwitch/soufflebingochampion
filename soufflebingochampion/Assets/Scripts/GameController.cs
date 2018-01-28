﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {


    public Sprite[] foodSprites;
    public Sprite[] decayedFoodSprites;

    public static GameController instance;


    [SerializeField]
    private GameObject resultsScreen;

    private InputManager im;

    //set the static instance, make sure there is only one
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Use this for initialization
    void Start () {
        im = InputManager.instance;
		
	}

    //called from the bingo board when someone has won!!!
    public void PlayerWon(int playerNum, FoodItem.FoodTypes[] winningfoods)
    {
        resultsScreen.SetActive(true);

        string winningDescription = ProcessWinningFood(winningfoods);

        resultsScreen.GetComponentInChildren<Text>().text = "Player " + (playerNum + 1) + " won!!\nYou made a" + winningDescription + " souffle!";
    }



    private string ProcessWinningFood(FoodItem.FoodTypes[] thefoods)
    {
        string res = "";

        res = " " + replaceTier(thefoods[0]);

        string second = res;

        int i = 1;
        
        while (second == res)
        {
            second = replaceTier(thefoods[i]);
            i++;

            if (i >= 5)
            {
                second = "";
            }
        }

        if (second != " plain")
            res = res + second;


        return res;
    }


    private string replaceTier(FoodItem.FoodTypes thefood)
    {
        string res = "";

        switch (thefood)
        {
            case FoodItem.FoodTypes.pepper:
            case FoodItem.FoodTypes.paprika:
                res = " spicy";
                break;

            case FoodItem.FoodTypes.potato:
            case FoodItem.FoodTypes.sweetpotato:
                res = " starchy";
                break;
            case FoodItem.FoodTypes.raspberry:
            case FoodItem.FoodTypes.strawberry:
            case FoodItem.FoodTypes.apple:
            case FoodItem.FoodTypes.lemon:
                res = " fruity";
                break;
            case FoodItem.FoodTypes.nutmeg:
            case FoodItem.FoodTypes.cinnamon:
                res = " spiced";
                break;
            case FoodItem.FoodTypes.oregano:
            case FoodItem.FoodTypes.parsley:
                res = " seasoned";
                break;
            case FoodItem.FoodTypes.chocolate:
            case FoodItem.FoodTypes.crab:
            case FoodItem.FoodTypes.cheese:
            case FoodItem.FoodTypes.vinegar:
                res = " " + thefood.ToString();
                break;
            

            default:
                res = " plain";
                break;
        }


        return res;
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(im.getPlayerMove(0));
	}
}
