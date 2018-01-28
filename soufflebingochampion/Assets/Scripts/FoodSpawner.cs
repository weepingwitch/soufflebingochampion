using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour {


    [SerializeField]
    private GameObject foodBase, conveyerSpotBase, beltImage;
    [SerializeField]
    private bool movingRight;

    private Vector3 moveDirect;

    [SerializeField]
    private float conveyerLength;



    private int foodcount;

	// Use this for initialization
	void Start () {
        InvokeRepeating("SpawnAFood", 0f, 1f);


        beltImage.transform.localScale = new Vector3(conveyerLength, 1);
        int movefactor = -1;
        
        if (movingRight)
        {
            movefactor = 1;
        }
        moveDirect = new Vector3(movefactor, 0);
        beltImage.transform.localPosition = new Vector3(((conveyerLength / 2f)-.5f)*movefactor, 0, 0);
        Vector3 spotPos = Vector3.zero;
        for (int i = 0; i < conveyerLength; i++)
        {
            var conveySpot = Instantiate(conveyerSpotBase);
            conveySpot.transform.position = transform.position + new Vector3(0, .5f, 0) + spotPos;
            spotPos += Vector3.right * movefactor;
            conveySpot.GetComponent<ConveyerSpot>().StartMoving(moveDirect, conveyerLength);

        }


	}
	
	// Update is called once per frame
	void Update () {
		
	}


    private void SpawnAFood()
    {
        var conveySpot = Instantiate(conveyerSpotBase);
        conveySpot.transform.position = transform.position + new Vector3(0,.5f,0);
        foodcount++;
        if (foodcount > 1)
        {
            foodcount = 0;
            var spawnedFood = Instantiate(foodBase, conveySpot.transform);
            spawnedFood.transform.localPosition = new Vector3(0, -.5f, 0);
            spawnedFood.GetComponent<FoodItem>().SetRandom();
        }
        
        conveySpot.GetComponent<ConveyerSpot>().StartMoving(moveDirect, conveyerLength);
    }

}
