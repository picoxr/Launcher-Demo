//----------------------------------------------
//            Monumental Techniques
// Copyright © 2016 - Arto Vaarala, Kirnu Interactive
// http://www.kirnuarp.com
//----------------------------------------------
using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(UVHeightGenerator))]
public class UVHeightGeneratorEditor : Editor {

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		
		UVHeightGenerator myScript = (UVHeightGenerator)target;
		if(GUILayout.Button("Generate Height UVs"))
		{
			myScript.GenerateUVs();
		}
	}
}
