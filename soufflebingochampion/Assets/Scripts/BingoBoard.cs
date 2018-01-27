using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BingoBoard : MonoBehaviour {

    private int myid;

    private BitArray myboard;

    private BitArray[] solutions;


    //called before Start
    private void Awake()
    {

        myboard = new BitArray(25);
        solutions = new BitArray[12];
       
    }


    // Use this for initialization
    void Start () {
        //DebugPrintBoard();
        //Debug.Log(ConvertBitArray(myboard));

        DebugCheckBoard();
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
    private void DebugPrintBoard()
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


    //check bingochecker
    private void DebugCheckBoard()
    {
#if UNITY_EDITOR
        myboard.Set(3, true);
        myboard.Set(8, true);


        solutions[0] = new BitArray(25);
        solutions[0].Set(2, true);

        solutions[1] = new BitArray(25);
        solutions[1].Set(8, true);

        
        Debug.Log("test 0 - should be true: " + CheckForBingo());
        myboard.Set(8, false);
        Debug.Log("test 1 - should be false: " + CheckForBingo());

#endif
    }





    //called upon updating board, to check for a bingo
    //still working on this, not sure if it will work yet lol
    private bool CheckForBingo()
    {
        int cursolution = 0;


        int mycardint = ConvertBitArray(myboard);


        for (int i = 0; i < solutions.Length; i++)
        {

            cursolution = ConvertBitArray(solutions[i]);
            if (cursolution == (mycardint & cursolution))
                return true;
        }
        return false;
    }



    //convert bitarray to int
    private int ConvertBitArray(BitArray mybits)
    {
        if (mybits == null)
            return -1;
        if (mybits.Length > 32)
            return -2;

        var result = new int[1];
        mybits.CopyTo(result, 0);
        return result[0];
    }

}
