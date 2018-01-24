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
	public Button AudioButton;
	public Button MutedButton;
	
        private AtomTouchGUI my_controller;

        //hack indirection because responsibility for UI and effects are separated,
        //which is unnecessary/cumbersome when they need to be interlinked
        private void my_ToggleAudio ()
        {
            my_controller.ToggleAudio();
            if(AudioListener.volume > 0)
            {
              AudioButton.gameObject.SetActive(true);
              MutedButton.gameObject.SetActive(false);
            }
            else
            {
              AudioButton.gameObject.SetActive(false);
              MutedButton.gameObject.SetActive(true);
            }
        }

	public void Init (AtomTouchGUI controller)
	{
                my_controller = controller;

		levelsButton.onClick.AddListener (controller.LevelsOpen);
		creditsButton.onClick.AddListener (controller.CreditsOpen);
		freePlayButton.onClick.AddListener (controller.FreePlayOpen);
		ReplayVideoButton.onClick.AddListener (controller.IntroVideoOpen);
		AudioButton.onClick.AddListener (my_ToggleAudio);
		MutedButton.onClick.AddListener (my_ToggleAudio);
	}
}
