using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelsPanel : MonoBehaviour {

	public GameObject levelPanel;

	public Sprite complete;

	private AtomTouchGUI controller;

	[System.Serializable]
	public class LevelTile
	{
		public GameObject tile;
		public Image levelIcon;
		public Text description;
		public Button startButton;
		Text startButtonText;
	}

	public LevelTile[] levelTiles;

	public void Init(AtomTouchGUI controller) 
	{
		this.controller = controller;
	}

	public void StartButtonPressed ()
	{
		controller.LevelsClose ();
		controller.LevelGuideOpen ();
	}

	public void LevelComplete(int level) {
		levelTiles [level].startButton.GetComponent<Image> ().sprite = complete;
		levelTiles [level].startButton.GetComponentInChildren<Text> ().text = "Restart";
	}

}
