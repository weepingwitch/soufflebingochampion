using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {

	//temporary script for testing fuctions from MusicManager
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKeyDown ("q")) {
			MusicManager.instance.TitleMX ();
		}

		if (Input.GetKeyDown ("e")) {
			MusicManager.instance.VictoryMX (0);
		}

		if (Input.GetKeyDown ("d")) {
			MusicManager.instance.VictoryMX (1);
		}

		if (Input.GetKeyDown ("w")) {
			MusicManager.instance.MainMX ();
		}




		if (Input.GetKeyDown ("z")) {
			MusicManager.instance.MainLayer2 ();
		}

		if (Input.GetKeyDown ("x")) {
			MusicManager.instance.MainLayer3 ();
		}

		if (Input.GetKeyDown ("c")) {
			MusicManager.instance.MainLayer4 ();
		}

		if (Input.GetKeyDown ("v")) {
			MusicManager.instance.MainLayer5 ();
		}
	}
}
