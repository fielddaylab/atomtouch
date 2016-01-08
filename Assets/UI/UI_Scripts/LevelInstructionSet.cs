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
		public GameObject gameObject;
		public Button trigger;
		public int buttonTriggerCount;
		public GameObject buttonSet;
		public bool cleared;
		public int TriggerID;
	}

	
}
