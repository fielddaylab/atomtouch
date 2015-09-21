using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroVideoController : MonoBehaviour {

	public AtomTouchGUI controller;
	public MovieTexture introVideo;
	

	// Use this for initialization
	void Start () {
		GetComponent<RawImage> ().texture = introVideo as MovieTexture;
		introVideo.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space) && introVideo.isPlaying) {
			introVideo.Stop();
			controller.IntroVideoClose();
		}
	}
}
