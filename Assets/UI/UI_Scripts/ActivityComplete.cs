using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActivityComplete : MonoBehaviour {

	public AtomTouchGUI controller;
	public Text text;
	public Button next;

	void Start () {
		next.onClick.AddListener (onNext);
	}

	public void onNext () {
		controller.LevelsOpen ();
		controller.hudController.activityCompletePanel.SetActive (false);
		controller.hudController.blockPanel.SetActive (false);
		StaticVariables.canSelectAtoms = true;
	}

}
