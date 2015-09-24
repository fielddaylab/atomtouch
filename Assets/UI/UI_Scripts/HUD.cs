using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour {

	public GameObject settingsPanel;
	public GameObject buckinghamPanel;
	public GameObject lenardJonesPanel;


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

}
