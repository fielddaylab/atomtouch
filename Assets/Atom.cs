using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//L-J potentials from Zhen and Davies, Phys. Stat. Sol. a, 78, 595 (1983)
//Symbol, epsilon/k_Boltzmann (K) n-m version, 12-6 version, sigma (Angstroms),
//     mass in amu, mass in (20 amu) for Unity 
//     FCC lattice parameter in Angstroms, expected NN bond (Angs)
//Au: 4683.0, 5152.9, 2.6367, 196.967, 9.848, 4.080, 2.88
//Cu: 3401.1, 4733.5, 2.3374,  63.546, 3.177, 3.610, 2.55
//Pt: 7184.2, 7908.7, 2.5394, 165.084, 8.254, 3.920, 2.77

public abstract class Atom : MonoBehaviour
{
	private List<GameObject> molecules;
	private Vector3 offset;
	private Vector3 screenPoint;
	private Vector3 lastMousePosition;
	private Vector3 mouseDelta;
	public bool held { get; set; }

	public static float timeScale = 1.0f;
	
	private float cutoff = 10; //mutliplier for cutoff

	//variables that must be implemented because they are declared as abstract in the base class
	protected abstract float epsilon{ get; } // J
	protected abstract float sigma{ get; } // m=Angstroms for Unity
	protected abstract float massamu{ get; } //amu

	private GameObject moleculeToMove = null;
	private Vector2 prevTouchPosition = new Vector2(0.0f, 0.0f);
	private float deltaTouch2 = 0.0f;
	private bool moveZDirection = false;
	private bool reflecting = false;
	private Vector3 reflectingVelocity;
	private float radius = 15.0f;
	private float lastTapTime;
	[HideInInspector]public bool doubleTapped = false;

	void FixedUpdate(){
		Time.timeScale = timeScale;
		GameObject[] allMolecules = GameObject.FindGameObjectsWithTag("Molecule");
		molecules = new List<GameObject>();
		float totalEnergy = 0.0f;

		for(int i = 0; i < allMolecules.Length; i++){
			double distance = Vector3.Distance(transform.position, allMolecules[i].transform.position);
			if(allMolecules[i] != gameObject && distance < (cutoff * sigma)){
				molecules.Add(allMolecules[i]);
			}
		}


		double finalMagnitude = 0;
		Vector3 finalForce = new Vector3 (0.000f, 0.000f, 0.000f);
		for (int i = 0; i < molecules.Count; i++) {
			//Vector3 vect = molecules[i].transform.position - transform.position;
			Vector3 direction = new Vector3(molecules[i].transform.position.x - transform.position.x, molecules[i].transform.position.y - transform.position.y, molecules[i].transform.position.z - transform.position.z);
			direction.Normalize();
			
			double distance = Vector3.Distance(transform.position, molecules[i].transform.position);
			//print ("distance: " + distance);
			double part1 = ((-48 * epsilon) / Math.Pow(distance, 2));
			//print ("part1: " + part1);
			double part2 = (Math.Pow ((sigma / distance), 12) - (.5f * Math.Pow ((sigma / distance), 6)));
			//print ("part2: " + part2);
			double magnitude = (part1 * part2 * distance);
			finalForce += (direction * (float)magnitude);
			finalMagnitude += magnitude;
		}

		Vector3 adjustedForce = finalForce / (1.6605f * (float)(Math.Pow (10, -25))); //adjust mass input
		adjustedForce = adjustedForce * (float)(Math.Pow (10, -10)); //normalize back Angstroms = m from extra r_ij denomintor term

		rigidbody.AddForce (adjustedForce);

		//adjust velocity for the desired temperature of the system
		//if (Time.time > 10.0f) {
			Vector3 newVelocity = gameObject.rigidbody.velocity * TemperatureCalc.squareRootAlpha;
		if (!rigidbody.isKinematic && !float.IsInfinity(TemperatureCalc.squareRootAlpha) && allMolecules.Length > 1) {
				rigidbody.velocity = newVelocity;
			}
		//}

		BoundingSphere ();

	}

	void Update(){
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			HandleTouch ();
		}
		if (doubleTapped) {
			CameraScript cameraScript = Camera.main.GetComponent<CameraScript>();
			cameraScript.setCameraCoordinates(transform);
		}
	}

	void BoundingSphere(){
		CameraScript cameraScript = Camera.main.GetComponent<CameraScript> ();
		if (Vector3.Distance (cameraScript.centerPos, transform.position) > radius) {
			if(reflecting){
				//print ("Ball is most likely stuck");
				rigidbody.velocity = reflectingVelocity;
			}
			else{
				reflecting = true;
				reflectingVelocity = Vector3.Reflect(rigidbody.velocity, (cameraScript.centerPos - transform.position).normalized);
				rigidbody.velocity = reflectingVelocity;
				//Vector3 f = new Vector3(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value) * 20.0f;
				//rigidbody.AddForce(f);
			}
		}
		else{
			reflecting = false;
		}
	}

	//controls for touch devices
	void HandleTouch(){
		if (Input.touchCount == 1) {
			HandleMovingAtom();
		}
		else if(Input.touchCount == 2){

			Touch touch2 = Input.GetTouch(1);
			if(touch2.phase == TouchPhase.Began){
				moveZDirection = true;
				//moves the atom to the center of the screen (0, 0)
//				if(moleculeToMove != null){
//					Quaternion cameraRotation = Camera.main.transform.rotation;
//					moleculeToMove.transform.position = (cameraRotation * new Vector3(0.0f, 0.0f, moleculeToMove.transform.position.z));
//				}
			}
			else if(touch2.phase == TouchPhase.Moved){
				Vector2 touchOnePrevPos = touch2.position - touch2.deltaPosition;
				float deltaMagnitudeDiff = touch2.position.y - touchOnePrevPos.y;
				deltaTouch2 = deltaMagnitudeDiff / 10.0f;
				CameraScript cameraScript = Camera.main.GetComponent<CameraScript>();
				if(moleculeToMove != null){
					Quaternion cameraRotation = Camera.main.transform.rotation;

					//forcast what the z-direction will be
					Vector3 forcastPosition = moleculeToMove.transform.position;
					forcastPosition += (cameraRotation * new Vector3(0.0f, 0.0f, deltaTouch2));
					if (Vector3.Distance (cameraScript.centerPos, forcastPosition) < radius){
						moleculeToMove.transform.position += (cameraRotation * new Vector3(0.0f, 0.0f, deltaTouch2));
						screenPoint += new Vector3(0.0f, 0.0f, deltaTouch2);
					}
				}
			}
		}
		else if(Input.touchCount == 0 && moveZDirection){
			moveZDirection = false;
			moleculeToMove = null;
			held = false;
		}
	}

	void ResetDoubleTapped(){
		GameObject[] allMolecules = GameObject.FindGameObjectsWithTag("Molecule");
		for (int i = 0; i < allMolecules.Length; i++) {
			Atom atomScript = allMolecules[i].GetComponent<Atom>();
			atomScript.doubleTapped = false;
		}
	}

	void HandleMovingAtom(){
		Touch touch = Input.GetTouch(0);
		
		if(touch.phase == TouchPhase.Began){
			Ray ray = Camera.main.ScreenPointToRay( Input.touches[0].position );
			RaycastHit hitInfo;
			if (Physics.Raycast( ray, out hitInfo ) && hitInfo.transform.gameObject.tag == "Molecule" && hitInfo.transform.gameObject == gameObject)
			{
				if((Time.time - lastTapTime) < 1.0f){
					ResetDoubleTapped();
					doubleTapped = true;
				}
				moleculeToMove = gameObject;
				screenPoint = Camera.main.WorldToScreenPoint(transform.position);
				offset = moleculeToMove.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y - 50, screenPoint.z));
				held = true;
				rigidbody.isKinematic = true;
				lastTapTime = Time.time;
			}
		}
		else if(touch.phase == TouchPhase.Moved){
			if(moleculeToMove != null && !doubleTapped){
				Vector3 curScreenPoint = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, screenPoint.z);
				Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
				mouseDelta = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 0.0f) - lastMousePosition;
				lastMousePosition = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 0.0f);
				moleculeToMove.transform.position = curPosition;
			}
		}
		else if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled){
			if(moleculeToMove != null){
				moleculeToMove = null;
				Quaternion cameraRotation = Camera.main.transform.rotation;
				rigidbody.isKinematic = false;
				rigidbody.AddForce (cameraRotation * mouseDelta * 50.0f);
				held = false;
			}
		}
	}


	//controls for debugging on pc
	void OnMouseDown (){
		if (Application.platform != RuntimePlatform.IPhonePlayer) {
			rigidbody.isKinematic = true;
			
			screenPoint = Camera.main.WorldToScreenPoint(transform.position);
			offset = transform.position - Camera.main.ScreenToWorldPoint(
				new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
			held = true;
		}
	}

	void OnMouseDrag(){
		if (Application.platform != RuntimePlatform.IPhonePlayer) {
			Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
			CameraScript cameraScript = Camera.main.GetComponent<CameraScript> ();
			transform.position = curPosition;
			mouseDelta = Input.mousePosition - lastMousePosition;
			lastMousePosition = Input.mousePosition;
		}

	}

	void OnMouseUp (){
		if (Application.platform != RuntimePlatform.IPhonePlayer) {
			Quaternion cameraRotation = Camera.main.transform.rotation;
			rigidbody.isKinematic = false;
			rigidbody.AddForce (cameraRotation * mouseDelta * 50.0f);
			held = false;
		}
	}
}
