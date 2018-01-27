using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BingoBoard : MonoBehaviour {

    private int myid;

    private bool[] myboard;


    //called before Start
    private void Awake()
    {
        myboard = new bool[25];

    }


    // Use this for initialization
    void Start () {
        DebugBoard();
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

    //output board value
    private void DebugBoard()
    {
#if UNITY_EDITOR
        string boardstr = "";
        foreach (bool bit in myboard)
        {
            if (bit)
            {
                boardstr += "1";
            }
            else
            {
                boardstr += "0";
            }
        }

        Debug.Log(boardstr);
        #endif
    }


    //called upon updating board, to check for a bingo
    private bool CheckForBingo()
    {
        bool result = false;




        //do the bingo checking logic here lol

        return result;

    }

    //still working on this, not sure if it will work yet lol
    private bool CheckSolution(int mycard, int[] solutions)
    {
        for (int i = 0; i < solutions.Length; i++)
            if (solutions[i] == (mycard & solutions[i]))
                return true;
        return false;
    }

}
