/**
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

		Vector3 c1;
		Vector3 c2;
		Vector3 camPos;
		Vector3 c2ToCam;
		Vector3 c2ToS3;
		Vector3 s1;
		Vector3 s2;
		Vector3 s3;
		Vector3 s4;
		float lineWidth = 0.05f;

		mat.SetPass (0);

		int numberLinesDrawn = 0;
		int maxLinesDrawn = (int)Time.realtimeSinceStartup;


	




		if (StaticVariables.drawBondLines) {

			GL.LoadProjectionMatrix (Camera.main.projectionMatrix);
			GL.PushMatrix();
			//mat.SetPass (0);
			GL.Begin (GL.QUADS);
			
			GL.Color (bondColor);
		
			for (int i = 0; i < Atom.AllAtoms.Count; i++) {
				for(int j = i + 1; j < Atom.AllAtoms.Count; j++){
					Atom currAtom = Atom.AllAtoms[i];
					Atom neighborAtom = Atom.AllAtoms[j];
					if((currAtom.transform.position - neighborAtom.transform.position).magnitude 
						< currAtom.BondDistance(neighborAtom)){
						//draw a line from currAtom to atomNeighbor
						//if(bondColor == null)bondColor = Color.clear;

							c1 = currAtom.transform.position;
							c2 = neighborAtom.transform.position;

							camPos = Camera.main.gameObject.transform.position;

							c2ToCam = camPos-c2;
							c2ToS3 = Vector3.Cross(c1-c2, c2ToCam);
							c2ToS3.Normalize();
							s1 = c1 - c2ToS3*lineWidth/2.0f;
							s2 = c2 - c2ToS3*lineWidth/2.0f;
							s3 = c2 + c2ToS3 * lineWidth/2.0f; 
							s4 = s3 + (s1-s2);

							GL.Vertex (s1);
							GL.Vertex (s2);
							GL.Vertex (s3);
							GL.Vertex (s4);

					}
				}
			}
			GL.End ();
			GL.PopMatrix();
		}
	}


}
