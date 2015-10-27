using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PlayAudioActive : MonoBehaviour {

	public AudioSource mainMenuAudio;

	void Update() {
		if (gameObject.activeSelf && !mainMenuAudio.isPlaying) {
			mainMenuAudio.Play ();
		} 
	}

}
