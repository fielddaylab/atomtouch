using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class LevelsPanel : MonoBehaviour {

	public GameObject levelPanel;

	public Sprite complete;

	private AudioSource buttonSound;

	private AtomTouchGUI controller;

	void Start () {
		buttonSound = GetComponent<AudioSource> ();
	}

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
		buttonSound.Play ();
	}

	public void LevelComplete(int level) {
		levelTiles [level].startButton.GetComponent<Image> ().sprite = complete;
		//levelTiles [level].startButton.GetComponentInChildren<Text> ().text = "Restart";
	}

}
