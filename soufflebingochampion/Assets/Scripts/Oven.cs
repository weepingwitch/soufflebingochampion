using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Oven : MonoBehaviour {

    [SerializeField]
    public int ownerNum;
    [SerializeField]
    private BingoBoard myBoard;
    [SerializeField]
    public SpriteRenderer successMark;
    [SerializeField]
    public SpriteRenderer failureMark;
    [SerializeField]
    public GameObject statusMessage;


    private GameController gc;

	// Use this for initialization
	void Start () {
        gc = GameController.instance;
        successMark.enabled = false;
        failureMark.enabled = false;
        statusMessage.SetActive(false);

        // set the color of the statusMessage background image
       // Graphic image = statusMessage.GetComponent<Image>().GetComponent<Graphic>();
        // chef
        if (ownerNum == 0)
        {
            Color chefBlue = new Color(97/255f, 96/255f, 251/255f, 100/255f);
        //    image.color = chefBlue;
        }

        // guy
        else
        {
            //Color guyRed = new Color(199/255f, 0, 8/255f, 100/255f);
            Color guyRed = new Color(199/255f, 51/255f, 57/255f, 100/255f);
         //   image.color = guyRed;
        }
	}

	// Update is called once per frame
	void Update () {

	}

    //process the food item
    public void DoCook(FoodItem theFood)
    {
        BingoBoard.MatchResult result = myBoard.AddItem(theFood.GetFoodType());
        int popupDuration = 1;

        switch (result)
        {
            case BingoBoard.MatchResult.success:
                StartCoroutine(ShowPopup(successMark, popupDuration));
                break;
            case BingoBoard.MatchResult.repeat:
               // statusMessage.GetComponentInChildren<Text>().text = "Ingredient already added";
                StartCoroutine(ShowPopup(statusMessage, popupDuration));
                StartCoroutine(ShowPopup(failureMark, popupDuration));
                break;
            case BingoBoard.MatchResult.failure:
              //  statusMessage.GetComponentInChildren<Text>().text = "Wrong ingredient";
                StartCoroutine(ShowPopup(statusMessage, popupDuration));
                StartCoroutine(ShowPopup(failureMark, popupDuration));
                break;
            default:
                break;
        }

        Destroy(theFood.gameObject);
    }

    // display an object for the specified period of time
    IEnumerator ShowPopup (GameObject popup, float delay) {
         popup.SetActive(true);
         yield return new WaitForSeconds(delay);
         popup.SetActive(false);
     }

     // display a sprite object for the specified period of time
    IEnumerator ShowPopup (SpriteRenderer popup, float delay) {
         popup.enabled = true;
         yield return new WaitForSeconds(delay);
         popup.enabled = false;
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
