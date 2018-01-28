using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : MonoBehaviour {

    [SerializeField]
    private BingoBoard myBoard;
    [SerializeField]
    private int ownerNum;


    private GameController gc;

	// Use this for initialization
	void Start () {
        gc = GameController.instance;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //process the food item
    public void DoCook(FoodItem theFood)
    {
        bool isGood = myBoard.AddItem(theFood.GetFoodType());

        //do something here to indicate success
        if (isGood)
        {

        }


        Destroy(theFood.gameObject);
    }


    //start receiving the food item
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("fooditem"))
        {


            var theFood = collision.gameObject.GetComponent<FoodItem>();
            if (theFood != null)
            {
                if (!theFood.IsCurrentlyDecaying() && theFood.owner == ownerNum)
                

                theFood.goIntoOven(this);
                //Destroy(theFood.gameObject);
            }
        }
    }
}
