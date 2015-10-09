using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CountdownTimer : MonoBehaviour {

	public Text timerText;
	public GameObject Timer;

	float initialTime;
	float offsetTime;
	float stopTime;
	float countdownTime;

	bool timerRunning;
	
	void Start () {
		timerRunning = false;
	}
	
	void Update () {
		Debug.Log (Timer.activeSelf);
		if (Timer.activeSelf) {
			if (!timerRunning) {
				StartTimer (30);
				timerRunning = true;
			} 
		} else {
			timerRunning = false;
		}

		if (countdownTime >= 0 && timerRunning) {
			timerText.text = countdownTime.ToString () + " Sec";
			offsetTime = (UnityEngine.Time.realtimeSinceStartup - initialTime);
			countdownTime = (int)Mathf.Ceil(stopTime - offsetTime);
		} else {
			Timer.SetActive (false);
		}

	}

	public void StartTimer (float sec){
		initialTime = UnityEngine.Time.realtimeSinceStartup;
		offsetTime = (UnityEngine.Time.realtimeSinceStartup - initialTime);
		stopTime = sec;
		countdownTime = (int)Mathf.Ceil(stopTime - offsetTime); 
	}
}
