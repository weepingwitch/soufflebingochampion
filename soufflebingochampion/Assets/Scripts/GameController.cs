using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {


    public Sprite[] foodSprites;
    public Sprite[] decayedFoodSprites;

    public static GameController instance;

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
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(im.getPlayerMove(0));
	}
}
