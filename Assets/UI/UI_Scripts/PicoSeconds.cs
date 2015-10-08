using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PicoSeconds : MonoBehaviour {
		
	public Text time;
	int tenth;

	void Update () {
		tenth = (int)Mathf.Floor (((StaticVariables.currentTime - (Mathf.Floor (StaticVariables.currentTime))) * 10));
		if (tenth == 10) {
			tenth = 0;
		}
		time.text = ((Mathf.Floor(StaticVariables.currentTime))).ToString() + "." + tenth.ToString() + " ps"; // rounding to 1 tenth for asthentics
	}
}
