using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuPanel : MonoBehaviour
{

	public GameObject Panel;
	public Button levelsButton;
	public Button creditsButton;
	public Button freePlayButton;
	public Button ReplayVideoButton;
	
	public void Init (AtomTouchGUI controler)
	{
		levelsButton.onClick.AddListener (controler.LevelsOpen);
		creditsButton.onClick.AddListener (controler.CreditsOpen);
		freePlayButton.onClick.AddListener (controler.FreePlayOpen);
		ReplayVideoButton.onClick.AddListener (controler.IntroVideoOpen);
	}
}
