using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherUtils : MonoBehaviour {

	private static OtherUtils instance;
	public static OtherUtils Instance
	{
		get{
			if (instance == null) {
				instance = FindObjectOfType<OtherUtils> ();
			}
			if (instance == null) {
				GameObject obj = new GameObject ("OtherUtils");
				instance = obj.AddComponent<OtherUtils> ();
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
			javaObj = new AndroidJavaObject("com.picovr.manager.OtherManager");
		}
		return javaObj;
	}

	/**
     * 获取当前时间
     * @return xx:xx
     */
	public string GetCurrentTime() {
		if (Application.platform == RuntimePlatform.Android) {
			return GetJavaObject ().Call<string> ("getCurrentTime");
		} else {
			return "00:00";
		}
	}

	/**
     * 是否有网络连接
     * @return
     */
	public bool GetNetStatus() {
		if (Application.platform == RuntimePlatform.Android) {
			return GetJavaObject ().Call<bool> ("getNetStatus");
		} else {
			return true;
		}
	}

	/**
     * 获取WiFi信号强弱
     * @return 0-4
     */
	public int GetWifiLevel() {
		if (Application.platform == RuntimePlatform.Android) {
			return GetJavaObject ().Call<int> ("getWifiLevel");
		} else {
			return 0;
		}
	}

	/**
     * 获取WiFi开关状态
     * @return 0,1,2,3,4
     */
	public int GetWifiSwitchState(){
		if (Application.platform == RuntimePlatform.Android) {
			return GetJavaObject ().Call<int> ("getWifiSwitchState");
		} else {
			return 0;
		}
	}

	/**
     * 获取网络连接状态
     * @return 1连接 0未连接
     */
	public int GetNetworkConnectionState(){
		if (Application.platform == RuntimePlatform.Android) {
			return GetJavaObject ().Call<int> ("getWifiConnectionState");
		} else {
			return 1;
		}
	}

	/**
     * 获取蓝牙开关状态
     * @return 10,11,12,13
     */
	public int GetBluetoothStatus() {
		if (Application.platform == RuntimePlatform.Android) {
			return GetJavaObject ().Call<int> ("getBluetoothStatus");
		} else {
			return 10;
		}
	}

	/**
     * 获取系统当前音量
     * @return
     */
	public int GetVolume() {
		if (Application.platform == RuntimePlatform.Android) {
			return GetJavaObject ().Call<int> ("getVolume");
		} else {
			return 1;
		}
	}

	/**
     * 获取当前语言
     * @return
     */
	public string GetLanguage() {
		if (Application.platform == RuntimePlatform.Android) {
			return GetJavaObject ().Call<string> ("getLanguage");
		} else {
			return "zh";
		}
	}

	/**
     * 获取地区
     * @return
     */
	public string GetCountry(){
		if (Application.platform == RuntimePlatform.Android) {
			return GetJavaObject ().Call<string> ("getCountry");
		} else {
			return "zh";
		}
	}

	/**
     * 获取国家码
     * @return CN US JP ...
     */
	public string GetCountryCode(){
		if (Application.platform == RuntimePlatform.Android) {
			return GetJavaObject ().Call<string> ("getCountryCode");
		} else {
			return "CN";
		}
	}

	/**
     * 获取版本名称
     * @return
     */
	public string GetVersionName(){
		if (Application.platform == RuntimePlatform.Android) {
			return GetJavaObject ().Call<string> ("GetVersionName");
		} else {
			return "1.0";
		}
	}

	/**
     * 获取版本号
     * @return
     */
	public int GetVersionCode(){
		if (Application.platform == RuntimePlatform.Android) {
			return GetJavaObject ().Call<int> ("getVersionCode");
		} else {
			return 1;
		}
	}

	/**
     * Launcher的服务器地址
     * @return
     */
	public string GetBaseUrl(){
		if (Application.platform == RuntimePlatform.Android) {
			return GetJavaObject ().Call<string> ("getBaseUrl");
		} else {
			return Constant.BASE_URL;
		}
	}

	/**
     * 往sp文件中写入数据
     * @return
     */
	public bool PutIntDataToSp(string key, int value){
		return GetJavaObject ().Call<bool> ("putIntDataToSp", key, value);
	}

	/**
     * 从sp文件中读取数据
     * @return
     */
	public int GetIntDataFromSp(string key, int defValue) {
		return GetJavaObject ().Call<int> ("getIntDataFromSp", key, defValue);
	}

	/**
     * 获取用户中心的用户信息
     * @return
     */
	public string GetUserInfo(){
		return GetJavaObject ().Call<string> ("getUserInfo");
	}

	/**
     * 获取用户头像
     * @return
     */
	public byte[] GetUserAvatar(){
		return GetJavaObject ().Call<byte[]> ("getUserAvatar");
	}

	/**
     * getSystemProperties
     * @param key
     * @param defaultValue
     * @return
     */
	public string GetSystemProperties(string key, string defaultValue) {
		return GetJavaObject ().Call<string> ("getSystemProperties" ,key, defaultValue);
	}

	/**
     * 是否有外置sdCard
     * @return
     */
	public bool HasExternalSdcard(){
		return GetJavaObject ().Call<bool> ("hasExternalSdcard");
	}

	/**
     * 外置sdCard的路径
     * @return
     */
	public string GetExternalSdcardPath() {
		return GetJavaObject ().Call<string> ("getExternalSdcardPath");
	}

	/*
	 * 发送launcher启动的广播
	 */
	public void SendLaunchBroadcast(){
		LogUtils.Log ("发送launcher启动可见的广播");
		GetJavaObject ().Call ("sendLaunchBroadcast");
	}

	/*
	 * 获取设备电量 1-100
	 */
	public int GetBattery(){
		return GetJavaObject ().Call<int> ("getBattery");
	}

	/**
     * 是否有系统更新
     * @return 0：无   1：有
     */
	public int HasSystemUpdate(){
		return GetJavaObject ().Call<int> ("hasSystemUpdate");
	}

	/**
     * 是否有app更新
     * @return 0：无   1：有
     */
	public int HasAppUpdate(){
		return GetJavaObject ().Call<int> ("hasAppUpdate");
	}

	/*
	 * resume时，延迟2s，有app升级信息时会打开版本管理
	 */
	public void OpenVersionManager(){
		LogUtils.Log ("延迟2s，检查是否可以打开版本管理");
		GetJavaObject ().Call ("openVersionManager");
	}

	/*
	 * 启动版本管理
	 */
	public void StartVersionManager(){
		GetJavaObject ().Call ("startVersionManager");
	}

}
