using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stove : MonoBehaviour {

    [SerializeField]
    private BingoBoard myBoard;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}



    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("fooditem"))
        {


            var theFood = collision.gameObject.GetComponent<FoodItem>();
            if (theFood != null)
            {
                if (!theFood.IsCurrentlyDecaying())
                myBoard.AddItem(theFood.GetFoodType());
                Destroy(theFood.gameObject);
            }
        }
    }
}
