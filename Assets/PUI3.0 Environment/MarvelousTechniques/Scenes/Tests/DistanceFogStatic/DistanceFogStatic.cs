using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class DistanceFogStatic : MonoBehaviour {

	List<Material> materials = new List<Material>();

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
						if (mat.name.Contains("DistanceFogStatic")) {
							materials.Add(mat);
						}
					}
				}
			}
		}
	}

	void updateMaterials(){
		foreach(Material m in materials){
			if(m){
				m.SetVector ("_FogStaticStartPos",new Vector4(transform.position.x,transform.position.y,transform.position.z,0));
			}
		}

	}

	void Update () {
		updateMaterials ();
	}
}
