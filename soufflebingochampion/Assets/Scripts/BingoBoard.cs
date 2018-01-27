using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BingoBoard : MonoBehaviour {

    private int myid;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    //called to add an ingredient to the bingo board
    public void AddItem(FoodItem.FoodTypes newitem)
    {

        //do something here to add it to the board lol

        if (CheckForBingo())
        {
            DoWin();
        }


    }

    //set this as belonging to player 1 or 2 or whatever
    public void SetPlayerNumber(int newplayernum)
    {
        myid = newplayernum;
    }

    //return which player this belongs to 
    public int GetPlayerNumber()
    {
        return myid;
    }


    //called when this person gets a bingo
    private void DoWin()
    {

        #if UNITY_EDITOR
                Debug.Log("player " + myid + " won!!!");
        #endif

    }


    //called upon updating board, to check for a bingo
    private bool CheckForBingo()
    {
        bool result = false;



        return result;

    }

}
