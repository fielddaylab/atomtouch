﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class LevelInstructions {
	public Instructions[] instructions;
}

	[System.Serializable]
	public class Instructions {
		public string heading;
		[Multiline]
		public string instruction;
		[Multiline]
		public string[] instructions;
		public GameObject target;
		public GameObject[] showObjects;
		public Button[] buttonTriggers;
		public int buttonTriggerCount;
		public GameObject triggerButtonSet;
		public bool hasYesNo;
		public bool hasMultipleChoice;
		public string[] buttonAnswers;
		public int zone;
		public int TriggerID;
		public bool blockerOn;
		public bool cleared;

	}

public class LevelInstructionSet : MonoBehaviour {
	
	public LevelInstructions[] levelInstructions;
	
}
