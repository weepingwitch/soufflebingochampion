using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BingoBoard : MonoBehaviour {

    private GameController gc;
    private BingoSquare[] squares;

    private BitArray myBoard;
    private BitArray[] solutions;

    private FoodItem.FoodTypes[] goalboard;

    private List<FoodItem.FoodTypes> winningfoods;

    [SerializeField]
    private int myId;

    [SerializeField]
    private BingoSquare squareBase;

    //called before Start
    private void Awake()
    {
        //initialize the values
        myBoard = new BitArray(25);
        winningfoods = new List<FoodItem.FoodTypes>();
        solutions = new BitArray[12];
        goalboard = new FoodItem.FoodTypes[25];
        squares = new BingoSquare[25];

        //generate the array of bingo solutions
        GenerateSolutions();

    }


    // Use this for initialization
    void Start () {
        gc = GameController.instance;

        //generate your card
        GenerateRandomBoard();

        //DebugCheckBoard();

        //DebugPrintGoal();

        //AddItem(FoodItem.FoodTypes.a);
        //DebugPrintBoard(myboard);

	}


    // Update is called once per frame
    void Update () {

	}


    //called to add an ingredient to the bingo board
    public bool AddItem(FoodItem.FoodTypes newitem)
    {
        bool res = false;
        //compare new food to goal board, mark spot as completed if match
        for (int i = 0; i < goalboard.Length; i++)
        {
            if (newitem == goalboard[i])
            {
                myBoard.Set(i, true);
                squares[i].markFilled();
                res = true;
            }
        }
        
        //check if there is a bingo
        if (CheckForBingo())
        {
            DoWin();
        }
        return res;

    }

    //Fill the board with random foods !!
    private void GenerateRandomBoard()
    {

        var randomlist =  new List<FoodItem.FoodTypes>((FoodItem.FoodTypes[])System.Enum.GetValues(typeof(FoodItem.FoodTypes)));
        Shuffle(randomlist);


        for (int i = 0; i < goalboard.Length; i++)
        {
            //will need to change this logic once we have more foods
            goalboard[i] = randomlist[i];
            squares[i] = Instantiate(squareBase, transform);
            squares[i].transform.localPosition = new Vector3(i/5, i%5, 0);
            squares[i].setFood(randomlist[i]);
            squares[i].setPlayerId(myId);
        }
    }

    //set this as belonging to player 1 or 2 or whatever
    public void SetPlayerNumber(int newplayernum)
    {
        myId = newplayernum;
    }

    //return which player this belongs to
    public int GetPlayerNumber()
    {
        return myId;
    }


    //called when this person gets a bingo
    private void DoWin()
    {

        gc.PlayerWon(myId, winningfoods.ToArray()) ;
        #if UNITY_EDITOR
             //   Debug.Log("player " + myId + " won!!!");
        #endif

    }


    private void SetWinningIngredients(BitArray winningpattern)
    {
        for (int i = 0; i < winningpattern.Length; i++)
        {
            if (winningpattern[i])
            {
                winningfoods.Add(goalboard[i]);
            }
        }

        winningfoods.Sort();
        winningfoods.Reverse();


    }



    //called upon updating board, to check for a bingo
    private bool CheckForBingo()
    {
        int cursolution = 0;


        int mycardint = ConvertBitArray(myBoard);


        for (int i = 0; i < solutions.Length; i++)
        {

            cursolution = ConvertBitArray(solutions[i]);
            if (cursolution == (mycardint & cursolution))
            {
                //DebugPrintBoard(myboard);
                //DebugPrintBoard(solutions[i]);
                // DebugPrintBoard(ConvertBitArray(new BitArray((mycardint & cursolution)));

                SetWinningIngredients(solutions[i]);


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

    private void DebugPrintGoal()
    {
#if UNITY_EDITOR
        foreach (FoodItem.FoodTypes i in goalboard)
        {
            Debug.Log(i.ToString());
        }
#endif
    }


    //check bingochecker
    private void DebugCheckBoard()
    {
#if UNITY_EDITOR

        myBoard = MakeBitArray(new int[] { 1, 0, 0, 1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, });

        Debug.Log("test 0 - should be false: " + CheckForBingo());
        myBoard = MakeBitArray(new int[] { 1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, });
        Debug.Log("test 1 - should be true: " + CheckForBingo());

#endif
    }


    private void Shuffle<T>( IList<T> list)
    {
        System.Random rng = new System.Random((int)Time.time*100 + Random.Range(0,9999));
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

}
