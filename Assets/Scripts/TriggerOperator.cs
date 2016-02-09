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

	public void SelectingAtoms() {
		if (triggerID == 4) {
			LG.EventTriggered ();
		}
	}

	public void DeletingAtoms() {
		if (triggerID == 5) {
			LG.EventTriggered ();
		}
	}

	public void TempSliderZero() {
		if (triggerID == 6) {
			LG.EventTriggered ();
		}
	}

	public void TempSlider1000() {
		if (triggerID == 7) {
			LG.EventTriggered ();
		}
	}
	public void TempSlider5000() {
		if (triggerID == 8) {
			LG.EventTriggered ();
		}
	}
}
