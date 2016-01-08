using UnityEngine;
using System.Collections;

public class TriggerOperator : MonoBehaviour {

	public LevelGuide LG;
	public int triggerID;

	public void ScrollingAtom() {
		if (triggerID == 2) {
			LG.EventTriggered();
		}
	}

	public void MovingAtom() {
		if (triggerID == 1) {
			LG.EventTriggered();
		}
	}

	public void ChangeingPerspective() {
		if (triggerID == 3) {
			LG.EventTriggered();
		}
	}
}
