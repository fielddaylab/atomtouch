using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class LevelGuide : MonoBehaviour
{
	public TriggerOperator triggerOperator;
	public GameObject rigPanel;
	public GameObject contentPanel;
	public LevelGuideScroll levelGuideScroll;
	public Button backButton;
	public Button nextButton;
	public Text nextButtonText;
	public Text heading;
	public Text instruction;
	public AtomTouchGUI controller;
	public LevelsPanel levelsPanel;
	public LevelInstructionSet LIS;
	public GameObject yesNoButtonSet;
	public GameObject multipleChoiceButtonSet;
	public GameObject[] hideObjects;

	private int levelNumber;
	private int instructionNumber;
	private GameObject target;
	private int buttonTriggerCount;
	private Button[] buttonTrggers;
	private GameObject yesNo;
	private GameObject multipleChoice;
	private int answer;
	private string[] buttonAnswers;

	private AudioSource buttonSound;

	void Start () {
		buttonSound = GetComponent<AudioSource> ();
	}

	public void SetLevelGuide (int levelNumber, int instructionNumber)
	{
		// Set variables for current instruction
		this.levelNumber = levelNumber;
		this.instructionNumber = instructionNumber;
		this.target = LIS.levelInstructions [levelNumber].instructions [instructionNumber].target;
		this.buttonTrggers = LIS.levelInstructions [levelNumber].instructions [instructionNumber].buttonTriggers;
		heading.text = LIS.levelInstructions [levelNumber].instructions [instructionNumber].heading;


		// Set instructions based on answer
		if (instructionNumber != 0) {
			if (LIS.levelInstructions [levelNumber].instructions [instructionNumber - 1].hasYesNo) {
				instruction.text = LIS.levelInstructions [levelNumber].instructions [instructionNumber].instructions [answer];
			} else if (LIS.levelInstructions [levelNumber].instructions [instructionNumber - 1].hasMultipleChoice) {
				instruction.text = LIS.levelInstructions [levelNumber].instructions [instructionNumber].instructions [answer];
			} else {
				instruction.text = LIS.levelInstructions [levelNumber].instructions [instructionNumber].instruction;
			}
		} else {
			instruction.text = LIS.levelInstructions [levelNumber].instructions [instructionNumber].instruction;
		}
		this.buttonAnswers = LIS.levelInstructions [levelNumber].instructions [instructionNumber].buttonAnswers;

		// Target
		if (target != null)
			target.SetActive (true);

		// ButtonTriggers
		if (buttonTrggers != null) {
			if (!LIS.levelInstructions [levelNumber].instructions [instructionNumber].cleared) 
				nextButton.interactable = false;
				foreach (Button button in buttonTrggers) {
					button.onClick.AddListener(delegate {
					ButtonTriggered();
				});
			}
		}

		// Question Buttons Activate
		if (LIS.levelInstructions [levelNumber].instructions [instructionNumber].hasYesNo) {
			yesNoButtonSet.SetActive (true);
		}

		if (LIS.levelInstructions [levelNumber].instructions [instructionNumber].hasMultipleChoice) {
			LIS.levelInstructions [levelNumber].instructions [instructionNumber].cleared = false;
			multipleChoiceButtonSet.SetActive (true);
			Button[] buttons = multipleChoiceButtonSet.GetComponentsInChildren<Button> ();
			for (int i = 0; i < buttonAnswers.Length; i++) {
				buttons [i].GetComponentInChildren<Text> ().text = buttonAnswers [i];
			}
		}
		// Zones
		if (LIS.levelInstructions [levelNumber].instructions [instructionNumber].zone > 0) {
			ChangeZones (LIS.levelInstructions [levelNumber].instructions [instructionNumber].zone);
		} else {
			ChangeZones (100);
		}

		// NextButton
		if (LIS.levelInstructions [levelNumber].instructions [instructionNumber].cleared) {
			nextButton.interactable = true;
		} else {
			nextButton.interactable = false;
		}

		// Set Trigger ID
		triggerOperator.triggerID = LIS.levelInstructions [levelNumber].instructions [instructionNumber].TriggerID;

		if (instructionNumber == 0) {
			backButton.interactable = false;
		} else {
			backButton.interactable = true;
		}
		if (instructionNumber == LIS.levelInstructions [levelNumber].instructions.Length - 1) 
			nextButtonText.text = "Finish";
		if (instructionNumber < LIS.levelInstructions [levelNumber].instructions.Length - 1) 
			nextButtonText.text = "Next";

		// Blocker
		if (LIS.levelInstructions [levelNumber].instructions [instructionNumber].blockerOn) {
			foreach (GameObject ob in hideObjects) {
				ob.SetActive (false);
			}
			StaticVariables.canSelectAtoms = false;
		} else {
			foreach (GameObject ob in hideObjects) {
				ob.SetActive (true);
			}
			StaticVariables.canSelectAtoms = true;
		}

		// ShowObjects
		if (LIS.levelInstructions [levelNumber].instructions [instructionNumber].showObjects.Length > 0) {
			foreach (GameObject ob in LIS.levelInstructions [levelNumber].instructions [instructionNumber].showObjects) {
				ob.SetActive (true);
			}
			StaticVariables.canSelectAtoms = false;
		}

		buttonTriggerCount = 0;

	}

	public void ButtonTriggered() {
		buttonTriggerCount++;
		if (buttonTriggerCount == LIS.levelInstructions [levelNumber].instructions [instructionNumber].buttonTriggerCount) {
			nextButton.interactable = true;
			foreach (Button button in buttonTrggers) {
				button.onClick.RemoveListener (delegate {
					ButtonTriggered ();
				});
			}
			LIS.levelInstructions [levelNumber].instructions [instructionNumber].cleared = true;
			if (target != null)
				target.SetActive (false);
			buttonTriggerCount = 0;
		}
	}

	public void EventTriggered () {
		nextButton.interactable = true;
		LIS.levelInstructions [levelNumber].instructions [instructionNumber].cleared = true;
	}

	public void NextButton ()
	{
		buttonSound.Play ();
		// reset answer buttons - MUST STAY ABOVE LIS CHANGE!
		if (LIS.levelInstructions [levelNumber].instructions [instructionNumber].hasYesNo) {
			yesNoButtonSet.SetActive (false);
		}

		if (LIS.levelInstructions [levelNumber].instructions [instructionNumber].hasMultipleChoice) {
			multipleChoiceButtonSet.SetActive (false);
		}
		if (instructionNumber == LIS.levelInstructions [levelNumber].instructions.Length - 1) {
			controller.hudController.activityCompletePanel.SetActive(true);
			controller.LevelGuideClose();
			//controller.LevelsOpen();
			levelsPanel.LevelComplete(levelNumber);
			return;
		}

		if (target != null)
			target.SetActive (false);
		SetLevelGuide (levelNumber, this.instructionNumber + 1);
	}

	public void BackButton() {
		buttonSound.Play ();
		// reset answer buttons - MUST STAY ABOVE LIS CHANGE!
		if (LIS.levelInstructions [levelNumber].instructions [instructionNumber].hasYesNo) {
			yesNoButtonSet.SetActive (false);
		}

		if (LIS.levelInstructions [levelNumber].instructions [instructionNumber].hasMultipleChoice) {
			multipleChoiceButtonSet.SetActive (false);
		}
		if (target != null)
			target.SetActive (false);
		SetLevelGuide (levelNumber, this.instructionNumber - 1);
	}

	public void OnMouseDown ()
	{
		controller.cameraScript.enabled = false;
	}

	public void OnMouseUp ()
	{
		controller.cameraScript.enabled = true;
	}

	public void LevelGuideQuite () {
		// reset answer buttons - MUST STAY ABOVE LIS CHANGE!
		if (LIS.levelInstructions [levelNumber].instructions [instructionNumber].hasYesNo) {
			yesNoButtonSet.SetActive (false);
		}

		if (LIS.levelInstructions [levelNumber].instructions [instructionNumber].hasMultipleChoice) {
			multipleChoiceButtonSet.SetActive (false);
		}
		if (target != null)
			target.SetActive (false);
	}
	// No = 0, Yes = 1
	public void YesNo (int answer) {
		this.answer = answer; 
		EventTriggered ();
	}

	// A = 0, B = 1, C = 2, D = 3
	public void MultipleChoice (int answer) {
		this.answer = answer;
		EventTriggered ();
	}

	public void ChangeZones(int zone) {
		for (int i = 0; i < Atom.AllAtoms.Count; i++) {
			Atom currAtom = Atom.AllAtoms [i];

			if (currAtom.selectionZone == zone) {
				currAtom.SetSelected (true);
				Debug.Log (i);
			} else {
				currAtom.SetSelected (false);
			}	
		}
	}
}		
