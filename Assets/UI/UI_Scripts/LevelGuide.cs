﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelGuide : MonoBehaviour
{

	public GameObject rigPanel;
	public GameObject contentPanel;
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

	public void SetLevelGuide (int levelNumber, int instructionNumber)
	{
		this.levelNumber = levelNumber;
		this.instructionNumber = instructionNumber;
		heading.text = LIS.levelInstructions [levelNumber].instructions [instructionNumber].heading;
		instruction.text = LIS.levelInstructions [levelNumber].instructions [instructionNumber].instruction;
		Debug.Log (LIS.levelInstructions [levelNumber].instructions [instructionNumber].gameObject != null);
		if (LIS.levelInstructions [levelNumber].instructions [instructionNumber].gameObject != null)
			LIS.levelInstructions [levelNumber].instructions [instructionNumber].gameObject.SetActive (true);
		if (instructionNumber == 0)
			backButton.enabled = false;
		else
			backButton.enabled = true;
		if (instructionNumber == LIS.levelInstructions [levelNumber].instructions.Length - 1) 
			nextButtonText.text = "Finish";
		if (instructionNumber < LIS.levelInstructions [levelNumber].instructions.Length - 1) 
			nextButtonText.text = "Next";
//		Debug.Log (" level number = " + levelNumber + " /n instruction number = " + instructionNumber);
	}

	public void NextButton ()
	{
		if (instructionNumber == LIS.levelInstructions [levelNumber].instructions.Length - 1) {
			controller.LevelGuideClose ();
			controller.LevelsOpen ();
			levelsPanel.LevelComplete(levelNumber);
			nextButtonText.text = "Next";
			return;
		}

		if (LIS.levelInstructions [levelNumber].instructions [instructionNumber].gameObject != null)
			LIS.levelInstructions [levelNumber].instructions [instructionNumber].gameObject.SetActive (false);
		SetLevelGuide (levelNumber, this.instructionNumber + 1);
	}

	public void BackButton() {
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
}		
