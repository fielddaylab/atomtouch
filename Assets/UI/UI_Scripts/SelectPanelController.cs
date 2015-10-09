using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SelectPanelController : MonoBehaviour {

	public AtomTouchGUI controller;

	public GameObject selectPanel;

	public Button selectedAtoms;
	public Button delete;
	public Button selectAll;
	public Button deselect;

	public Text selectedAtomsText;

//	private Text deleteText;
//	private Text selectAllText;
//	private Text deselectText;

	private int numberSelectedAtoms;

	void Start () {
//		deleteText = delete.GetComponentInChildren<Text> ();
//		selectAllText = selectAll.GetComponentInChildren<Text> ();
//		deselectText = deselect.GetComponentInChildren<Text> ();
	}

	// Update is called once per frame
	void Update () {
		numberSelectedAtoms = controller.CountSelectedAtoms ();
		selectedAtomsText.text = numberSelectedAtoms + " Atom(s) Selected";
		/*if (Potential.currentPotential == Potential.potentialType.LennardJones) {
			if (selectedAtomsLenard.gameObject.activeSelf == false) {
				selectedAtomsLenard.gameObject.SetActive(true);
				selectedAtomsBuck.gameObject.SetActive (false);
			}
			selectedAtomsTextLenard.text = numberSelectedAtoms + " Atom(s) Selected \n " +
				"Cu: " + controller.copperCount.nam + 
				"\nAu: " + controller.goldCount +
				"\nPl: " + controller.platinumCount;
		} else {
			if (selectedAtomsBuck.gameObject.activeSelf == false) {
				selectedAtomsBuck.gameObject.SetActive(true);
				selectedAtomsLenard.gameObject.SetActive (false);
			}
			selectedAtomsTextBuck.text = numberSelectedAtoms + " Atom(s) Selected";
		}*/
	}
}
