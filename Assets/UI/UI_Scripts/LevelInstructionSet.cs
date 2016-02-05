using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelInstructionSet : MonoBehaviour {
	
	public LevelInstructions[] levelInstructions;

	[System.Serializable]
	public class LevelInstructions {	

		public Instructions[] instructions;
	}

	[System.Serializable]
	public class Instructions {
		public string heading;
		[Multiline]
		public string instruction;
		public string[] instructions;
		public GameObject target;
		public Button[] buttonTriggers;
		public int buttonTriggerCount;
		public GameObject triggerButtonSet;
		public bool hasYesNo;
		public bool hasMultipleChoice;
		public string[] buttonAnswers;
		public bool hasZones;
		public int zone;
		public int TriggerID;
		public bool cleared;

	}

	
}
