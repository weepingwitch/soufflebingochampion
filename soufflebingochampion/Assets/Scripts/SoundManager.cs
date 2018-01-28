using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class SoundManager : MonoBehaviour {

	public static SoundManager instance = null;

	AudioSource[] source;


	//Quick access to all AudioClips here
	//_______________________________________________________
	public AudioClip[] foodImpact; //place FoodImpact sfx here

	public AudioClip[] p1Throw; //P1FoodThrow sfx
	public AudioClip[] p2Throw; //P2FoodThrow sfx

	public AudioClip[] foodRot; //FoodRot sfx

	public AudioClip p1DeliverFood; //DeliverFoodP1
	public AudioClip p2DeliverFood; //DeliverFoodP2

	public AudioClip pickup; //FoodPickup






	//Example function to put on other game objects for randomly selecting
		//from an array of sounds without repeating the same one twice in a row
	public void RandomExample(){
		int lastClip = -1;
		int randClip = Random.Range (0, foodImpact.Length - 1); //used foodImpact as example
		if (randClip == lastClip) {
			while (randClip == lastClip) {
				 randClip = Random.Range (0, foodImpact.Length - 1);
			}
		}

		//add clip from above to AudioSource and Play(); 
	}
}
