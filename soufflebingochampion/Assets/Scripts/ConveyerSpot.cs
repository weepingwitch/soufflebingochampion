using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyerSpot : MonoBehaviour {


    private Vector3 origPosition;
    public Vector3 moveVect;
    private float moveSpeed = 1;
    private bool isMoving = false;
    private float moveDistance;

	// Use this for initialization
	void Start () {
        
	}


    public void StartMoving(Vector3 direct, float newMoveDist = 4)
    {
        origPosition = transform.position;
        moveVect = direct;
        isMoving = true;
        moveDistance = newMoveDist;
        //Debug.Log(moveDistance);
    }
	
	// Update is called once per frame
	void Update () {
		if (isMoving)
        {
            transform.position += moveVect * moveSpeed * Time.deltaTime;

            if (Vector2.Distance(transform.position,origPosition) > moveDistance)
            {
                Destroy(gameObject);

            }
        }
	}
}
