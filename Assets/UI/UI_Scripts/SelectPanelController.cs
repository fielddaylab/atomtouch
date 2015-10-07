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

	private Text selectedAtomsText;
//	private Text deleteText;
//	private Text selectAllText;
//	private Text deselectText;

	private int numberSelectedAtoms;

	void Start () {
		selectedAtomsText = selectedAtoms.GetComponentInChildren<Text> ();
//		deleteText = delete.GetComponentInChildren<Text> ();
//		selectAllText = selectAll.GetComponentInChildren<Text> ();
//		deselectText = deselect.GetComponentInChildren<Text> ();
	}

	// Update is called once per frame
	void Update () {
		numberSelectedAtoms = controller.CountSelectedAtoms ();
		selectedAtomsText.text = numberSelectedAtoms + " Atom(s) Selected";

		if (numberSelectedAtoms > 0) {
			delete.interactable = true;
			selectPanel.SetActive (true);
		} else {
			delete.interactable = false;
			selectPanel.SetActive (false);
		}
	}
}
