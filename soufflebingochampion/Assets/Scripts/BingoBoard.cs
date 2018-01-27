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

        GenerateSolutions();
       
    }


    // Use this for initialization
    void Start () {


        DebugCheckBoard();


        
	}

   
    // Update is called once per frame
    void Update () {
		
	}


    //called to add an ingredient to the bingo board
    public void AddItem(FoodItem.FoodTypes newitem)
    {

        //do something here to add it to the board lol


        //check if there is a bingo
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
        int cursolution = 0;


        int mycardint = ConvertBitArray(myboard);


        for (int i = 0; i < solutions.Length; i++)
        {

            cursolution = ConvertBitArray(solutions[i]);
            if (cursolution == (mycardint & cursolution))
            {
                //DebugPrintBoard(myboard);
                //DebugPrintBoard(solutions[i]);
               // DebugPrintBoard(ConvertBitArray(new BitArray((mycardint & cursolution)));
                
                return true;
                
            }
                
        }
        return false;
    }



    // generate array of possible solutions
    private void GenerateSolutions()
    {
        //horizontals
        solutions[0] = MakeBitArray(new int[] { 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0});
        solutions[1] = MakeBitArray(new int[] { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0});
        solutions[2] = MakeBitArray(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0});
        solutions[3] = MakeBitArray(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0});
        solutions[4] = MakeBitArray(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1});

        //verticals
        solutions[5] = MakeBitArray(new int[] { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0});
        solutions[6] = MakeBitArray(new int[] { 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0});
        solutions[7] = MakeBitArray(new int[] { 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0});
        solutions[8] = MakeBitArray(new int[] { 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0});
        solutions[9] = MakeBitArray(new int[] { 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1});


        //diagonals
        solutions[10] = MakeBitArray(new int[] { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1});
        solutions[11] = MakeBitArray(new int[] { 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0});


    }


    private BitArray MakeBitArray(int[] myints)
    {
        BitArray res = new BitArray(25);


        for (int i = 0; i < myints.Length; i++)
        {
            if (myints[i] == 1)
            {
                res.Set(i, true);
            }
            else
            {
                res.Set(i, false);
            }
        }

        return res;
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


    //output board value
    private void DebugPrintBoard(BitArray mybits)
    {
#if UNITY_EDITOR
        string boardstr = "";
        foreach (bool bit in mybits)
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

        myboard = MakeBitArray(new int[] { 1, 0, 0, 1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, });
        
        Debug.Log("test 0 - should be false: " + CheckForBingo());
        myboard = MakeBitArray(new int[] { 1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, });
        Debug.Log("test 1 - should be true: " + CheckForBingo());

#endif
    }


}
