﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TemperatureCalc : MonoBehaviour {

	private double kB = 1.381 * Math.Pow(10,-23); // J/K
	public static float squareRootAlpha = 1.0f;
	//public static float desiredTemperature = 0.00001f; //K
	public static float desiredTemperature = 0.001f; //K
	
	void FixedUpdate () {

		GameObject[] allMolecules = GameObject.FindGameObjectsWithTag("Molecule");
		float totalEnergy = 0.0f;
		for (int i = 0; i < allMolecules.Length; i++) {
			//compute the total energy in the system
			GameObject molecule = allMolecules[i];
			if(molecule.rigidbody && !molecule.rigidbody.isKinematic){
				//mass is hardcoded
				double mass = 3.27;
				//double mass = molecule.rigidbody.mass;
				//print ("velocity: " + molecule.rigidbody.velocity.magnitude);
				double velocitySquared = Math.Pow((molecule.rigidbody.velocity.magnitude), 2);
				totalEnergy += (float)(.5f * (mass) * velocitySquared);
			}
		}

		double adjustedkB = kB / (1.6605f * (float)Math.Pow (10, -25));
		double instantTemp = totalEnergy / (3.0f / 2.0f) / allMolecules.Length / adjustedkB;
		double alpha = desiredTemperature / instantTemp;
		squareRootAlpha = (float)Math.Pow (alpha, .5f);
		//print ("squareRootAlpha: " + squareRootAlpha);
	}
}