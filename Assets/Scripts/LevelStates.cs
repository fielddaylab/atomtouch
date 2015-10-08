using UnityEngine;
using System.Collections;

public class LevelStates : MonoBehaviour
{

	public AtomTouchGUI controller;
	public LevelGuide levelGuide;
	public CreateEnvironment createEnviroment;

	public void Level01 ()
	{ 
		controller.currentAtomPreset = "box";
		createEnviroment.CreatePresetConfiguration (controller.currentAtomPreset);
		controller.LevelsClose ();
		controller.CameraScriptOn (true);
		levelGuide.SetLevelGuide (0, 0);
		controller.LevelGuideOpen ();
		controller.ResetAll ();
		controller.hudController.LevelCompleted ("Introduction");
		Debug.Log ("Level one started");
	}

	public void Level02 ()
	{
		controller.currentAtomPreset = "box";
		createEnviroment.CreatePresetConfiguration (controller.currentAtomPreset);
		controller.LevelsClose ();
		controller.CameraScriptOn (true);
		levelGuide.SetLevelGuide (1, 0);
		controller.LevelGuideOpen ();
		controller.ResetAll ();
		Debug.Log ("Level eight started");
		controller.hudController.LevelCompleted ("Introduction");
	}

	public void Level03 ()
	{	
		controller.currentAtomPreset = "states_matter";
		createEnviroment.CreatePresetConfiguration (controller.currentAtomPreset);
		controller.ResetAll ();
		controller.LevelsClose ();
		controller.CameraScriptOn (true);
		levelGuide.SetLevelGuide (2, 0);
		controller.LevelGuideOpen ();
		Debug.Log ("Level two started");
		controller.hudController.LevelCompleted ("States of Matter");
	}

	public void Level04 ()
	{
		controller.currentAtomPreset = "Changing_State_with_Tempature";
		createEnviroment.CreatePresetConfiguration (controller.currentAtomPreset);
		controller.ResetAll ();
		controller.LevelsClose ();
		controller.CameraScriptOn (true);
		levelGuide.SetLevelGuide (3, 0);
		controller.LevelGuideOpen ();
		Debug.Log ("Level three started");
		controller.hudController.LevelCompleted ("Tempature");
	}

	public void Level05 ()
	{
		controller.currentAtomPreset = "Changing_State_with_Volume";
		createEnviroment.CreatePresetConfiguration (controller.currentAtomPreset);
		controller.ResetAll ();
		controller.LevelsClose ();
		controller.CameraScriptOn (true);
		levelGuide.SetLevelGuide (4, 0);
		controller.LevelGuideOpen ();
		Debug.Log ("Level four started");
		controller.hudController.LevelCompleted ("Volume");
	}

	public void Level06 ()
	{
		controller.currentAtomPreset = "Everything_is_Made_of_Atoms";
		createEnviroment.CreatePresetConfiguration (controller.currentAtomPreset);
		controller.ResetAll ();
		controller.LevelsClose ();
		controller.CameraScriptOn (true);
		levelGuide.SetLevelGuide (5, 0);
		controller.LevelGuideOpen ();
		Debug.Log ("Level five started");
		controller.hudController.LevelCompleted ("Everything is Atoms");
	}

	public void Level07 ()
	{
		controller.currentAtomPreset = "Forces";
		createEnviroment.CreatePresetConfiguration (controller.currentAtomPreset);
		controller.ResetAll ();
		controller.LevelsClose ();
		controller.CameraScriptOn (true);
		levelGuide.SetLevelGuide (6, 0);
		controller.LevelGuideOpen ();
		Debug.Log ("Level six started");
		controller.hudController.LevelCompleted ("Forces");
	}

	public void Level08 ()
	{
		controller.currentAtomPreset = "Sandbox";
		createEnviroment.CreatePresetConfiguration (controller.currentAtomPreset);
		controller.ResetAll ();
		controller.LevelsClose ();
		controller.CameraScriptOn (true);
		levelGuide.SetLevelGuide (7, 0);
		controller.LevelGuideOpen ();
		Debug.Log ("Level seven started");
	}


}
