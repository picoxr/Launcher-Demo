using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Test : MonoBehaviour
{

	public Image maskImage;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		#if UNITY_EDITOR
		if (Input.GetKeyDown (KeyCode.A)) {
			Dialog.Show(Localization.Get("Update_Title"),Localization.Get("Dialog_Ok"),Localization.Get("Dialog_Cancel"),()=> {
			},()=> {
			});
		}
		if (Input.GetKeyDown (KeyCode.B)) {
			Dialog.Show ("bbbbbbbbbbbbbbbbbbbbbbbbbbbbbb", "ok",() => {
			});
		}
		if (Input.GetKeyDown (KeyCode.C)) {
			ToastController.Show (false);
		}
		if (Input.GetKeyDown (KeyCode.U)) {
			UpdateDialog.Show ("");
		}
		if (Input.GetKeyDown (KeyCode.T)) {
			Localization.language = "en";
			TipDialog.Show();
		}
		if (Input.GetKeyDown (KeyCode.V)) {
			VoiceDialog.Show();
		}
		if (Input.GetKeyDown (KeyCode.M)) {
			maskImage.color = Color.black;
		}
		if(Input.GetKeyDown(KeyCode.O)){
			Toast.Show("This is a toast.");
		}
		#endif
	}
		
}
