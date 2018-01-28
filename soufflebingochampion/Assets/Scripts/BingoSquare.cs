using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BingoSquare : MonoBehaviour {

    static GameController gc;
    private GameObject foodObject;
    private SpriteRenderer foodSpriteRenderer;
    private SpriteRenderer check;

    [SerializeField]
    private SpriteRenderer checkPrefab;

	// Use this for initialization
	void Start () {
        check = Instantiate(checkPrefab, transform);
        check.enabled = false;
	}

    //set the sprite based on what food is in this spot
    public void setfood(FoodItem.FoodTypes newfood)
    {
        if (gc == null)
        {
            gc = GameController.instance;
        }

        foodObject = new GameObject("foodItem");
        foodObject.transform.parent = transform;
        foodObject.transform.localPosition = new Vector3(0, 0, 0);
        foodObject.transform.localScale = new Vector3(1, 1, 1);
        foodSpriteRenderer = foodObject.AddComponent<SpriteRenderer>() as SpriteRenderer;
        foodSpriteRenderer.sortingLayerName = "STOVE";
        foodSpriteRenderer.sortingOrder = 5;
        foodSpriteRenderer.sprite = gc.foodSprites[(int)newfood];

    }


    //mark the spot as filled
    public void markFilled()
    {
        check.enabled = true;
    }

}
