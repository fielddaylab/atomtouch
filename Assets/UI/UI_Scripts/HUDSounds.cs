using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class HUDSounds : MonoBehaviour {


	public AudioSource HUDButtons;
	public AudioSource mainMenuButtons;
	public AudioSource menu;

	public void PlayButtonAudio () {
		HUDButtons.Play ();
	}

	public void PlayMainMenu () {
		mainMenuButtons.Play ();
	}

}
