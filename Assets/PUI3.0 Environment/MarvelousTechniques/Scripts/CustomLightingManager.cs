using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class CustomLightingManager : MonoBehaviour {

	public Color ambientColor = Color.white;
	public float ambientPower = 0;
	public Color tintColor = Color.white;
	public Color rimColor = Color.black;
	public float rimPower = 0;
	public Color lightmapColor = Color.black;
	public float lightmapPower = 0;
	public bool lightmapEnabled = false;
	public float lightmapLight = 0;

	Color lastAmbientColor = Color.clear;
	float lastAmbientPower = -1;
	Color lastTintColor = Color.clear;
	Color lastRimColor = Color.clear;
	float lastRimPower = -1;
	Color lastLightmapColor = Color.clear;
	float lastLightmapPower = -1;
	bool lastLightmapEnabled = true;
	float lastLightmapLight = -1;

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
			if(m){
				if (lastAmbientPower != ambientPower) {
					m.SetFloat ("_AmbientPower",ambientPower);
				}
				if (lastAmbientColor != ambientColor) {
					m.SetColor ("_AmbientColor",ambientColor);
				}
				if (lastTintColor != tintColor) {
					m.SetColor ("_LightTint",tintColor);
				}
				if (lastRimPower != rimPower) {
					m.SetFloat ("_RimPower",rimPower);
				}
				if (lastRimColor != rimColor) {
					m.SetColor ("_RimColor",rimColor);
				}
				if (lastLightmapPower != lightmapPower) {
					m.SetFloat ("_LightmapPower",lightmapPower);
				}
				if (lastLightmapColor != lightmapColor) {
					m.SetColor ("_LightmapColor",lightmapColor);
				}
				if (lastLightmapEnabled != lightmapEnabled) {
					if (lightmapEnabled) {
						m.EnableKeyword ("LIGHTMAP");
					} else {
						m.DisableKeyword ("LIGHTMAP");
					}
				}
				if (lastLightmapLight != lightmapLight) {
					m.SetFloat ("_ShadowPower",lightmapLight);
				}

			}
		}
		lastAmbientPower = ambientPower;
		lastAmbientColor  = ambientColor;
		lastTintColor = tintColor;
		lastRimPower = rimPower;
		lastRimColor = rimColor;
		lastLightmapEnabled = lightmapEnabled;
		lastLightmapLight = lightmapLight;
	}

	void Update () {
		updateMaterials ();
	}
}
