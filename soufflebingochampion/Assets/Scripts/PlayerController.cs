using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


    //should be 0 or 1
    public int playernum;


    
    private GameController gc;
    private InputManager im;
    [SerializeField]
    private Rigidbody2D rb2d;
    [SerializeField]
    private SpriteRenderer pimg, aimindicator;


    private Vector2 aim = Vector2.right;

    public float movespeed = 4f;

	// Use this for initialization
	void Start () {
        gc = GameController.instance;
        im = InputManager.instance;
	}
	
	// Update is called once per frame
	void Update () {

        //handle movement
        Vector2 movevect = im.getPlayerMove(playernum);
        rb2d.velocity = movevect * movespeed;
        
        //handle aiming and sprite direction
        Vector2 aimdirect = im.getPlayerAim(playernum);
        if (Mathf.Abs(aimdirect.magnitude) > .1f)
        {
            aim = aimdirect.normalized;

                pimg.flipX = (aim.x < 0);
            
        }
        else if (Mathf.Abs(movevect.magnitude) > .1f)
        {
            aim = movevect.normalized;
            pimg.flipX = (aim.x < 0);
        }

        //handle aim indicator
        aimindicator.transform.localPosition = aim;
        aimindicator.transform.up = aim;





	}




}
