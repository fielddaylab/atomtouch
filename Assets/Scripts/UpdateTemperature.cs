﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpdateTemperature : MonoBehaviour {

	public AtomTouchGUI controller;

	Text text;
	// Use this for initialization
	void Start () {
		text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		//if(oldTemperature < 0)
		if(StaticVariables.desiredTemperature < 0.001f){
			StaticVariables.desiredTemperature = 0.001f;
			controller.levelGuideGameObject.GetComponent<TriggerOperator> ().TempSliderZero ();
		}
		if (StaticVariables.desiredTemperature >= 1000f) {
			controller.levelGuideGameObject.GetComponent<TriggerOperator> ().TempSlider1000 ();
		}
		if (StaticVariables.desiredTemperature >= 5000f) {
			controller.levelGuideGameObject.GetComponent<TriggerOperator> ().TempSlider5000 ();
		}
		text.text = "Temp" + System.Environment.NewLine 
		+  (StaticVariables.desiredTemperature / StaticVariables.tempScaler) + "K"
		+ System.Environment.NewLine + "(" + KToC(StaticVariables.desiredTemperature).ToString("0.00") +"°C)";
	}

	public static float KToC(float k){
		return k-273.15f;
	}
}
