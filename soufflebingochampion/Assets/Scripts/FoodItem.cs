using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodItem : MonoBehaviour {


    public enum FoodTypes { eggs, butter, cheese, milk, seasoning, a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z  }

    


    private FoodTypes current_food;

    [SerializeField]
    private SpriteRenderer foodSR;

    [SerializeField]
    private Color DecayColor;

    [SerializeField]
    private Rigidbody2D rb2d;

    private bool isDecaying;

    private float countdowntimer;
    private float decaytime = 5f;


    private float throwtime;
    private float throwtimer;
    private Vector2 origvel;
    

    private GameController gc;


    public bool isbeingthrown = true;

	// Use this for initialization
	void Start() { 
         gc = GameController.instance;
        //SetFoodType(FoodTypes.milk);

        //throwfood(Vector2.left*2f);
        
 

	}

    //call this to throw the food
    public void throwfood(Vector2 direct, float duration = 2f)
    {

        rb2d.velocity = direct;
        origvel = direct;
        throwtime = duration;
        throwtimer = throwtime;
        isbeingthrown = true;
        

    }

	
	// Update is called once per frame
	void Update () {
		
        if (isbeingthrown)
        {
            throwtimer -= Time.deltaTime;
            if (throwtimer < 1f)
            {
                rb2d.velocity = Vector2.Lerp(origvel, Vector2.zero, 1-throwtimer);

                if (throwtimer <= 0f)
                {
                    isbeingthrown = false;
                    rb2d.velocity = Vector2.zero;
                    StartDecay();
                }

            }

            
            
        }



        //process decaying food
        if (isDecaying)
        {
            countdowntimer -= Time.deltaTime;

            //change sprite here i guess
            foodSR.color = Color.Lerp(Color.white, DecayColor, GetDecayPercentage());


            //check to see if it is fully decayed
            if (countdowntimer <= 0)
            { 
                GoBad();
            }

        }

	}





    //get what the current food item is
    public FoodTypes GetFoodType()
    {
        return current_food;
    }

    //set what type of food the item is
    public void SetFoodType(FoodTypes newtype)
    {
        if (gc == null)
            gc = GameController.instance;
        current_food = newtype;
        foodSR.sprite = gc.foodSprites[(int)newtype];
        Invoke("becomePickUppable", 1f);

    }

    //for when food lands on the floor - start the 5 second rule countdown
    public void StartDecay()
    {
        if (!isDecaying)
        {
           
            isDecaying = true;
            countdowntimer = decaytime;

        }
    }


    //is this food item currently decaying?
    public bool IsCurrentlyDecaying()
    {
        return isDecaying;
    }

    //return how much (0f-1f) the food has decayed
    public float GetDecayPercentage()
    {
        return Mathf.Clamp01((decaytime - countdowntimer) / decaytime);
    }

    private void becomePickUppable()
    {
        gameObject.layer = 10;
    }

    //do something here when the food goes bad?!
    private void GoBad()
    {

        gameObject.tag = "slippery";
        

        //remove this script so it doesn't update anymore and just stays as a sprite renderer
        Destroy(this);

       
    }


}
