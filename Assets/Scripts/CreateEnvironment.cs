/**
 * Class: CreateEnvironment.cs
 * Created by: Justin Moeller
 * Description: This class handles anything that has to do with the creation of the environment.
 * (i.e the atoms, the box, the lines, and the width/height/depth text). Most of the functionality 
 * of this class happens when the game begins, however, the box, the lines, and the text all must be 
 * scaled when the user changes the volume of the box. Additionally, to cut down on some computation,
 * all of the sigma values are pre-computed and then stored in a dictionary. The key to the dictionary
 * is a String made up of the two atoms for which you want the sigma value for. (i.e "CopperCopper" or
 * "CopperGold" or "GoldPlatinum") All of the atoms are also named (given a number 0-numAtoms) for easier 
 * access and debugging. 
 * 
 * 
 **/ 

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class CreateEnvironment : MonoBehaviour {
	
	public int numMolecules = 1;
	public Rigidbody[] molecules = new Rigidbody[5];
	
	public GameObject plane;
	public Vector3 centerPos;
	[HideInInspector]public float errorBuffer = 0.5f;
	//line
	public Material lineMat; 
	public Color lineColor;
	public float lineWidth = 0.2f;
	//cube
	[HideInInspector]public float width;
	[HideInInspector]public float height;
	[HideInInspector]public float depth;
	[HideInInspector]public float volume = 8000.0f;

	public TextMesh textMeshPrefab;
	private TextMesh bottomText;
	private TextMesh sideText;
	private TextMesh depthText;

	private TextMesh bottomPlusZText;
	private TextMesh bottomPlusYText;
	private TextMesh bottomPlusYZText;

	private TextMesh sidePlusXText;
	private TextMesh sidePlusZText;
	private TextMesh sidePlusXZText;

	private TextMesh depthPlusYText;
	private TextMesh depthPlusZText;
	private TextMesh depthPlusYZText;

	private Vector3 vx;
	private Vector3 vy;
	private Vector3 vz;
	//planes
	public static GameObject bottomPlane;
	public static GameObject topPlane;
	public static GameObject backPlane;
	public static GameObject frontPlane;
	public static GameObject rightPlane;
	public static GameObject leftPlane;
	[HideInInspector]public AtomTouchGUI atomTouchGUI;
	public Vector3 initialCenterPos;
	
	//this variables points to the instance of the create environment
	public static CreateEnvironment myEnvironment;
	
	void Awake(){
		CreateEnvironment.myEnvironment = this;
		atomTouchGUI = Camera.main.GetComponent<AtomTouchGUI> ();
		atomTouchGUI.changingTemp = false;
		atomTouchGUI.changingVol = false;
		//when first started, pause timer
		StaticVariables.pauseTime = true;

		centerPos = Vector3.zero;
		initialCenterPos = centerPos;
		
		//figure out the dimensions of the box based on the volume
		width = Mathf.Pow (volume, (1.0f / 3.0f));
		height = Mathf.Pow (volume, (1.0f / 3.0f));
		depth = Mathf.Pow (volume, (1.0f / 3.0f));
		
		vx  = new Vector3(width,0.0f,0.0f);
		vy = new Vector3(0.0f,height, 0.0f);
		vz = new Vector3(0.0f,0.0f, depth);

		CreatePlanes();
		


		CreateAllLines();
	}
	void CreatePlanes(){
		//create the bottom plane
		Quaternion bottonPlaneRotation = Quaternion.Euler (0.0f, 0.0f, 0.0f);
		Vector3 bottomPlanePos = new Vector3 (centerPos.x, centerPos.y - (height/2.0f), centerPos.z);
		bottomPlane = Instantiate (plane, bottomPlanePos, bottonPlaneRotation) as GameObject;
		bottomPlane.transform.localScale = new Vector3 (width / 10.0f, height / 10.0f, depth / 10.0f);
		bottomPlane.name = "BottomPlane";
		bottomPlane.tag = "Plane";
		bottomPlane.GetComponent<Collider>().enabled = true;
		
		//create the top plane
		Quaternion topPlaneRotation = Quaternion.Euler (0.0f, 180.0f, 180.0f);
		Vector3 topPlanePos = new Vector3 (centerPos.x, centerPos.y + (height/2.0f), centerPos.z);
		topPlane = Instantiate (plane, topPlanePos, topPlaneRotation) as GameObject;
		topPlane.transform.localScale = new Vector3 (width / 10.0f, height / 10.0f, depth / 10.0f);
		topPlane.name = "TopPlane";
		topPlane.tag = "Plane";
		topPlane.GetComponent<Collider>().enabled = true;
		
		//create the back plane
		Quaternion backPlaneRotation = Quaternion.Euler (270.0f, 0.0f, 0.0f);
		Vector3 backPlanePos = new Vector3 (centerPos.x, centerPos.y, centerPos.z + (depth/2.0f));
		backPlane = Instantiate (plane, backPlanePos, backPlaneRotation) as GameObject;
		backPlane.transform.localScale = new Vector3 (width / 10.0f, depth / 10.0f, height / 10.0f);
		backPlane.name = "BackPlane";
		backPlane.tag = "Plane";
		backPlane.GetComponent<Collider>().enabled = true;
		
		//create the front plane
		Quaternion frontPlaneRotation = Quaternion.Euler (90.0f, 0.0f, 0.0f);
		Vector3 frontPlanePos = new Vector3 (centerPos.x, centerPos.y, centerPos.z - (depth/2.0f));
		frontPlane = Instantiate (plane, frontPlanePos, frontPlaneRotation) as GameObject;
		frontPlane.transform.localScale = new Vector3 (width / 10.0f, depth / 10.0f, height / 10.0f);
		frontPlane.name = "FrontPlane";
		frontPlane.tag = "Plane";
		frontPlane.GetComponent<Collider>().enabled = true;
		
		//create the right plane
		Quaternion rightPlaneRotation = Quaternion.Euler (0.0f, 0.0f, 90.0f);
		Vector3 rightPlanePos = new Vector3 (centerPos.x + (width/2.0f), centerPos.y, centerPos.z);
		rightPlane = Instantiate (plane, rightPlanePos, rightPlaneRotation) as GameObject;
		rightPlane.transform.localScale = new Vector3 (height / 10.0f, width / 10.0f, depth / 10.0f);
		rightPlane.name = "RightPlane";
		rightPlane.tag = "Plane";
		rightPlane.GetComponent<Collider>().enabled = true;
		
		//create the left plane
		Quaternion leftPlaneRotation = Quaternion.Euler (0.0f, 0.0f, 270.0f);
		Vector3 leftPlanePos = new Vector3 (centerPos.x - (width/2.0f), centerPos.y, centerPos.z);
		leftPlane = Instantiate (plane, leftPlanePos, leftPlaneRotation) as GameObject;
		leftPlane.transform.localScale = new Vector3 (height / 10.0f, width / 10.0f, depth / 10.0f);
		leftPlane.name = "LeftPlane";
		leftPlane.tag = "Plane";
		leftPlane.GetComponent<Collider>().enabled = true;
	}
	void CreateLine(ref GameObject plane, ref TextMesh tm, Vector3 positionOffset){
		tm = Instantiate(textMeshPrefab, plane.transform.position + positionOffset, Quaternion.identity) as TextMesh;
		tm.text = "";
		LineRenderer line = tm.transform.gameObject.AddComponent<LineRenderer> ();
		line.material = lineMat;
		line.SetColors(lineColor, lineColor);
		line.SetWidth(lineWidth, lineWidth);
		line.SetVertexCount(2);
	}
	void UpdateLine(ref GameObject plane, ref TextMesh tm, Vector3 offset0, Vector3 offset1){
		
		LineRenderer line = tm.GetComponent<LineRenderer> ();
		Vector3 v0
			= plane.transform.position + offset0;

		Vector3 v1 
			= v0 + offset1;
		line.SetPosition(0, v0);
		line.SetPosition(1, v1);
		line.SetWidth(lineWidth, lineWidth);
		tm.transform.position  = (v0 + v1) /2.0f;
		
	}
	void CreateAllLines(){
		CreateLine(ref bottomPlane,ref bottomText, new Vector3(0.0f, 0.0f, -depth/2.0f));
		CreateLine(ref bottomPlane, ref bottomPlusZText, new Vector3(0.0f, 0.0f, depth/2.0f));
		CreateLine(ref bottomPlane, ref bottomPlusYText, new Vector3(0.0f, height, -depth/2.0f));
		CreateLine(ref bottomPlane, ref bottomPlusYZText, new Vector3(0.0f, height, depth/2.0f));

		CreateLine(ref frontPlane,ref sideText, new Vector3(-width/2.0f, 0.0f, 0.0f));
		CreateLine(ref frontPlane,ref sidePlusXText, new Vector3(width/2.0f, 0.0f, 0.0f));
		CreateLine(ref backPlane,ref sidePlusZText, new Vector3(0.0f, 0.0f,-depth/2.0f));
		CreateLine(ref backPlane,ref sidePlusXZText, new Vector3(0.0f, 0.0f,depth/2.0f));

		CreateLine(ref leftPlane, ref depthText, new Vector3(-height/2.0f, 0.0f, 0.0f));
		CreateLine(ref leftPlane, ref depthPlusYText, new Vector3(height/2.0f,0.0f,0.0f));
		CreateLine(ref rightPlane, ref depthPlusZText, new Vector3(0.0f,0.0f,depth/2.0f));
		CreateLine(ref rightPlane, ref depthPlusYZText, new Vector3(0.0f,0.0f,-depth/2.0f));
	}
	void UpdateAllLines(){
		UpdateLine(ref bottomPlane, ref bottomText, 
			new Vector3(-width/2.0f,0.0f,-depth/2.0f), vx);
		UpdateLine(ref bottomPlane, ref bottomPlusZText, 
			new Vector3(-width/2.0f,0.0f,depth/2.0f), vx);
		UpdateLine(ref bottomPlane, ref bottomPlusYText,
			new Vector3(-width/2.0f,height,-depth/2.0f), vx);
		UpdateLine(ref bottomPlane, ref bottomPlusYZText,
			new Vector3(-width/2.0f,height,depth/2.0f), vx);

		UpdateLine(ref frontPlane, ref sideText,
			new Vector3(-width/2.0f,-height/2.0f,0.0f), vy);
		UpdateLine(ref frontPlane, ref sidePlusXText,
			new Vector3(width/2.0f,-height/2.0f,0.0f), vy);
		UpdateLine(ref backPlane, ref sidePlusZText,
			new Vector3(-width/2.0f, -height/2.0f, 0.0f), vy);
		UpdateLine(ref backPlane, ref sidePlusXZText,
			new Vector3(width/2.0f, -height/2.0f, 0.0f), vy);
		
		UpdateLine(ref leftPlane, ref depthText,
			new Vector3(0.0f,-height/2.0f,-depth/2.0f), vz);
		UpdateLine(ref leftPlane, ref depthPlusYText,
			new Vector3(0.0f, height/2.0f, -depth/2.0f), vz);
		UpdateLine(ref rightPlane, ref depthPlusZText,
			new Vector3(0.0f,-height/2.0f, -depth/2.0f), vz);
		UpdateLine(ref rightPlane, ref depthPlusYZText,
			new Vector3(0.0f,height/2.0f, -depth/2.0f), vz);
	}
	void Start () {
		// pre-compute coefficients used in various types of potentials so that we don't have to calculate them dynamically
		preCompute ();
		
		//create the atoms
		InitAtoms ();
		
		
		//give all of the atoms a random velocity on startup
		atomTouchGUI.AllAtomsKick ();
	}
	
	void Update ()
	{
		
		//when the volume of the box changes the width, height, and depth must change as well. The lines and the text must respond accordingly
		width = Mathf.Pow (volume, (1.0f / 3.0f));
		height = Mathf.Pow (volume, (1.0f / 3.0f));
		depth = Mathf.Pow (volume, (1.0f / 3.0f));

		vx.x = width;
		vy.y = height;
		vz.z = depth;

		UpdateAllLines();
		

		//change the position of the box
		rightPlane.transform.position = new Vector3 (initialCenterPos.x + (width/2.0f), initialCenterPos.y, initialCenterPos.z);
		leftPlane.transform.position = new Vector3 (initialCenterPos.x - (width/2.0f), initialCenterPos.y, initialCenterPos.z);
		bottomPlane.transform.position = new Vector3 (initialCenterPos.x, initialCenterPos.y - (height/2.0f), initialCenterPos.z);
		topPlane.transform.position = new Vector3 (initialCenterPos.x, initialCenterPos.y + (height/2.0f), initialCenterPos.z);
		backPlane.transform.position = new Vector3 (initialCenterPos.x, initialCenterPos.y, initialCenterPos.z + (depth/2.0f));
		frontPlane.transform.position = new Vector3 (initialCenterPos.x, initialCenterPos.y, initialCenterPos.z - (depth/2.0f));
		
		//change the size of the box
		bottomPlane.transform.localScale = new Vector3 (width / 10.0f, height / 10.0f, depth / 10.0f);
		topPlane.transform.localScale = new Vector3 (width / 10.0f, height / 10.0f, depth / 10.0f);
		backPlane.transform.localScale = new Vector3 (width / 10.0f, depth / 10.0f, height / 10.0f);
		frontPlane.transform.localScale = new Vector3 (width / 10.0f, depth / 10.0f, height / 10.0f);
		rightPlane.transform.localScale = new Vector3 (height / 10.0f, width / 10.0f, depth / 10.0f);
		leftPlane.transform.localScale = new Vector3 (height / 10.0f, width / 10.0f, depth / 10.0f);
	}
	
	//make the side text become vertical
	String VerticalText(String text){
		System.Text.StringBuilder verticalText = new System.Text.StringBuilder (text.Length * 2);
		for (int i = 0; i < text.Length; i++) {
			verticalText.Append(text[i]).Append("\n");
		}
		return verticalText.ToString();
	}
	
	public void preCompute()
	{
		if (Potential.currentPotential == Potential.potentialType.LennardJones)
			Potential.myPotential = new LennardJones();
		else if(Potential.currentPotential == Potential.potentialType.Buckingham)
			Potential.myPotential = new Buckingham();
		
		Potential.myPotential.preCompute ();
	}
	
	//initialize the atoms to a random position and to the original number of atoms
	public void InitAtoms()
	{
		//destroy and unregister all the current atoms
		for (int i = Atom.AllAtoms.Count-1; i >= 0; i--)
		{
			Atom currAtom = Atom.AllAtoms [i];
			Atom.UnregisterAtom(currAtom);
			Destroy (currAtom.gameObject);
		}
		if(Potential.currentPotential == Potential.potentialType.Buckingham)
			CreatePresetConfiguration ("box");
		else
			CreateRandomConfiguration();

		AtomTouchGUI.myAtomTouchGUI.TryEnableAddAtomBtns();
	}
	//scene selector callback
	public void LoadPresetAtoms(string filename){
		//destroy and unregister all the current atoms
		for (int i = Atom.AllAtoms.Count-1; i >= 0; i--)
		{
			Atom currAtom = Atom.AllAtoms [i];
			Atom.UnregisterAtom(currAtom);
			Destroy (currAtom.gameObject);
		}
		CreatePresetConfiguration (filename);
		
		AtomTouchGUI.myAtomTouchGUI.TryEnableAddAtomBtns();
	}
	// Create atoms by reading the information from an text input file in Resources folder. The file format is XYZ.
	public void CreatePresetConfiguration(string filename)
	{
		//Debug.Log ("Uploading text file!");
		TextAsset textFile = Resources.Load (filename) as TextAsset;
		//Debug.Log("text file uploaded!");
		Debug.Log("loading " + filename);
		string allLines = textFile.text;
		allLines = allLines.ToLower ();
		string[] lineArray = allLines.Split ("\r\n".ToCharArray (), 
			StringSplitOptions.RemoveEmptyEntries);

		string[] textArray1;
		string[] textArray2;
		
		int numAtoms = int.Parse(lineArray[0]);

		textArray1 = lineArray[1].Split(',');

		for (int i=0; i<textArray1.Length; i++)
		{

			textArray2 = textArray1[i].Split('=');
			string attr = textArray2[0].Trim().ToLower();
			
			if (attr.Equals("potential"))
			{
				string val = textArray2[1].Trim().ToLower();
				if (val.Equals("buckingham")){
					Potential.currentPotential = Potential.potentialType.Buckingham;
					SettingsControl.currentPotentialType 
						= Potential.potentialType.Buckingham;

					SettingsControl.mySettings.buckinghamOn.GetComponent<Toggle>().isOn = true;
					SettingsControl.mySettings.lenJonesOn.GetComponent<Toggle>().isOn = false;
				}else if (val.Equals("lennardjones")){
					Potential.currentPotential = Potential.potentialType.LennardJones;
					SettingsControl.currentPotentialType 
						= Potential.potentialType.LennardJones;

					SettingsControl.mySettings.buckinghamOn.GetComponent<Toggle>().isOn = false;
					SettingsControl.mySettings.lenJonesOn.GetComponent<Toggle>().isOn = true;
				}
				SettingsControl.mySettings.OnChange_SimType();

			}
			else if (attr.Equals("temperature"))
			{
				string val = textArray2[1].Trim().ToLower();
				StaticVariables.desiredTemperature = float.Parse(textArray2[1]);
				//change temp slider value
				atomTouchGUI.tempSliderComponent.value 
					= StaticVariables.tempRangeHigh - StaticVariables.desiredTemperature;
				

				atomTouchGUI.SnapTempToInterval(10.0f);
				atomTouchGUI.changingTemp = false;
				
			}
			else if (attr.Equals("volume"))
			{
				string val = textArray2[1].Trim().ToLower();
				myEnvironment.volume = float.Parse(textArray2[1]);
				float depth = Mathf.Pow(myEnvironment.volume, 1f/3f);
				float sliderVal = 
					(StaticVariables.maxVol + StaticVariables.minVol-depth)/10.0f;

				//change vol slider value
				atomTouchGUI.volSliderComponent.value = sliderVal;
				atomTouchGUI.SnapVolumeToInterval(0.5f);
				atomTouchGUI.changingVol = false;
			}
			/*
			else if (attr.Equals("timespeed"))
			{
				string val = textArray2[1].Trim().ToLower();
				if (val.Equals("stopped")){
					AtomTouchGUI.currentTimeSpeed = StaticVariables.TimeSpeed.Stopped;
				}else if (val.Equals("slowmotion")){
					AtomTouchGUI.currentTimeSpeed = StaticVariables.TimeSpeed.SlowMotion;
				}else{
					AtomTouchGUI.currentTimeSpeed = StaticVariables.TimeSpeed.Normal;
				}
				atomTouchGUI.changeTimer();
			}
			*/

		}
		
		for (int iLine=2; iLine < 2+numAtoms; iLine++)
		{
			textArray1 = lineArray[iLine].Split('\t');
			Vector3 pos = new Vector3(float.Parse(textArray1[1]), float.Parse(textArray1[2]), float.Parse(textArray1[3]));
			Vector3 vel = new Vector3(float.Parse(textArray1[4]), float.Parse(textArray1[5]), float.Parse(textArray1[6]));

			int atomID = 0;

			if (textArray1[0].Equals("cu"))
				atomID = 0;
			if (textArray1[0].Equals("au"))
				atomID = 1;
			if (textArray1[0].Equals("pt"))
				atomID = 2;
			if (textArray1[0].Equals("na"))
				atomID = 3;
			if (textArray1[0].Equals("cl"))
				atomID = 4;

			Rigidbody preFab = molecules[atomID];
			
			int preFabID = preFab.GetInstanceID();
			if(preFabID == atomTouchGUI.copperPrefab.GetInstanceID()){
				Copper.count++;
				atomTouchGUI.copperCount.GetComponent<Text>().text = "Cu: " + Copper.count;
			}else if(preFabID == atomTouchGUI.goldPrefab.GetInstanceID()){
				Gold.count++;
				atomTouchGUI.goldCount.GetComponent<Text>().text = "Au: " + Gold.count;
			}else if(preFabID == atomTouchGUI.platinumPrefab.GetInstanceID()){
				Platinum.count++;
				atomTouchGUI.platinumCount.GetComponent<Text>().text = "Pt: " + Platinum.count;
			}
			Quaternion curRotation = Quaternion.Euler(0, 0, 0);
			Instantiate(preFab, myEnvironment.centerPos, curRotation);
			
			int i = Atom.AllAtoms.Count-1;
			Atom currAtom = Atom.AllAtoms[i];
			currAtom.gameObject.name = currAtom.GetInstanceID().ToString();
			currAtom.GetComponent<Rigidbody>().freezeRotation = true;
			
			currAtom.position =  pos;
			currAtom.transform.position = currAtom.position;
			//kick it
			currAtom.velocity = vel;
			Potential.myPotential.calculateVerletRadius (currAtom);
		}
		
		preCompute ();
		
	}

	private void CreateRandomConfiguration()
	{
		//initialize the new atoms
		if (Potential.currentPotential == Potential.potentialType.LennardJones)
		{
			Debug.Log(molecules[0].gameObject.name);
			for (int i = 0; i < numMolecules; i++)
			{
				createAtom (molecules [0]);
			}
		}
		else if (Potential.currentPotential == Potential.potentialType.Buckingham)
		{
			for (int i = 0; i < numMolecules/2; i++)
			{
				//sodium
				createAtom (molecules [3]);
			}
			for (int i = numMolecules/2; i < numMolecules; i++)
			{
				//chlorine
				createAtom (molecules [4]);
			}
		}
		atomTouchGUI.AllAtomsKick();
	}
	
	// this method creates a new atom from the type of the preFab and checks the position to have a far enough distance from other atoms
	public void createAtom(Rigidbody preFab)
	{
		int preFabID = preFab.GetInstanceID();
		if(preFabID == atomTouchGUI.copperPrefab.GetInstanceID()){
			Copper.count++;
			atomTouchGUI.copperCount.GetComponent<Text>().text = "Cu: " + Copper.count;
		}else if(preFabID == atomTouchGUI.goldPrefab.GetInstanceID()){
			Gold.count++;
			atomTouchGUI.goldCount.GetComponent<Text>().text = "Au: " + Gold.count;
		}else if(preFabID == atomTouchGUI.platinumPrefab.GetInstanceID()){
			Platinum.count++;
			atomTouchGUI.platinumCount.GetComponent<Text>().text = "Pt: " + Platinum.count;
		}
		CreateEnvironment myEnvironment = CreateEnvironment.myEnvironment;
		Quaternion curRotation = Quaternion.Euler(0, 0, 0);
		preFab.gameObject.GetComponent<MeshRenderer>().enabled = SettingsControl.renderAtoms;

		Instantiate(preFab, myEnvironment.centerPos, curRotation);

		int i = Atom.AllAtoms.Count-1;
		Atom currAtom = Atom.AllAtoms[i];
		currAtom.gameObject.name = currAtom.GetInstanceID().ToString();
		//Debug.Log(currAtom.GetInstanceID());
		currAtom.GetComponent<Rigidbody>().freezeRotation = true;
		currAtom.GetComponent<TrailRenderer>().enabled = SettingsControl.mySettings.trailsToggle.isOn;

		float realWidth = myEnvironment.width - 2.0f * myEnvironment.errorBuffer;
		float realHeight = myEnvironment.height - 2.0f * myEnvironment.errorBuffer;
		float realDepth = myEnvironment.depth - 2.0f * myEnvironment.errorBuffer;
		Vector3 centerPos = myEnvironment.centerPos;
		
		int tryNumber = 0;
		
		bool proximityFlag = false;
		while ((proximityFlag == false) && (tryNumber < 100))
		{
			tryNumber ++;
			currAtom.position =  new Vector3 (centerPos.x + (UnityEngine.Random.Range (-realWidth/2.0f, realWidth/2.0f)), centerPos.y + (UnityEngine.Random.Range (-realHeight/2.0f, realHeight/2.0f)), centerPos.z + (UnityEngine.Random.Range (-realDepth/2.0f, realDepth/2.0f)));
			proximityFlag = myEnvironment.checkProximity(currAtom);
		}
		currAtom.transform.position = currAtom.position;
		//kick it
		atomTouchGUI.AtomKick(i);
		Potential.myPotential.calculateVerletRadius (currAtom);

		if ((tryNumber == 100) && (proximityFlag == false)) 
		{
			Atom.UnregisterAtom(currAtom);
			Destroy (currAtom.gameObject);
			Debug.Log ("No space for atoms!");
		}
		
	}
	
	//check the distance between the atoms, if it is larger than the equilibrium position move accept the random number, otherwise pick another set of random positions.
	private bool checkProximity(Atom currAtom)
	{
		bool proximityFlag = true;
		for (int i = 0; i < Atom.AllAtoms.Count - 1; i++)
		{
			Atom otherAtom = Atom.AllAtoms[i];
			Vector3 deltaR = Boundary.myBoundary.deltaPosition(currAtom, otherAtom);
			
			float distanceSqr = deltaR.sqrMagnitude;
			//only get the forces of the atoms that are within the cutoff range
			if (distanceSqr < (3.0f * 3.0f))
			{
				proximityFlag = false;
			}
		}
		return proximityFlag;
	}
}
