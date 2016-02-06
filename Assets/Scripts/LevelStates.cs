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
		controller.hudController.LevelCompleted ("the Introduction Activity");
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
		controller.hudController.LevelCompleted ("Practice Your Skills");
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
		controller.hudController.LevelCompleted ("the States of Matter Activity");
		controller.hudController.blockPanel.SetActive (true);
		StaticVariables.canSelectAtoms = false;
		controller.changeTimer ();
		controller.changeTimer ();
	}

	public void Level04 ()
	{
		controller.currentAtomPreset = "Changing_State_with_Tempature";
		createEnviroment.CreatePresetConfiguration (controller.currentAtomPreset);
		controller.ResetAll ();
		controller.LevelsClose ();
		controller.CameraScriptOn (true);
		levelGuide.SetLevelGuide (3, 0);
		AtomTouchGUI.currentTimeSpeed = StaticVariables.TimeSpeed.SlowMotion;
		controller.changeTimer ();
		controller.LevelGuideOpen ();
		Debug.Log ("Level three started");
		controller.hudController.LevelCompleted ("the Temperature Activity");
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
		controller.hudController.LevelCompleted ("the Everything is Atoms Activity");
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
		controller.hudController.LevelCompleted ("the Forces Activity");
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
