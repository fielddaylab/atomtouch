using UnityEngine;
using System.Collections;

public class LevelStates : MonoBehaviour
{

	public AtomTouchGUI controller;
	public LevelGuide levelGuide;

	public void Level01 ()
	{
		controller.LevelsClose ();
		controller.CameraScriptOn (true);
		levelGuide.SetLevelGuide (0, 0);
		controller.LevelGuideOpen ();

		Debug.Log ("Level one started");
	}

	public void Level02 ()
	{
		controller.LevelsClose ();
		controller.CameraScriptOn (true);
		levelGuide.SetLevelGuide (1, 0);
		controller.LevelGuideOpen ();
		Debug.Log ("Level two started");
	}

	public void Level03 ()
	{
		controller.LevelsClose ();
		controller.CameraScriptOn (true);
		Debug.Log ("Level three started");
	}

	public void Level04 ()
	{
		controller.LevelsClose ();
		controller.CameraScriptOn (true);
		Debug.Log ("Level four started");
	}

	public void Level05 ()
	{
		controller.LevelsClose ();
		controller.CameraScriptOn (true);
		Debug.Log ("Level five started");
	}
}
