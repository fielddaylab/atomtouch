using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour {
	public AtomTouchGUI controller;

	public GameObject settingsPanel;
	public GameObject buckinghamPanel;
	public GameObject lenardJonesPanel;
	public GameObject activityCompletePanel;
	public Button settingsButton;
	public GameObject selectPanel;
	public GameObject blockPanel;
	private int numberSelectedAtoms;

	ActivityComplete activityComplete;

	void Start () {
		activityComplete = activityCompletePanel.GetComponent<ActivityComplete>() as ActivityComplete;
	}

	void Update () {
		numberSelectedAtoms = controller.CountSelectedAtoms ();
		
		if (numberSelectedAtoms > 0) {
			selectPanel.SetActive (true);
		} else {
			selectPanel.SetActive (false);
		}
	}

	public void SettingsOpen() {
		settingsPanel.SetActive (true);
	}

	public void SettingsClose() {
		settingsPanel.SetActive (false);
	}
	
	public void ChangeHUDtoBuckingham (){
		buckinghamPanel.SetActive (true);
		lenardJonesPanel.SetActive (false);
	}
	
	public void ChangeHUDtoLenardJones (){
		buckinghamPanel.SetActive (false);
		lenardJonesPanel.SetActive (true);
	}

	public void LevelCompleted(string levelName) {
		activityComplete.text.text = "Nice work! You've completed " + levelName + "!";
	}

}
