using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BingoSquare : MonoBehaviour {


    static GameController gc;

    [SerializeField]
    private SpriteRenderer bg, fooditem, check;

	// Use this for initialization
	void Start () {
        
        check.enabled = false;
	}

    //set the sprite based on what food is in this spot
    public void setfood(FoodItem.FoodTypes newfood)
    {
      
        if (gc == null)
        {
            gc = GameController.instance;
        }

        fooditem.sprite = gc.foodSprites[(int)newfood];
        
    }


    //mark the spot as filled
    public void markFilled()
    {

        check.enabled = true;
    }

}
