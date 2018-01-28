using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour {

	public static MusicManager instance = null;
	AudioSource[] source; //MusicManager's AudioSource, all music plays from here
						  //source[0, 1]  -> Title
						  //source [2, 3, 4, 5, 6] -> Main MX
						  //source[7,0] -> Victory
	public float fadeOutTime = 1.5f; //time takes to fade out when calling FadeOutMusic()
	public float fadeInTime = 0.2f;  //time takes to fade in when calling FadeInMusic()
	public AudioMixerSnapshot fadeOut; //place FadeOut snapshot here
	public AudioMixerSnapshot startSnap;  //place default snapshot here
	//for Title MX
	public AudioClip titleIntro;
	public AudioClip titleLoop;

	//for Victory MX
	public AudioClip victoryIntro;
	public AudioClip victoryLoop;
	public AudioMixerSnapshot mainMXOut;
	public float mainMXOutTime = 1f;

	//for Main MX
	public AudioClip layer1;
	public AudioClip layer2;
	public AudioClip layer3;
	public AudioClip layer4;
	public AudioClip layer5;

	public float layerTransTime = 2.5f;
	public AudioMixerSnapshot layer1Snap;
	public AudioMixerSnapshot layer2Snap;
	public AudioMixerSnapshot layer3Snap;
	public AudioMixerSnapshot layer4Snap;
	public AudioMixerSnapshot layer5Snap;








	// Use this for initialization
	void Awake () {
		//check if insance of MusicManager exists, if so, destroy this instance
		if (instance == null) {			
			instance = this;			
		} else if (instance != this) {	
			Destroy (gameObject);
		}

		source = GetComponents<AudioSource> ();

		Debug.Log (source.Length);
	}




	//Title music control
	//___________________________________________________________

	public void TitleMX(){
		//if music already playing, fade out music and stop all music playing
			//load title music, set second section to loop, and play
		StopMusic();
		LoadTitleMX ();
		FadeInMusic ();
		source [1].Play ();
		source [0].loop = true;
		Invoke ("PlayTitleMX", source [1].clip.length);
	}




		
	//load titlemx in AudioSource clips
	void LoadTitleMX(){
		source [1].clip = titleIntro;
		source [0].clip = titleLoop;
	}


	void PlayTitleMX(){
		source [0].Play ();
	}


	//Victory music control
	//____________________________________

	//FIX
	public void VictoryMX(){
		//if music already playing, fade out music and stop all music playing
			//load Victory music, set second section to loop, and play
		if (source [2].isPlaying) {
			LoadVictoryMX ();
			mainMXOut.TransitionTo (mainMXOutTime);
			source [7].Play ();
			source [0].loop = true;
			Invoke ("PlayVictoryMX", source [7].clip.length);
		} else if (!source [2].isPlaying) {
			LoadVictoryMX ();
			source [7].Play ();
			source [0].loop = true;
			Invoke ("PlayVictoryMX", source [7].clip.length);
		}
	}


	//load Victory mx in AudioSource clips
	void LoadVictoryMX(){
		source [7].clip = victoryIntro;
		source [0].clip = victoryLoop;
	}

	void PlayVictoryMX(){
		source [0].Play ();
	}




	//Main gameplay music control
	//____________________________________

	public void MainMX(){
		//if music already playing, fade out music and stop all music playing
		//load Victory music, set second section to loop, and play
		if (source [0].isPlaying || source [1].isPlaying) {
			FadeOutMusic ();
			Invoke ("LoadMainMX", fadeOutTime);
			layer1Snap.TransitionTo (0.2f);
			Invoke ("PlayMainMX",  fadeOutTime);
		} else if (!source [0].isPlaying && !source [1].isPlaying) {
			layer1Snap.TransitionTo (0.2f);
			LoadMainMX ();
			PlayMainMX ();
		}
	}

	//places layers in corresponding AudioSource clips, sets all to loop
	void LoadMainMX(){
		source [2].clip = layer1;
		source [3].clip = layer2;
		source [4].clip = layer3;
		source [5].clip = layer4;
		source [6].clip = layer5;

		source [2].loop = true;
		source [3].loop = true;
		source [4].loop = true;
		source [5].loop = true;
		source [6].loop = true;
	}

	//plays all AudioSources at once -> called from MainMX()^
	void PlayMainMX(){
		source [2].Play ();
		source [3].Play ();
		source [4].Play ();
		source [5].Play ();
		source [6].Play ();
	}



	//transition to layer2
	public void MainLayer2(){
		layer2Snap.TransitionTo (layerTransTime);
	}
	//transition to layer3
	public void MainLayer3(){
		layer3Snap.TransitionTo (layerTransTime);
	}
	//transition to layer4
	public void MainLayer4(){
		layer4Snap.TransitionTo (layerTransTime);
	}
	//transition to layer5
	public void MainLayer5(){
		layer5Snap.TransitionTo (layerTransTime);
	}


	//Global music control
	//_____________________________________


	//fades out any music currently playing, stops AudioSources after fadeout
	public void FadeOutMusic(){
		fadeOut.TransitionTo (fadeOutTime);
		Invoke ("StopMusic", fadeOutTime);
	}


	public void StopMusic(){
		source [0].Stop ();
		source [1].Stop ();
		source [2].Stop ();
		source [3].Stop ();
		source [4].Stop ();
		source [5].Stop ();
		source [6].Stop ();
		source [7].Stop ();

	}



	public void FadeInMusic(){
		startSnap.TransitionTo (fadeInTime);
	}
}
