using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class SoundManager : MonoBehaviour {

	/* List of functions (all are for random clip selection w/o repetition):
	 * 
	 * FoodImpact() 
	 * P1Throw() 
	 * P2Throw()
	 * FoodRot()
	 * Slip()
	 * FoodBounce()
	 */


	public static SoundManager instance = null;

	AudioSource[] source;


	//Quick access to all AudioClips here
	//_______________________________________________________
	public AudioClip[] foodImpact; //place FoodImpact sfx here
	public AudioClip eggCrabImpact;
	public AudioClip smallGlassImpact;
	public AudioClip largeGlassImpact;

	public AudioClip[] p1Throw; //P1FoodThrow sfx
	public AudioClip[] p2Throw; //P2FoodThrow sfx

	public AudioClip[] foodRot; //FoodRot sfx

	public AudioClip p1DeliverFood; //DeliverFoodP1
	public AudioClip p2DeliverFood; //DeliverFoodP2

	public AudioClip[] slip; //slip sfx

	public AudioClip pickup; //foodPickup

	public AudioClip[] foodBounce; //FoodBounce sfx


	void Awake () {
		//check if insance of SoundManager exists, if so, destroy this instance
		if (instance == null) {			
			instance = this;			
		} else if (instance != this) {	
			Destroy (gameObject);
		}
	}
		

	//Call to select random foodImpact sfx
	public AudioClip FoodImpact(){
		AudioClip clip;
		int lastClip = -1;
		int randClip = Random.Range (0, foodImpact.Length - 1); 
		if (randClip == lastClip) {
			while (randClip == lastClip) {
				randClip = Random.Range (0, foodImpact.Length - 1);
			}
		}
		clip = foodImpact[randClip];
		lastClip = randClip;
		return clip;
	}



	//Call to select random p1Throw sfx
	public AudioClip P1Throw(){
		AudioClip clip;
		int lastClip = -1;
		int randClip = Random.Range (0, p1Throw.Length - 1); 
		if (randClip == lastClip) {
			while (randClip == lastClip) {
				randClip = Random.Range (0, p1Throw.Length - 1);
			}
		}
		clip = p1Throw[randClip];
		lastClip = randClip;
		return clip;
	}


	//Call to select random p2Throw sfx
	public AudioClip P2Throw(){
		AudioClip clip;
		int lastClip = -1;
		int randClip = Random.Range (0, p2Throw.Length - 1); 
		if (randClip == lastClip) {
			while (randClip == lastClip) {
				randClip = Random.Range (0, p2Throw.Length - 1);
			}
		}
		clip = p2Throw[randClip];
		lastClip = randClip;
		return clip;
	}


	//Call to select random foodRot sfx
	public AudioClip FoodRot(){
		AudioClip clip;
		int lastClip = -1;
		int randClip = Random.Range (0, foodRot.Length - 1); 
		if (randClip == lastClip) {
			while (randClip == lastClip) {
				randClip = Random.Range (0, foodRot.Length - 1);
			}
		}
		clip = foodRot[randClip];
		lastClip = randClip;
		return clip;
	}

	//Call to select random slip sfx
	public AudioClip Slip(){
		AudioClip clip;
		int lastClip = -1;
		int randClip = Random.Range (0, slip.Length - 1); 
		if (randClip == lastClip) {
			while (randClip == lastClip) {
				randClip = Random.Range (0, slip.Length - 1);
			}
		}
		clip = slip[randClip];
		lastClip = randClip;
		return clip;
	}

	//Call to select random foodBounce sfx
	public AudioClip FoodBounce(){
		AudioClip clip;
		int lastClip = -1;
		int randClip = Random.Range (0, foodBounce.Length - 1); 
		if (randClip == lastClip) {
			while (randClip == lastClip) {
				randClip = Random.Range (0, foodBounce.Length - 1);
			}
		}
		clip = foodBounce[randClip];
		lastClip = randClip;
		return clip;
	}








}
