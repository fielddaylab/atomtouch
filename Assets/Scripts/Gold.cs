/**
 * Class: Gold.cs
 * Created by: Justin Moeller
 * Description: This class defines anything that is gold specific, and NOT related to all of
 * the atoms. This class is derived from the base class of Atom.cs, and takes on all of its behavior.
 * It must override all of the abstract variables and functions that are defined in Atom.cs, such as
 * atomName, epsilon, sigma, massamu, SetSelected(), and SetTransparent().
 * 
 * 
 **/


using UnityEngine;
using System.Collections;
using System;

public class Gold : Atom
{
	/*
	public Material goldMaterial;
	public Material selectedMaterial;
	public Material transparentMaterial;
	*/
	public static int count = 0;
	
	public override String atomName { 
		get{ return "Gold"; } 
	}
	
	
	public override int atomID {
		get{ return 1;}
	}
	
	public override float epsilon {
		get { return 5152.9f * 1.381f * (float)Math.Pow (10, -23); } // J
	}
	
	public override float sigma {
		get { return 2.6367f; }
	}
	
	public override float massamu {
		get { return 196.967f; } //amu for Gold
		//get { return 35.453f; } //amu for Chlorine
	}
	
	// We assume gold to play the role of chloride
	public override float buck_A {
		get { return 405774.0f*1.6f*Mathf.Pow(10,-19); } //units of [J]
	}
	
	public override float buck_B {
		get { return 4.207408f; } //units of [1/Angstrom]
	}
	
	public override float buck_C {
		get { return 72.4f*1.6f*Mathf.Pow(10,-19); } //units of [J.Anstrom^6]
	}
	
	public override float buck_D {
		get { return 145.425f*1.6f*Mathf.Pow(10,-19); } //units of [J.Angstrom^8]
	}
	
	public override float Q_eff {
		get { return -0.7f*1.6f*Mathf.Pow(10,-19); } //units of Coulomb
	}
	/*
	public override void SetSelected (bool selected){
		if (selected) {
			gameObject.GetComponent<Renderer>().material = selectedMaterial;
		}
		else{
			gameObject.GetComponent<Renderer>().material = goldMaterial;
		}
		//Atom.EnableSelectAtomGroup(NumberofAtom.selectedAtoms>0);
//		Debug.Log("selected atoms: " + NumberofAtom.selectedAtoms);
	}
	
	public override void SetTransparent(bool transparent){
		if (transparent) {
			gameObject.GetComponent<Renderer>().material = transparentMaterial;
		}
		else{
			gameObject.GetComponent<Renderer>().material = goldMaterial;
		}
	}
	*/
	void Start ()
	{
		//make the atom its original color to start
		SetSelected (false);
		//scale the atom according to sigma
		//gameObject.transform.localScale = new Vector3(sigma * .5f, sigma * .5f, sigma * .5f);
		gameObject.transform.localScale = new Vector3(1.35f, 1.35f, 1.35f);
	}
}

