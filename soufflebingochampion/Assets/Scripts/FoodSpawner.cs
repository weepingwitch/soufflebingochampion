using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour {


    [SerializeField]
    private GameObject foodBase, conveyerSpotBase;
    [SerializeField]
    private Vector3 moveDirect;

    private int foodcount;

	// Use this for initialization
	void Start () {
        InvokeRepeating("SpawnAFood", .2f, 1f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    private void SpawnAFood()
    {
        var conveySpot = Instantiate(conveyerSpotBase);
        conveySpot.transform.position = transform.position;
        foodcount++;
        if (foodcount > 2)
        {
            foodcount = 0;
            var spawnedFood = Instantiate(foodBase, conveySpot.transform);
            spawnedFood.transform.localPosition = new Vector3(0, -.5f, 0);
            spawnedFood.GetComponent<FoodItem>().SetRandom();
        }
        
        conveySpot.GetComponent<ConveyerSpot>().StartMoving(moveDirect, 14f);
    }

}
