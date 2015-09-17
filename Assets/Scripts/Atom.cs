/**
 * Class: Atom.cs
 * Created by: Justin Moeller
 * Description: This is the main class for dealing with anything related to the atoms.
 * The atoms' forces are calculated in FixedUpdate, then their velocities are scaled based
 * on the temperature of the system. FixedUpdate is currently called every .0005 seconds,
 * which is Time.fixedDeltaTime. In the FixedUpdate function, it is clearly marked where the
 * code for the new potentials should go. The rest of the class handles the user interactions with 
 * the atoms, such as double tapping or moving an atom. Because OnMouseDown, OnMouseDrag, and
 * OnMouseUp are not supported for iOS, the code that handles dragging an atom had to be implemented
 * twice, once for iOS and once for PC. This class is the base class for copper, gold, and platinum
 * and every atom runs this script. The abstract variables and functions that are declared must be 
 * defined by the children of this class because they are probably different per child. (i.e sigma and epsilon)
 * The collision detection for each atom is NOT handled by Unity, but rather in the Update function.
 * It simply checks if the atom is within the box, and if its not, it reverses its velocity to go back
 * inside the box. Transparency and selection are handled by passing the call to their child, then
 * changing the material of the atom.
 * 
 * 
 **/

//L-J potentials from Zhen and Davies, Phys. Stat. Sol. a, 78, 595 (1983)
//Symbol, epsilon/k_Boltzmann (K) n-m version, 12-6 version, sigma (Angstroms),
//     mass in amu, mass in (20 amu) for Unity 
//     FCC lattice parameter in Angstroms, expected NN bond (Angs)
//Au: 4683.0, 5152.9, 2.6367, 196.967, 9.848, 4.080, 2.88
//Cu: 3401.1, 4733.5, 2.3374,  63.546, 3.177, 3.610, 2.55
//Pt: 7184.2, 7908.7, 2.5394, 165.084, 8.254, 3.920, 2.77

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class Atom : MonoBehaviour
{
	[HideInInspector]public AtomTouchGUI atomTouchGUI;

	private Vector3 offset;
	private Vector3 screenPoint;
	private Vector3 lastMousePosition;
	private Vector3 lastTouchPosition;
	private float deltaTouch2 = 0.0f;
	private bool moveZDirection = false;
	public bool isTransparent = false;
	[HideInInspector]public bool selected = false;
	[HideInInspector]public bool doubleTapped = false;
	//these dictionaries are used when moving groups of atoms. The key is the atom's name: (i.e "0" or "1")
	public static Dictionary<string, Vector3> gameObjectOffsets;
	public static Dictionary<string, Vector3> gameObjectScreenPoints;
	private float dragStartTime;
	private bool dragCalled;
	
	protected static List<Atom> m_AllAtoms = new List<Atom> ();
	
	public TextMesh textMeshPrefab;
	
	//Materials
	public Material untexturedDefaultMaterial;
	public Material untexturedSelectedMaterial;
	public Material untexturedTransparentMaterial;
	public Material defaultMaterial;
	public Material selectedMaterial;
	public Material transparentMaterial;
	//public Material 
	public bool held { get; set; }
	public int state;
	public enum State{
		BeingDragged,
		Held,
		Default,
		Selected
	};
	//variables that must be implemented because they are declared as abstract in the base class
	public abstract float epsilon{ get; } // J
	public abstract float sigma { get; }
	public abstract float massamu{ get; } //amu
	//change material
	public void SetSelected (bool selected){
		MeshRenderer mr = GetComponent<MeshRenderer>();
		if(SettingsControl.textureOn){
			if(selected){
				mr.material = selectedMaterial;
			}else{
				mr.material = defaultMaterial;
			}
		}else{
			if(selected){
				mr.material = untexturedSelectedMaterial;
			}else{
				mr.material = untexturedDefaultMaterial;
			}
		}
	}
	public void SetTransparent (bool transparent){
		MeshRenderer mr = GetComponent<MeshRenderer>();
		if(SettingsControl.textureOn){
			if(transparent){
				isTransparent = true;
				mr.material = transparentMaterial;
			}else{
				mr.material = defaultMaterial;
			}
		}else{
			if(transparent){
				isTransparent = false;
				mr.material = untexturedTransparentMaterial;
			}else{
				mr.material = untexturedDefaultMaterial;
			}
		}
	}
	public abstract String atomName { get; }
	public abstract int atomID { get;}
	
	public abstract float buck_A { get; } // Buckingham potential coefficient
	public abstract float buck_B { get; } // Buckingham potential coefficient
	public abstract float buck_C { get; } // Buckingham potential coefficient
	public abstract float buck_D { get; } // Buckingham potential coefficient
	public abstract float Q_eff { get; } // Ion effective charge for use in Buckingham potential
	
	public float verletRadius = 0.0f;
	public List<Atom> neighborList = new List<Atom> ();
	
	//variables for performing the velocity verlet algorithm
	public Vector3 velocity = Vector3.zero;
	public Vector3 position = Vector3.zero;
	public Vector3 accelerationNew = Vector3.zero;
	public Vector3 accelerationOld = Vector3.zero;
	

	void Awake(){
		RegisterAtom (this);
		atomTouchGUI = Camera.main.gameObject.GetComponent<AtomTouchGUI>();
		state = (int)State.Default;
	}
	void Start(){
	}
	//void OnDestroy(){
	//	UnregisterAtom (this);
	//}
	
	
	// method to extract the list of allAtoms
	public static List<Atom> AllAtoms { 
		get {
			return m_AllAtoms;
		}
	}
	
	// method to register an added atom to the list of allAtoms
	protected static void RegisterAtom( Atom atom ) {
		m_AllAtoms.Add (atom);
	}
	
	// method to unregister a removed atom from the list of allAtoms
	public static void UnregisterAtom( Atom atom ) { 
		m_AllAtoms.Remove( atom );
	}
	
	
	//this function takes care of double tapping, collision detection, 
	//and detecting OnMouseDown, OnMouseDrag, and OnMouseUp on iOS
	void Update(){	
		if(SettingsControl.GamePaused)return;
		if(StaticVariables.mouseClickProcessed)return;
		if(!Application.isMobilePlatform)return;
		//StaticVariables.draggingAtoms = true;
		if(Input.touchCount > 0){
			
			Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
			RaycastHit hitInfo;
			if(state == (int)State.Held){
				if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved){
					state = (int)State.BeingDragged;
					//update position
					OnMouseDragIOS();
					lastTouchPosition = Input.GetTouch(0).position;
				}
				else if(Input.GetTouch(0).phase == TouchPhase.Canceled 
					|| Input.GetTouch(0).phase == TouchPhase.Ended){
					//user let go of the atom
					state = (int)State.Default;
					OnMouseUpIOS();
				}
				else if(Input.touchCount == 2){
						//handle z axis movement
						state = (int)State.BeingDragged;
						HandleZAxisTouch();
						lastTouchPosition = Input.GetTouch(0).position;
				}
				else if(Input.GetTouch(0).phase == TouchPhase.Canceled 
					|| Input.GetTouch(0).phase == TouchPhase.Ended){
					state = (int)State.Default;
					ResetTransparency();
				}
			}
			else if(state == (int)State.BeingDragged){
				OnMouseDragIOS();
				lastTouchPosition = Input.GetTouch(0).position;
				if(Input.GetTouch(0).phase == TouchPhase.Canceled 
					|| Input.GetTouch(0).phase == TouchPhase.Ended){
					//user let go of the atom
					state = (int)State.Default;
					OnMouseUpIOS();
				}
				else if(Input.touchCount == 2){
					//handle z axis movement
					HandleZAxisTouch();
					lastTouchPosition = Input.GetTouch(0).position;
				}
			}
			else if(state == (int)State.Default){
				//held = true;
				//Debug.Log("raycast hit atom!");
				if(Physics.Raycast(ray, out hitInfo)
					&& hitInfo.transform.gameObject.tag == "Molecule" 
					&& hitInfo.transform.gameObject == gameObject){
				
					if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began){
						state = (int)State.Held;
						OnTouch();
						lastTouchPosition = Input.GetTouch(0).position;
					}
					else if(Input.touchCount == 2){
						//handle z axis movement
						state = (int)State.BeingDragged;
						HandleZAxisTouch();
						lastTouchPosition = Input.GetTouch(0).position;
					}
					else if(Input.GetTouch(0).phase == TouchPhase.Canceled 
						|| Input.GetTouch(0).phase == TouchPhase.Ended){
						ResetTransparency();
					}
				}
				
				
			}
		}
		else if(Input.touchCount == 0){
			held = false;
		}
	}
	public static void EnableSelectAtomGroup(bool enable){
		AtomTouchGUI atomTouchGUI = Camera.main.GetComponent<AtomTouchGUI>();
		atomTouchGUI.selectAtomPanel.SetActive(enable);
	}

	public void ShowHelpTip(){
		StartCoroutine(Tooltip.self.Fade());
	}
	//this function gives the user the ability to control the z-axis of the atom on iOS
	void HandleZAxisTouch(){
		if(Input.touchCount == 2){
			Touch touch2 = Input.GetTouch(1);
			if(touch2.phase == TouchPhase.Began){
				moveZDirection = true;
			}
			else if(touch2.phase == TouchPhase.Moved){
				if(!selected){
					//this is for one atom
					Vector2 touchOnePrevPos = touch2.position - touch2.deltaPosition;
					float deltaMagnitudeDiff = touch2.position.y - touchOnePrevPos.y;
					deltaTouch2 = deltaMagnitudeDiff / 4.0f;

					Quaternion cameraRotation = Camera.main.transform.rotation;
					Vector3 projectPosition = transform.position;
					projectPosition += (cameraRotation * new Vector3(0.0f, 0.0f, deltaTouch2));
					Vector3 newPosition = CheckPosition(projectPosition);
					if(newPosition == projectPosition){
						transform.position = CheckPosition(projectPosition);
						this.position = CheckPosition(projectPosition);
						screenPoint += new Vector3(0.0f, 0.0f, deltaTouch2);
					}
					

					
				}
				else{
					//this is for a group of atoms
					Vector2 touchOnePrevPos = touch2.position - touch2.deltaPosition;
					float deltaMagnitudeDiff = touch2.position.y - touchOnePrevPos.y;
					deltaTouch2 = deltaMagnitudeDiff / 4.0f;
					Dictionary<string, Vector3> newAtomPositions = new Dictionary<string, Vector3>();
					bool moveAtoms = true;
					for(int i = 0; i < AllAtoms.Count && moveAtoms; i++){
						Atom currAtom = AllAtoms[i];
						if(!currAtom.selected) continue;
						Quaternion cameraRotation = Camera.main.transform.rotation;
						Vector3 projectPosition = currAtom.transform.position;
						projectPosition += (cameraRotation * new Vector3(0.0f, 0.0f, deltaTouch2));
						Vector3 newAtomPosition = CheckPosition(projectPosition);
						if(newAtomPosition != projectPosition){
							moveAtoms = false;
							if(gameObjectScreenPoints != null){
								gameObjectScreenPoints[currAtom.name] 
									+= new Vector3(0.0f, 0.0f, deltaTouch2);
							}
						}else{
							newAtomPositions.Add(currAtom.name, newAtomPosition);
						}
						
						
					}
					
					if(newAtomPositions.Count > 0 && moveAtoms){
						for(int i = 0; i < AllAtoms.Count; i++){
							Atom currAtom = AllAtoms[i];
							if(!currAtom.selected) continue;
							Vector3 newAtomPosition = newAtomPositions[currAtom.name];
							currAtom.transform.position = newAtomPosition;
							currAtom.position = newAtomPosition;
						}
					}
				}
			}
		}
		else if(Input.touchCount == 0 && moveZDirection){
			//this resets the neccesary variables so the atom can move in two dimensions again. It also resets the atom's material
			moveZDirection = false;
			held = false;
			for(int i = 0; i < AllAtoms.Count; i++){
				Atom currAtom = AllAtoms[i];
				currAtom.SetSelected(currAtom.selected);
			}
			
		}
	}
	
	//reset all of the atoms double tapped to false
	void ResetDoubleTapped(){
		for (int i = 0; i < AllAtoms.Count; i++) {
			Atom currAtom = AllAtoms[i];
			currAtom.doubleTapped = false;
		}
	}
	
	//this is the equivalent of OnMouseDown, but for iOS
	//void OnMouseDownIOS(){
	void OnTouch(){
		if (!Application.isMobilePlatform)return;
		if(SettingsControl.GamePaused)return;
		//Debug.Log("touch");
		/*
		RectTransform rt = AtomTouchGUI.myAtomTouchGUI.buttonPanel.GetComponent<RectTransform>();

		if(Input.mousePosition.x < 
			rt.rect.width *  AtomTouchGUI.myAtomTouchGUI.hud.GetComponent<Canvas>().scaleFactor
			|| Input.mousePosition.x > 
			Screen.width-rt.rect.width * AtomTouchGUI.myAtomTouchGUI.hud.GetComponent<Canvas>().scaleFactor){
			return;
		}
		*/
		dragStartTime = Time.realtimeSinceStartup;
		dragCalled = false;
		held = true;
		if (!selected) {
			//this is for one atom
			screenPoint = Camera.main.WorldToScreenPoint(transform.position);
			//the -15.0f here is for moving the atom above your finger
			offset = transform.position - Camera.main.ScreenToWorldPoint(
				new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y - 15.0f, screenPoint.z));
		}
		else{
			//this is for a group of atoms
			gameObjectOffsets = new Dictionary<string, Vector3>();
			gameObjectScreenPoints = new Dictionary<string, Vector3>();
			for(int i = 0; i < AllAtoms.Count; i++){
				Atom currAtom = AllAtoms[i];
				if(currAtom.selected){
					currAtom.GetComponent<Rigidbody>().isKinematic = true;
					Vector3 pointOnScreen = Camera.main.WorldToScreenPoint(currAtom.transform.position);
					//the -15.0f here is for moving the atom above your finger
					Vector3 atomOffset = currAtom.transform.position - Camera.main.ScreenToWorldPoint(
						new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y - 15.0f, pointOnScreen.z));
					currAtom.held = true;
					gameObjectOffsets.Add(currAtom.name, atomOffset);

					//Debug.Log("added atom offset: " + currAtom.name + ", " + atomOffset);
					gameObjectScreenPoints.Add(currAtom.name, pointOnScreen);
					//Debug.Log("added point on screen: " + currAtom.name + ", " + pointOnScreen);
				}
			}
		}
	}
	
	//controls for debugging on pc
	
	void OnMouseDown (){
		if(SettingsControl.GamePaused)return;
		if (Application.isMobilePlatform)return;
		/*
		RectTransform rt = AtomTouchGUI.myAtomTouchGUI.buttonPanel.GetComponent<RectTransform>();


		if(Input.mousePosition.x < 
			rt.rect.width *  AtomTouchGUI.myAtomTouchGUI.hud.GetComponent<Canvas>().scaleFactor
			|| Input.mousePosition.x > 
			Screen.width-rt.rect.width * AtomTouchGUI.myAtomTouchGUI.hud.GetComponent<Canvas>().scaleFactor){
			return;
		}
		*/
		dragStartTime = Time.realtimeSinceStartup;
		dragCalled = false;
		held = true;
		
		if(!selected){
			//this is for one atom
			screenPoint = Camera.main.WorldToScreenPoint(transform.position);
			//the -15.0 here is for moving the atom above your mouse
			offset = transform.position - Camera.main.ScreenToWorldPoint(
				new Vector3(Input.mousePosition.x, Input.mousePosition.y - 15.0f, screenPoint.z));
			//Debug.Log("mouse down, atom not selected");
		}else{
			//this is for a group of atoms
			gameObjectOffsets = new Dictionary<string, Vector3>();
			gameObjectScreenPoints = new Dictionary<string, Vector3>();
			for(int i = 0; i < AllAtoms.Count; i++){
				Atom currAtom = AllAtoms[i];
				if(currAtom.selected){
					currAtom.GetComponent<Rigidbody>().isKinematic = true;
					Vector3 pointOnScreen = Camera.main.WorldToScreenPoint(currAtom.transform.position);
					//the -15.0 here is for moving the atom above your mouse
					Vector3 atomOffset = currAtom.transform.position - Camera.main.ScreenToWorldPoint(
						new Vector3(Input.mousePosition.x, Input.mousePosition.y - 15.0f, pointOnScreen.z));
					currAtom.held = true;
					gameObjectOffsets.Add(currAtom.name, atomOffset);
					gameObjectScreenPoints.Add(currAtom.name, pointOnScreen);
					
				}
			}
		}
		
	}
	
	//this is the equivalent of OnMouseDrag for iOS
	void OnMouseDragIOS(){
		if(SettingsControl.GamePaused)return;
		if(atomTouchGUI.changingTemp || atomTouchGUI.changingVol){
			return;
		}
		StaticVariables.draggingAtoms = true;
		/*
		RectTransform rt = AtomTouchGUI.myAtomTouchGUI.buttonPanel.GetComponent<RectTransform>();

		if(Input.mousePosition.x < 
			rt.rect.width *  AtomTouchGUI.myAtomTouchGUI.hud.GetComponent<Canvas>().scaleFactor ){
			
			return;
		}
		*/
		Debug.Log("calling mobile drag");
		if (Time.realtimeSinceStartup - dragStartTime > 0.1f) {
			Debug.Log("time diff passed");
			dragCalled = true;
			if(!Tooltip.fadePlayed){
				ShowHelpTip();
			}
			
			ApplyTransparency();
			GetComponent<Rigidbody>().isKinematic = true;
			if(!selected){
				//this is for one atom
				Vector3 diffVector 
					= new Vector3(lastTouchPosition.x, lastTouchPosition.y) 
					- new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
				if(diffVector.magnitude > 0 && Input.touchCount == 1){
					Debug.Log("dragging");
					Vector3 curScreenPoint = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, screenPoint.z);
					Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
					curPosition = CheckPosition(curPosition);
					transform.position = curPosition;
					this.position = curPosition;
				}
			}
			else{
				//only move the atoms if none of them have been double tapped
				
				List<Vector3> atomPositions = new List<Vector3>();
				bool moveAtoms = true;
				for(int i = 0; i < AllAtoms.Count; i++){
					Atom currAtom = AllAtoms[i];
					Vector3 newAtomPosition = currAtom.transform.position;
					Vector3 diffVector = new Vector3(lastTouchPosition.x, lastTouchPosition.y) - new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
					if(diffVector.magnitude > 0 && currAtom.selected && Input.touchCount == 1){
						if(gameObjectOffsets != null && gameObjectScreenPoints != null){
							if(!gameObjectScreenPoints.ContainsKey(currAtom.name)){
								Debug.Log("screen points key not found: " + currAtom.name);
								return;
							}
							if(!gameObjectOffsets.ContainsKey(currAtom.name)){
								Debug.Log("offsets key not found: " + currAtom.name);
								return;
							}
							Vector3 currScreenPoint = gameObjectScreenPoints[currAtom.name];
							Vector3 currOffset = gameObjectOffsets[currAtom.name];
							
							Vector3 objScreenPoint = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, currScreenPoint.z);
							Vector3 curPosition = Camera.main.ScreenToWorldPoint(objScreenPoint) + currOffset;
							newAtomPosition = CheckPosition(curPosition);
							if(newAtomPosition != curPosition){
								moveAtoms = false;
							}
						}
					}
					Vector3 finalPosition = newAtomPosition;
					atomPositions.Add(finalPosition);
				}
				//only move the atoms if none of them have hit the wall of the box
				if(atomPositions.Count > 0 && moveAtoms){
					for(int i = 0; i < AllAtoms.Count; i++){
						Vector3 newAtomPosition = atomPositions[i];
						Atom currAtom = AllAtoms[i];
						currAtom.transform.position = newAtomPosition;
						currAtom.position = newAtomPosition;
					}
				}
				
			}
		}
	}
	
	void OnMouseDrag(){
		if(SettingsControl.GamePaused)return;
		if(atomTouchGUI.changingTemp || atomTouchGUI.changingVol)return;
		Debug.Log("calling pc drag");
		if(!Tooltip.fadePlayed)
			ShowHelpTip();
		/*
		RectTransform rt = AtomTouchGUI.myAtomTouchGUI.buttonPanel.GetComponent<RectTransform>();

		if(Input.mousePosition.x < 
			rt.rect.width *  AtomTouchGUI.myAtomTouchGUI.hud.GetComponent<Canvas>().scaleFactor
			|| Input.mousePosition.x > 
			Screen.width-rt.rect.width * AtomTouchGUI.myAtomTouchGUI.hud.GetComponent<Canvas>().scaleFactor){
			return;
		}
		*/
		StaticVariables.draggingAtoms = true;
		if (Application.isMobilePlatform)return;
		
		if(Time.realtimeSinceStartup - dragStartTime > 0.1f){
			dragCalled = true;
			Quaternion cameraRotation = Camera.main.transform.rotation;
			ApplyTransparency();
			GetComponent<Rigidbody>().isKinematic = true;
			
			if(!selected){
				//this is for one atom
				if((lastMousePosition - Input.mousePosition).magnitude > 0 ){
					Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
					Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
					curPosition = CheckPosition(curPosition);
					transform.position = curPosition;
					this.position = curPosition;
				}
				//this is the implementation of moving the atom in the z-direction
				float deltaZ = -Input.GetAxis("Mouse ScrollWheel");
				Vector3 projectPosition = transform.position;
				projectPosition += (cameraRotation * new Vector3(0.0f, 0.0f, deltaZ));
				Vector3 newPosition = CheckPosition(projectPosition);
				if(newPosition == projectPosition){
					transform.position = CheckPosition(projectPosition);
					this.position = CheckPosition(projectPosition);
					screenPoint += new Vector3(0.0f, 0.0f, deltaZ);
				}
				
				
			}
			else{
				//this is for a group of atoms
				
				//only move the atoms if none of them have been double tapped
				
				List<Vector3> atomPositions = new List<Vector3>();
				bool moveAtoms = true;
				for(int i = 0; i < AllAtoms.Count && moveAtoms; i++){
					Atom currAtom = AllAtoms[i];
					Vector3 newAtomPosition = currAtom.transform.position;
					if((lastMousePosition - Input.mousePosition).magnitude > 0 && currAtom.selected){
						if(!gameObjectScreenPoints.ContainsKey(currAtom.name)){
							Debug.Log("screen points key not found");
							return;
						}
						if(!gameObjectOffsets.ContainsKey(currAtom.name)){
							Debug.Log("offsets key not found");
							return;
						}
						Vector3 currScreenPoint = gameObjectScreenPoints[currAtom.name];
						Vector3 currOffset = gameObjectOffsets[currAtom.name];

						Vector3 objScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, currScreenPoint.z);
						Vector3 curPosition = Camera.main.ScreenToWorldPoint(objScreenPoint) + currOffset;
						newAtomPosition = CheckPosition(curPosition);
						if(newAtomPosition != curPosition){
							moveAtoms = false;
						}
						//currAtom.transform.position = newAtomPosition;
						//currAtom.position = newAtomPosition;
					}
					
					//handle z scroll
					Vector3 finalPosition = newAtomPosition;
					
					if(currAtom.selected){
						float deltaZ = -Input.GetAxis("Mouse ScrollWheel");
						Vector3 projectPosition = newAtomPosition;
						projectPosition += (cameraRotation * new Vector3(0.0f, 0.0f, deltaZ));
						finalPosition = CheckPosition(projectPosition);
						gameObjectScreenPoints[currAtom.name] += new Vector3(0.0f, 0.0f, deltaZ);
						if(finalPosition != projectPosition){
							moveAtoms = false;
						}
					}
					atomPositions.Add(finalPosition);
				}
				
				//only move the atoms if none of them have hit the walls of the box
				if(atomPositions.Count > 0 && moveAtoms){
					for(int i = 0; i < AllAtoms.Count; i++){
						Vector3 newAtomPosition = atomPositions[i];
						Atom currAtom = AllAtoms[i];
						currAtom.transform.position = newAtomPosition;
						currAtom.position = newAtomPosition;
					}
				}
				
			}
		}
		
		//always keep track of the last mouse position for the next frame for flinging atoms
		lastMousePosition = Input.mousePosition;
		
		
	}
	
	//this function is the equivalent of OnMouseUp for iOS
	void OnMouseUpIOS(){
		StaticVariables.draggingAtoms = false;
		if (!dragCalled) {
			//if the user only tapped the atom, this is executed
			selected = !selected;
			SetSelected(selected);
			GetComponent<Rigidbody>().isKinematic = false;
		}
		else{
			if(!selected){
				//this is for one atom
				GetComponent<Rigidbody>().isKinematic = false;
				
				Quaternion cameraRotation = Camera.main.transform.rotation;
				Vector3 direction = cameraRotation * (new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 0.0f) - new Vector3(lastTouchPosition.x, lastTouchPosition.y, 0.0f));
				float directionMagnitude = direction.magnitude;
				direction.Normalize();
				float magnitude = 2.0f * directionMagnitude;
				Vector3 flingVector = magnitude * new Vector3(direction.x, direction.y, 0.0f);
				this.velocity = flingVector;
			}
			else{
				//this is for a group of atoms
				for(int i = 0; i < AllAtoms.Count; i++){
					Atom currAtom = AllAtoms[i];
					if(currAtom.selected){
						currAtom.GetComponent<Rigidbody>().isKinematic = false;
						currAtom.held = false;
						
						Quaternion cameraRotation = Camera.main.transform.rotation;
						Vector3 direction = cameraRotation * (new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 0.0f) - new Vector3(lastTouchPosition.x, lastTouchPosition.y, 0.0f));
						float directionMagnitude = direction.magnitude;
						direction.Normalize();
						float magnitude = 2.0f * directionMagnitude;
						Vector3 flingVector = magnitude * new Vector3(direction.x, direction.y, 0.0f);
						this.velocity = flingVector;
					}
				}
			}
			
			//reset the selection status of all the atoms
			for(int i = 0; i < AllAtoms.Count; i++){
				Atom currAtom = AllAtoms[i];
				currAtom.SetSelected(currAtom.selected);
			}
			
		}
		held = false;
	}
	
	void OnMouseUp (){
		if(SettingsControl.GamePaused)return;
		StaticVariables.draggingAtoms = false;
		if (Application.isMobilePlatform)return;
		if(!dragCalled){
			//this is executed if an atom is only tapped
			selected = !selected;
			SetSelected(selected);

			GetComponent<Rigidbody>().isKinematic = false;
		}
		else{
			if(!selected){
				//this is for one atom
				GetComponent<Rigidbody>().isKinematic = false;
				
				Quaternion cameraRotation = Camera.main.transform.rotation;
				Vector2 direction = cameraRotation * (Input.mousePosition - lastMousePosition);
				direction.Normalize();
				float magnitude = 10.0f;
				Vector3 flingVector = magnitude * new Vector3(direction.x, direction.y, 0.0f);
				this.velocity = flingVector;
			}else{
				//this is for a group of atoms
				for(int i = 0; i < AllAtoms.Count; i++){
					Atom currAtom = AllAtoms[i];
					if(currAtom.selected){
						currAtom.GetComponent<Rigidbody>().isKinematic = false;
						currAtom.held = false;
						
						Quaternion cameraRotation = Camera.main.transform.rotation;
						Vector3 direction = cameraRotation * (Input.mousePosition - lastMousePosition);
						direction.Normalize();
						float magnitude = 10.0f;
						Vector3 flingVector = magnitude * new Vector3(direction.x, direction.y, 0.0f);
						this.velocity = flingVector;
					}
				}
			}
			
			//reset the selection status of all of the atoms
			for(int i = 0; i < AllAtoms.Count; i++){
				Atom currAtom = AllAtoms[i];
				currAtom.SetSelected(currAtom.selected);
			}
		}
		held = false;
		
	}
	
	//this functions returns the appropriate bond distance, given two atoms
	public float BondDistance(Atom otherAtom){
		//return 1.225f * StaticVariables.sigmaValues [atomID,otherAtom.atomID];
		return 3.0f;
	}
	
	//this function checks the position of an atom, and if its outside of the box, simply place the atom back inside the box
	Vector3 CheckPosition(Vector3 position){
		CreateEnvironment myEnvironment= CreateEnvironment.myEnvironment;
		Vector3 bottomPlanePos = CreateEnvironment.bottomPlane.transform.position;
		if (position.y > bottomPlanePos.y + (myEnvironment.height) - myEnvironment.errorBuffer) {
			position.y = bottomPlanePos.y + (myEnvironment.height) - myEnvironment.errorBuffer;
		}
		if (position.y < bottomPlanePos.y + myEnvironment.errorBuffer) {
			position.y = bottomPlanePos.y + myEnvironment.errorBuffer;
		}
		if (position.x > bottomPlanePos.x + (myEnvironment.width/2.0f) - myEnvironment.errorBuffer) {
			position.x = bottomPlanePos.x + (myEnvironment.width/2.0f) - myEnvironment.errorBuffer;
		}
		if (position.x < bottomPlanePos.x - (myEnvironment.width/2.0f) + myEnvironment.errorBuffer) {
			position.x = bottomPlanePos.x - (myEnvironment.width/2.0f) + myEnvironment.errorBuffer;
		}
		if (position.z > bottomPlanePos.z + (myEnvironment.depth/2.0f) - myEnvironment.errorBuffer) {
			position.z = bottomPlanePos.z + (myEnvironment.depth/2.0f) - myEnvironment.errorBuffer;
		}
		if (position.z < bottomPlanePos.z - (myEnvironment.depth/2.0f) + myEnvironment.errorBuffer) {
			position.z = bottomPlanePos.z - (myEnvironment.depth/2.0f) + myEnvironment.errorBuffer;
		}
		return position;
	}
	
	
	//makes all atoms transparency except for the current atom and all atoms that are "close" to it
	void ApplyTransparency(){
		for (int i = 0; i < AllAtoms.Count; i++) {
			Atom neighborAtom = AllAtoms[i];
			if(neighborAtom.gameObject == gameObject) continue;
			if(neighborAtom.selected){
				neighborAtom.SetSelected(neighborAtom.selected);
			}
			else if(!neighborAtom.selected && Vector3.Distance(gameObject.transform.position, neighborAtom.transform.position) > BondDistance(neighborAtom)){
				neighborAtom.SetTransparent(true);
			}
			else{
				neighborAtom.SetTransparent(false);
			}
		}
	}
	
	//resets all atoms transparency back to normal or to selected depending on its status
	public void ResetTransparency(){
		for (int i = 0; i < AllAtoms.Count; i++) {
			Atom currAtom = AllAtoms[i];
			if(currAtom.selected){
				currAtom.SetSelected(currAtom.selected);
			}
			else{
				currAtom.SetTransparent(false);
			}
		}
	}
	
	
}
