using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroVideoController : MonoBehaviour {

	public AtomTouchGUI controller;
	public MovieTexture introVideo;
	


	void Start () {
		GetComponent<RawImage> ().texture = introVideo as MovieTexture;
		introVideo.Play ();
	}
	

	void Update () {
		if (Input.GetKeyDown (KeyCode.Space) && introVideo.isPlaying) {
			introVideo.Stop();

		}

		if (!introVideo.isPlaying) {
			controller.IntroVideoClose();
		}
	}

	public void ScreenTouched() 
	{
		introVideo.Stop();
		controller.IntroVideoClose();
	}
}
