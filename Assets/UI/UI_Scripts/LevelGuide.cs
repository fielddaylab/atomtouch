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
	private int levelNumber;
	private int instructionNumber;
	private int buttonTriggerCount;

	private AudioSource buttonSound;

	void Start () {
		buttonSound = GetComponent<AudioSource> ();
	}

	public void SetLevelGuide (int levelNumber, int instructionNumber)
	{
		this.levelNumber = levelNumber;
		this.instructionNumber = instructionNumber;
		// Set variables for current instruction
		heading.text = LIS.levelInstructions [levelNumber].instructions [instructionNumber].heading;
		instruction.text = LIS.levelInstructions [levelNumber].instructions [instructionNumber].instruction;
		if (LIS.levelInstructions [levelNumber].instructions [instructionNumber].gameObject != null)
			LIS.levelInstructions [levelNumber].instructions [instructionNumber].gameObject.SetActive (true);

		// If button trigger exists: 1) add listener 2) set next unactive;
		if (LIS.levelInstructions [levelNumber].instructions [instructionNumber].trigger != null) {
			// check if level is cleared so Next doesn't always disable on back button
			if (!LIS.levelInstructions [levelNumber].instructions [instructionNumber].cleared) nextButton.interactable = false;
			LIS.levelInstructions [levelNumber].instructions [instructionNumber].trigger.onClick.AddListener (delegate {
				ButtonTriggered();
			});
		}
		if (LIS.levelInstructions [levelNumber].instructions [instructionNumber].cleared) {
			nextButton.interactable = true;
		} else {
			nextButton.interactable = false;
		}

		// set trigger ID
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

		buttonTriggerCount = 0;

	}

	public void ButtonTriggered() {
		buttonTriggerCount++;
		if (buttonTriggerCount == LIS.levelInstructions [levelNumber].instructions [instructionNumber].buttonTriggerCount) {
			nextButton.interactable = true;
			LIS.levelInstructions [levelNumber].instructions [instructionNumber].trigger.onClick.RemoveListener (delegate {
				ButtonTriggered();
			});
			LIS.levelInstructions [levelNumber].instructions [instructionNumber].cleared = true;
			if (LIS.levelInstructions [levelNumber].instructions [instructionNumber].gameObject != null)
			LIS.levelInstructions [levelNumber].instructions [instructionNumber].gameObject.SetActive(false);
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
		if (instructionNumber == LIS.levelInstructions [levelNumber].instructions.Length - 1) {
			controller.hudController.activityCompletePanel.SetActive(true);
			controller.LevelGuideClose();
			//controller.LevelsOpen();
			levelsPanel.LevelComplete(levelNumber);
			return;
		}

		if (LIS.levelInstructions [levelNumber].instructions [instructionNumber].gameObject != null)
			LIS.levelInstructions [levelNumber].instructions [instructionNumber].gameObject.SetActive (false);
		SetLevelGuide (levelNumber, this.instructionNumber + 1);
	}

	public void BackButton() {
		buttonSound.Play ();
		if (LIS.levelInstructions [levelNumber].instructions [instructionNumber].gameObject != null)
			LIS.levelInstructions [levelNumber].instructions [instructionNumber].gameObject.SetActive (false);
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

	public void LevelGuideQuiteAnim () {
		if (LIS.levelInstructions [levelNumber].instructions [instructionNumber].gameObject != null)
			LIS.levelInstructions [levelNumber].instructions [instructionNumber].gameObject.SetActive (false);
	}
}		
