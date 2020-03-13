//----------------------------------------------
//            Marvelous Techniques
// Copyright © 2016 - Arto Vaarala, Kirnu Interactive
// http://www.kirnuarp.com
//----------------------------------------------
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class UVHeightGenerator : MonoBehaviour {

	public bool makeAllMeshesUnique = false;

	public void GenerateUVs () {
		calculateUVs(transform, gameObject);
		foreach (Transform child in transform) {
			GameObject go = child.gameObject;
			calculateUVs(child, go);		
		}
	}

	private void calculateUVs(Transform t,GameObject go){
		MeshFilter mf = go.GetComponent<MeshFilter> ();
		if (mf == null) {
			return;
		}

		Mesh mesh = mf.sharedMesh;    
		if (mesh == null) {
			return;
		}
		if (makeAllMeshesUnique) {
			mf.sharedMesh = (Mesh)Instantiate (mf.sharedMesh);
			mesh = mf.sharedMesh;
		}
		Vector2[] uvs = new Vector2[mesh.vertices.Length];
		float minYPos = float.MaxValue;
		float maxYPos = float.MinValue;
		for (int triangle = 0; triangle < mesh.triangles.Length/3; triangle++) {			
			for (int i=0; i<3; i++) {				
				int vertexIndex = mesh.triangles [triangle * 3 + i];
				float yPos;
				if (t != null) {
					yPos = t.TransformPoint (mesh.vertices [vertexIndex]).y;
				} else {
					yPos = mesh.vertices [vertexIndex].y;
				}
				if (yPos < minYPos) {
					minYPos = yPos;
				}
				else if (yPos > maxYPos) {
					maxYPos = yPos;
				}
			}
		}
		for (int triangle = 0; triangle < mesh.triangles.Length / 3; triangle++) {			
			for (int i = 0; i < 3; i++) {
				Vector3 pos = Vector3.zero;
				int vertexIndex = mesh.triangles [triangle * 3 + i];

				if (t != null) {
					pos = t.TransformPoint (mesh.vertices [vertexIndex]);
				} else {
					pos = mesh.vertices [vertexIndex];
				}	

				uvs [vertexIndex].y = (pos.y - minYPos) / Mathf.Abs (maxYPos-minYPos);

			}
		}
		mesh.uv3 = uvs;
	}

	public static Vector3 Vabs(Vector3 a) {
		return new Vector3(Mathf.Abs(a.x), Mathf.Abs(a.y), Mathf.Abs(a.z));
	}
}
