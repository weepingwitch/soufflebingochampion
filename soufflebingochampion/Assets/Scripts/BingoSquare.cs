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
    public void setFood(FoodItem.FoodTypes newfood)
    {
        if (gc == null)
        {
            gc = GameController.instance;
        }

        foodObject = new GameObject("foodItem");
        foodObject.transform.parent = transform;
        foodObject.transform.localPosition = new Vector3(0, 0, 0);
        foodObject.transform.localScale = new Vector3(0.6f, 0.6f, 1);
        foodSpriteRenderer = foodObject.AddComponent<SpriteRenderer>() as SpriteRenderer;
        foodSpriteRenderer.sortingLayerName = "OVEN";
        foodSpriteRenderer.sortingOrder = 5;
        foodSpriteRenderer.sprite = gc.foodSprites[(int)newfood];

    }

    //set the color based on which player this bingo board belongs to
    public void setPlayerId(int id)
    {
        SpriteRenderer sp = GetComponent<SpriteRenderer>();
        // chef
        if (id == 0)
        {
            Color chefBlue = new Color(97/255f, 96/255f, 251/255f, 100/255f);
            sp.color = chefBlue;
            chefBlue.a = 1;
            foodSpriteRenderer.color = chefBlue;
        }

        // guy
        else
        {
            //Color guyRed = new Color(199/255f, 0, 8/255f, 100/255f);
            Color guyRed = new Color(199/255f, 51/255f, 57/255f, 100/255f);
            sp.color = guyRed;
            guyRed.a = 1;
            foodSpriteRenderer.color = guyRed;
        }

    }

    //mark the spot as filled
    public void markFilled()
    {
        check.enabled = true;
    }

}
