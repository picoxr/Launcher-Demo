//----------------------------------------------
//            Marvelous Techniques
// Copyright © 2015 - Arto Vaarala, Kirnu Interactive
// http://www.kirnuarp.com
//----------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (Transform))]
[ExecuteInEditMode]
public class DirectionalLightManager : MonoBehaviour {

	List<Material> materials = new List<Material>();
	Vector3 lastUpVector = Vector3.zero;
	Vector3 lastRightVector = Vector3.zero;

	void Start () {
		findMaterials();
		updateMaterials ();
	}

	void findMaterials(){
		materials.Clear ();
		Renderer[] arrend = (Renderer[])Resources.FindObjectsOfTypeAll(typeof(Renderer));
		foreach (Renderer rend in arrend) {
			if (rend != null) {
				foreach (Material mat in rend.sharedMaterials) {
					if (mat && !materials.Contains (mat)) {
						if (mat.shader != null) {
							if(mat.shader.name.Contains("Kirnu/Marvelous/") && 
							   mat.shader.name.Contains("CustomLighting")){
								materials.Add(mat);
							}
						}
					}
				}
			}
		}
	}

	void updateMaterials(){
		foreach(Material m in materials){
			if(m && m.HasProperty("_LightDirT") && m.IsKeywordEnabled("USE_DIR_LIGHT")&&(lastUpVector != transform.up || 
				lastRightVector != transform.right)){
				m.SetVector("_LightDirF",transform.forward);
				m.SetVector("_LightDirR",-transform.right);
				m.SetVector("_LightDirT",-transform.up);
			}
		}
		lastUpVector = transform.up;
		lastRightVector  = transform.right;
	}
		
	void Update () {
		updateMaterials ();
	}
}
