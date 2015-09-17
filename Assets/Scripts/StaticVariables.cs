/**
 * Class: StaticVariables.cs
 * Created By: Justin Moeller
 * Description: This class is simply a list of static variables and static functions
 * that can be called from any class. The static variables in this list are either
 * constants or variables that can be controlled from across the entire system of
 * atom (i.e currentPotential). There are two functions in this class, DrawLine and
 * DrawQuad. DrawLine draw a line in 3D space and DrawQuad draws a quad in 3D space.
 * To use these functions in 2D space, the coordinates given to the function must be
 * translated such that they are rotated based on the rotation of the camera. 
 * 
 * 
 **/ 


using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class StaticVariables {
	
	//makes sure mouseClick is only processed by either Atom or SettingsControl callbacks
	public static bool mouseClickProcessed = false;
	public static bool draggingAtoms = false;
	//MD time steps used in normal, slowmotion, and stop time mode.
	public static float MDTimestepNormal = 1.5f * Mathf.Pow (10, -15);
	public static float MDTimestepSlow = MDTimestepNormal / 10.0f ;
	public static float MDTimestepStop = MDTimestepNormal / 50.0f ;

	public static float MDTimestep = MDTimestepNormal;
	public static float MDTimestepSqr = MDTimestep * MDTimestep;
	public static float MDTimestepInPicosecond = MDTimestep / Mathf.Pow (10, -12);

	public static int maxAtoms = 100; //max number of atoms allowed
	public static Color atomEnabledColor = new Color(23/255.0f, 160/255.0f,242/255.0f, 1.0f);
	public static Color atomDisabledColor = new Color(80/255.0f, 80/255.0f, 80/255.0f, 90/255.0f);
	//scale timeScale with temp
	public static float maxTimeScale = 0.8f;
	public static float baseTimeScale = 0.2f;//when temp = 300k

	public static float maxTimeScaleBuck = 0.65f;
	public static float baseTimeScaleBuck = 0.25f;
	
	public static float clockTimeStart = 0.0f;
	public static float clockTimeEnd = 0.0f;
	
	// This is the number of frames that the program enters a slow motion mode to avoid some gliches.
	public static int slowMotionFrames = 20;
	//Suppose every Update physics interval (e.g. 0.02 seconds) is the
	//Molecular Dynamics timestep of 0.5 * 10^-15 seconds
	//public static float updateIntervalToRealTime = MDTimestep;
	
	//do not scale temperature all at once
	//public static float alphaDrag = 0.1f; original value
	public static float alphaDrag = 0.1f;
	
	//Boltzmann constant in J/K
	public static float kB = 1.381f * (float) Math.Pow(10,-23);
	
	//Permittivity of free space
	public static float epsilon0 = 8.85f * Mathf.Pow (10, -12);
	
	//Convert units of 1 amu to kg
	public static float amuToKg = 1.6605f * (float)Math.Pow(10, -27); 
	
	//Convert units of 100 amu to kg
	public static float massScaler = 1f;
	public static float mass100amuToKg = 100.0f* massScaler * amuToKg; 
	
	//Convert units of Angstroms to meters
	public static float angstromsToMeters = (float) Math.Pow (10,-10);
	
	//Number of MD timesteps to update verlet list
	public static int nVerlet = 100;
	
	//Temperature slider bounds in K
	public static float tempRangeLow = 0.001f;
	public static float tempRangeHigh = 5000.0f; 
	public static float tempDefault = 300.0f;
	public static float tempScaler = 1.0f; //for making atoms faster and increasing frame rate
	public static float desiredTemperature = 300.000f * tempScaler;
	
	public static float volRangeLow = 1.0f;
	public static float volRangeHigh = 4.0f;
	public static float volDefault = 1.0f;
	//this variable causes the bond lines to either draw or not draw
	public static bool drawBondLines = true;
	//the variable pauses the simulation of physics
	public static bool pauseTime = false;
	
	//this variable keeps track of the amount of simulation time that has passed in picoseconds
	public static float currentTime = 0.0f;
	//this variable keeps the total number of time steps passed in the simulation
	public static int iTime = 0;
	
	public static float kineticEnergy = 0.0f;  // units in Joules
	public static float potentialEnergy = 0.0f;  // units in Joules
	public static float currentTemperature = 0.0f;  // units in Kelvin
	
	public static float sqrtAlpha = 1.0f;
	public static float maxTemp = 5000.0f;
	public static float minTemp = 0.001f;
	public static float defaultTemp = 300.0f;

	public static float maxVol = 40.0f; //in angstroms
	public static float minVol = 10.0f;
	public static float defaultVol = 20.0f;
	//this is an enum of the different states that time can pass
	public enum TimeSpeed{
		Normal,
		SlowMotion,
		Stopped
	};
	
	//draws bond as billboards
	//c1 : atom1 center
	//c2 : atom2 center
	// s1--------s2
	// |		  |
	// s4_________s3
	public static void DrawLine(Vector3 c1, Vector3 c2, 
		Color atomColor1, Color atomColor2, float lineWidth, Material mat){

		Vector3 camPos = Camera.main.gameObject.transform.position;

		
		Vector3 c2ToCam = camPos-c2;
		Vector3 c2ToS3 = Vector3.Cross(c1-c2, c2ToCam);
		c2ToS3.Normalize();
		Vector3 s1 = c1 - c2ToS3*lineWidth/2.0f;
		Vector3 s2 = c2 - c2ToS3*lineWidth/2.0f;
		Vector3 s3 = c2 + c2ToS3 * lineWidth/2.0f; 
		Vector3 s4 = s3 + (s1-s2);
	
		if (!mat) {
			return;
		}
		GL.LoadProjectionMatrix (Camera.main.projectionMatrix);
		GL.PushMatrix();
		mat.SetPass (0);
		GL.Begin (GL.QUADS);
		
		GL.Color (atomColor1);
		GL.Vertex (s1);
		GL.Color (atomColor2);
		GL.Vertex (s2);
		GL.Vertex (s3);
		GL.Color (atomColor1);
		GL.Vertex (s4);
		
		GL.End ();
		GL.PopMatrix();
	}
	
	//this function will draw a quad in 3D space given four coordinates and a color
	//Note: this function must be called from within OnPostRender(). It will not display if called from another function
	public static void DrawQuad(Vector3 upperLeft, Vector3 upperRight, Vector3 lowerLeft, Vector3 lowerRight, Color color, Material mat){
		
		if (!mat) {
			return;
		}
		GL.LoadProjectionMatrix (Camera.main.projectionMatrix);
		GL.PushMatrix ();
		mat.SetPass (0);
		GL.Begin (GL.QUADS);
		GL.Color (color);
		GL.Vertex (upperLeft);
		GL.Vertex (upperRight);
		GL.Vertex (lowerRight);
		GL.Vertex (lowerLeft);
		GL.End ();
		GL.PopMatrix ();
	}
	
}
