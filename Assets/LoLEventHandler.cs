using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LoLSDK;
using SimpleJSON;

public class LoLEventHandler : MonoBehaviour
{
    ILOLSDK sdk;

    public int progress = 0;
    int maxProgress = 6*3;

    //each index represents a level, and tracks the highest index the player has completed
    int[] completedStages = new int[] {0, 0, 0, 0, 0, 0};
    int[][] progressPoints = new int[][]
    {
        new int[] {4, 7, 10}, //Intro
        new int[] {5, 7, 13}, //StatesOfMatter
        new int[] {6, 9, 12}, //Temp
        new int[] {2, 5, 7},  //Volume
        new int[] {1, 2, 4},  //EverythingIsAtoms
        new int[] {1, 2, 4}   //Forces
    };

    // Start is called before the first frame update
    void Start()
    {
        Application.runInBackground = false; //Game pauses when teacher pauses it


        //if (UNITY_EDITOR)
            //ILOLSDK sdk = new LoLSDK.MockWebGL();
        //elif UNITY_WEBGL
            ILOLSDK sdk = new LoLSDK.WebGL();

        //Initialize
        LOLSDK.Init(sdk, "http://localhost:25566");

        //Game is ready
        LOLSDK.Instance.GameIsReady();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickNext(int stageIndex, int textIndex)
    {
        if (stageIndex > 1) 
            stageIndex--; //There's an extra screen of text at element 1 for some reason

        Debug.Log("Stage: " + stageIndex.ToString() + " index: " + textIndex.ToString());

        //Checks if this screen should update progress
        if (Array.IndexOf(progressPoints[stageIndex], textIndex) >= 0)
        {
            //If this is the first time player has reached this screen, uniquely update progress
            if (completedStages[stageIndex] < textIndex) 
            {
                completedStages[stageIndex] = textIndex;
                progress += 1;
                updateProgress();
            }
        }
    }

    public void updateProgress()
    {
        Debug.Log("updated progress: " + progress.ToString());
        LOLSDK.Instance.SubmitProgress(progress, maxProgress);
    }

    public void TTSText(string textToSpeak)
    {
        LOLSDK.Instance.SpeakText(textToSpeak);
    }



    public void CompleteGame()
    {
        LOLSDK.Instance.CompleteGame();
    }
}
