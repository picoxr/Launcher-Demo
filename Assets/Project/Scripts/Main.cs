using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {

	// Use this for initialization
	void Start () {
		LogUtils.Log ("===================================== VRLauncher Unity Start =====================================");
		//设置语言
		string language = OtherUtils.Instance.GetLanguage();
		string country = OtherUtils.Instance.GetCountry ();
		LogUtils.Log (language+"<===>"+country);
		if (country == "TW") {
			Localization.language = language + "-" + country;
		} else {
			Localization.language = language;
		}
		//设置图片缓存路径
		LauncherUtils.SetImageCachePath(Application.persistentDataPath + "/ImageCache/");
		//初始化数据库
		//DBUtils.Instance.Init ();
		//初始化场景
		//ScreenManager.Instance.InitScreen();
		//bind sdk service
		if (Application.platform == RuntimePlatform.Android) {
			StartCoroutine (StartBind ());
		}
	}
	
	// Update is called once per frame
//	void Update () {
//		
//	}

	/*
	 * 清除最近使用
	 */
	public void RemoveTasks()
	{
		LogUtils.Log ("RemoveTasks");
		PicoUnityActivity.CallObjectMethod ("removeAppTasks");
	}

	IEnumerator StartBind(){
		yield return new WaitForSeconds(1.0f);
		LogUtils.Log ("-----------------BindService");
		Pvr_ControllerManager.Instance.BindService ();
	}

}
