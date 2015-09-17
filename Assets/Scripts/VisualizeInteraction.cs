﻿/**
 * Class: VisualizeInteraction.cs
 * Created by: Justin Moeller
 * Description: This class draws the lines between the atoms. Because it only needs to draw lines between 
 * every pair of atoms it only iterates through each distinct pair rather than every possible pair. (This 
 * reduces the time spent from an O(N^2) to (1/2)O(N^2)). The lines are only drawn if the variable in StaticVariables
 * drawBondLines is true. This variable is controlled from the user interface.
 * 
 * 
 * 
 **/


using UnityEngine;
using System.Collections;

public class VisualizeInteraction : MonoBehaviour {
	
	public Material mat;
	public Color bondColor;
	void OnPostRender(){

		if (StaticVariables.drawBondLines) {
			for (int i = 0; i < Atom.AllAtoms.Count; i++) {
				for(int j = i + 1; j < Atom.AllAtoms.Count; j++){
					Atom currAtom = Atom.AllAtoms[i];
					Atom neighborAtom = Atom.AllAtoms[j];
					if((currAtom.transform.position - neighborAtom.transform.position).magnitude 
						< currAtom.BondDistance(neighborAtom)){
						//draw a line from currAtom to atomNeighbor
						//if(bondColor == null)bondColor = Color.clear;
						StaticVariables.DrawLine (currAtom.transform.position, 
							neighborAtom.transform.position, bondColor, bondColor, 0.05f, mat);
					}
				}
			}
		}
	}


}
