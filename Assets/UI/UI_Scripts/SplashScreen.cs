using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SplashScreen : MonoBehaviour {
	public AtomTouchGUI controller;

	public Sprite[] frames;
	public int framesPerSecond;
	public Image image;
	
	private float index;

	bool looped;

	void Start () {
		looped = false;
	}
	void Update () {
		index = (Time.time * framesPerSecond);
		index = index % frames.Length;
		image.sprite = frames [(int)index];
		if ((int)index == 4) {			 
			if (looped) {
				this.gameObject.SetActive(false);
			}
			else {
			#if UNITY_IOS
			Debug.Log("Iphone detected, starting intro video using handheld.");	
			Handheld.PlayFullScreenMovie ("intro.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);	
			#endif
			looped = true;
			}
		}


	}


	/*
	public Image currentImage;
	public Sprite[] splashImages;
	public float timeBewteen;

	private float initialTime;

	bool run;

	// Use this for initialization
	public void Init () {
		initialTime = Time.realtimeSinceStartup;
		run = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (run)
		RunSplashScreen ();
	}

	public void RunSplashScreen () {
		int i = 0;
		if (Time.realtimeSinceStartup - initialTime % timeBewteen == 0) {
			currentImage.sprite = splashImages[i];
			i++;
		}
		if (splashImages.Length == i) {
			run = false;
			this.gameObject.SetActive (false);
		}
	}*/
}
