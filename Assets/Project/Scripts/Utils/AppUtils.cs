using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppUtils : MonoBehaviour {

	private static AppUtils instance;
	public static AppUtils Instance
	{
		get{
			if (instance == null) {
				instance = FindObjectOfType<AppUtils> ();
			}
			if (instance == null) {
				GameObject obj = new GameObject ("AppUtils");
				instance = obj.AddComponent<AppUtils> ();
				DontDestroyOnLoad (obj);
			}
			return instance;
		}
	}

	void Awake(){
		if (Application.platform == RuntimePlatform.Android) {
			Call_SetUnityActivity ();
		}
	}

	private void Call_SetUnityActivity(){
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		GetJavaObject().Call("setUnityActivity", jo);
	}

	private AndroidJavaObject javaObj = null;
	private AndroidJavaObject GetJavaObject(){
		if (javaObj == null){
			javaObj = new AndroidJavaObject("com.picovr.manager.AppManager");
		}
		return javaObj;
	}


	/**
     * 根据包名打开app
     * @param packageName
     */
	public void OpenAppByPackageName(string packageName){
		if (Application.platform == RuntimePlatform.Android) {
			LogUtils.Log ("OpenAppByPackageName : " + packageName);
			GetJavaObject ().Call ("openAppByPackageName", packageName);
		}
	}

	/**
     * 根据包名、类名打开app
     * @param packageName
     * @param activityName
     */
	public void OpenAppByComponentName(string packageName, string activityName) {
		if (Application.platform == RuntimePlatform.Android) {
			LogUtils.Log ("OpenAppByComponentName : " + packageName + ", " + activityName);
			GetJavaObject ().Call ("openAppByComponentName", packageName, activityName);
		}
	}

	/**
     * 根据action打开app
     * @param action
     */
	public void OpenAppByAction(string action){
		if (Application.platform == RuntimePlatform.Android) {
			LogUtils.Log ("OpenAppByAction : " + action);
			GetJavaObject ().Call ("openAppByAction", action);
		}
	}

	/**
     * 检查app是否安装
     * @param packageName
     * @return
     */
	public bool IsInstall(string packageName) {
		if (Application.platform == RuntimePlatform.Android) {
			bool result = GetJavaObject ().Call<bool> ("isInstall", packageName);
			if (result) {
				return IsEnabled (packageName);
			} else {
				return false; 
			}
		} else {
			return false;
		}
	}

	/**
     * 检查app是否enabled
     * @param packageName
     * @return
     */
	public bool IsEnabled(string packageName) {
		return GetJavaObject ().Call<bool> ("isEnabled", packageName);
	}

	/**
     * 打开浏览器，并启动相应URL界面
     * @param url
     */
	public void OpenBrowserByUrl(string url) {
		if (Application.platform == RuntimePlatform.Android) {
			GetJavaObject ().Call ("openBrowserByUrl", url);
		}
	}

	/**
     * 打开X5浏览器，并启动相应URL界面
     * @param url
     */
	public void OpenBrowserByUrlByX5(string url) {
		if (Application.platform == RuntimePlatform.Android) {
			GetJavaObject ().Call ("openBrowserByUrlByX5", url);
		}
	}

	/**
     * 打开store app详情页
     * @param mid
     * @param packageName
     */
	public void StartStoreAppDetail(string mid,string packageName) {
		if (Application.platform == RuntimePlatform.Android) {
			GetJavaObject ().Call ("startStoreAppDetail", mid, packageName);
		}
	}

	/**
     * 打开store分类
     * @param firstId：一级分类id
     * @param secondId：二级分类id
     */
	public void StartStoreCategory(int firstId,int secondId) {
		if (Application.platform == RuntimePlatform.Android) {
			GetJavaObject ().Call ("startStoreCategory", firstId, secondId);
		}
	}

	/**
     * 打开全景图片播放器
     */
	public void StartGalleryPalyer(string mid,string title,string small,string large){
		if (Application.platform == RuntimePlatform.Android) {
			GetJavaObject ().Call ("startGalleryPalyer", mid, title, small, large);
		}
	}

	/**
     * 打开Gallery某个分类
     */
	public void StartGalleryCategory(string mid){
		if (Application.platform == RuntimePlatform.Android) {
			GetJavaObject ().Call ("startGalleryCategory", mid);
		}
	}

	/**
     * 打开Gallery某个专题
     */
	public void StartGalleryTheme(string mid){
		if (Application.platform == RuntimePlatform.Android) {
			GetJavaObject ().Call ("startGalleryTheme", mid);
		}
	}

	/**
     * 打开VRWing的某个分类
     * public static final int SELECTED = 1; // 精选
     * public static final int SPECIAL = 2; // 专题
     * public static final int C_3D = 3; // 3D
     * public static final int C_360 = 4; // 360
     * public static final int C_2D = 5; // 2D
     * public static final int LOCAL = 6; // 本地
     * public static final int CACHE = 7; // 缓存
     * public static final int FAVORITE = 8; // 搜藏
     * public static final int HISTORY = 9; // 历史
     * public static final int USER_CENTER = 10; // 我的
     * 注：后台数据 3D=1、360=5、2D=13
     */
	public void StartVRWingCategory(string firstHierarchy,string secondHierarchy){
		if (Application.platform == RuntimePlatform.Android) {
			GetJavaObject ().Call ("startVRWingCategory", firstHierarchy, secondHierarchy);
		}
	}

	/**
     * 打开VRWing的某个专题，参考startVRWingCategory()方法
     * @param mid
     */
	public void StartVRWingTheme(string mid){
		if (Application.platform == RuntimePlatform.Android) {
			GetJavaObject ().Call ("startVRWingTheme", mid);
		}
	}

	/**
     * 打开播放器
     * @param path
     * @param mid
     * @param itemId
     * @param videoType
     * @param title
     */
	public void StartVideo(string path, string mid, string itemId,
		string videoType, string title) {
		if (Application.platform == RuntimePlatform.Android) {
			GetJavaObject ().Call ("startVideo", path, mid, itemId, videoType, title);
		}
	}

        /**
     * 通用版launcher打开播放器
     * @param path
     * @param mid
     * @param itemId
     * @param videoType
     * @param title
     * @param key
     */
    public void PlayVideo(string path, string mid, string itemId,
        string videoType, string title,int key)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            GetJavaObject().Call("playVideo", path, mid, itemId, videoType, title, key);
        }
    }

    /**
     * 打开vr设置
     * @param category
     */
    public void StartSubSettings(string category) {
		if (Application.platform == RuntimePlatform.Android) {
			GetJavaObject ().Call ("startSubSettings", category);
		}
	}

	/**
     * 强制升级对话框点击“立即升级”按钮调用该方法
     */
	public void SendBroadcastReboot() {
		GetJavaObject ().Call ("sendBroadcastReboot");
	}
		
}
