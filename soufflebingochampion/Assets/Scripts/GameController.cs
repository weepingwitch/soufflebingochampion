using System.Collections;
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
    public void PlayerWon(int playerNum)
    {
        resultsScreen.SetActive(true);
        resultsScreen.GetComponentInChildren<Text>().text = "Player " + (playerNum + 1) + " won!!";
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(im.getPlayerMove(0));
	}
}
