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
    int[,] progressPoints = new int[,]
    {
        {4, 7, 10}, //Intro
        {5, 7, 13}, //StatesOfMatter
        {6, 9, 12}, //Temp
        {2, 5, 7},  //Volume
        {1, 2, 4},  //EverythingIsAtoms
        {1, 2, 4}   //Forces
    };

    // Start is called before the first frame update
    void Start()
    {
        Application.runInBackground = false; //Game pauses when teacher pauses it


        //if (UNITY_EDITOR)
            ILOLSDK sdk = new LoLSDK.MockWebGL();
        //elif UNITY_WEBGL
        //ILOLSDK sdk = new LoLSDK.WebGL();

        //Initialize
        LOLSDK.Init(sdk, "I think a website link goes here?");

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
        //Checks if this screen should update progress, ugly because I couldn't get a row from a 2d array, sorry about that :/
        if ((progressPoints[stageIndex, 0] == textIndex) || (progressPoints[stageIndex, 1] == textIndex) || (progressPoints[stageIndex, 2] == textIndex))
        {
            //Progress should be updated on this screen

            if (completedStages[stageIndex] < textIndex) //First time player has reached this screen, uniquely update progress
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
