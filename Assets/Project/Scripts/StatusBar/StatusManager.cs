using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StatusManager : MonoBehaviour {

	private static StatusManager instance;
	public static StatusManager Instance
	{
		get{
			if (instance == null) {
				instance = FindObjectOfType<StatusManager> ();
			}
			return instance;
		}
	}
	//wifi
	private Wifi wifi = new Wifi();
	public Image wifiImage;
	//蓝牙
	private Bluetooth bluetooth = new Bluetooth();
	public Image bluetoothImage;
	//电量
	private Battery battery = new Battery();
	public Image batteryImage;
	public Text batteryText;
	//时间
	private SystemTime systemTime = new SystemTime();
	public Text timeText;
	//音量
	private Volume volume = new Volume();
	public GameObject volumeObj;
	public Image volumeImage;
	private float startShowVolumeTime = 0.0f;
	//手柄
	public Image handleImage;
    //设置
    public GameObject settingObj;

    public GridLayoutGroup mGrid;

    // Use this for initialization
    void Start () {
		//初始化数据
		wifi.NetworkState = (NetworkState)OtherUtils.Instance.GetNetworkConnectionState();
		wifi.WifiState = (WifiState)OtherUtils.Instance.GetWifiSwitchState();
		wifi.Level = OtherUtils.Instance.GetWifiLevel ().ToString();
		UpdateWifiUI ();
		bluetooth.BluetoothState = (BluetoothState)OtherUtils.Instance.GetBluetoothStatus ();
		UpdateBluetoothUI ();
		systemTime.Time = OtherUtils.Instance.GetCurrentTime ();
		UpdateTimeUI ();
	}
	
	// Update is called once per frame
	void Update () {
		if (startShowVolumeTime != 0.0f && Time.time - startShowVolumeTime > 1.0f) {
			volumeObj.SetActive (false);
			startShowVolumeTime = 0.0f;
		}
	}

	void OnApplicationPause(bool pauseStatus){
		if (!pauseStatus) {
			//更新时间
			UpdateTime (OtherUtils.Instance.GetCurrentTime());
		}
	}

	public void SetActive(bool isShow)
	{
		this.gameObject.SetActive (isShow);
	}

	/*
	 * 更新wifi图标
	*/
	private void UpdateWifiUI()
	{
		if (wifi.WifiState == WifiState.WIFI_STATE_ENABLED){
			//打开
			if(wifi.NetworkState == NetworkState.STATE_DISCONNECTED){
				//未连接
				wifiImage.sprite = Resources.Load("Image/wifi_disconnected", typeof(Sprite)) as Sprite;
			}else{
				//连接上热点
				wifiImage.sprite = Resources.Load("Image/wifi_connected_"+ wifi.Level, typeof(Sprite)) as Sprite;
			}
		}else{
			//未打开
			wifiImage.sprite = Resources.Load("Image/wifi_disabled", typeof(Sprite)) as Sprite;
		}
	}

	/*
	 * 更新蓝牙图标
	*/
	private void UpdateBluetoothUI()
	{
		if (bluetooth.BluetoothState == BluetoothState.STATE_ON) {
			if (bluetooth.BluetoothProfile == BluetoothProfile.STATE_CONNECTED) {
				bluetoothImage.sprite = Resources.Load("Image/bluetooth_connected", typeof(Sprite)) as Sprite;
			} else {
				bluetoothImage.sprite = Resources.Load("Image/bluetooth_on", typeof(Sprite)) as Sprite;
			}
		} else {
			bluetoothImage.sprite = Resources.Load("Image/bluetooth_off", typeof(Sprite)) as Sprite;
		}
	}

	/*
	 * 更新电池图标
	*/
	private void UpdateBatteryUI()
	{
		switch (battery.BatteryState) {
		case BatteryState.BATTERY_STATUS_CHARGING:
			batteryImage.sprite = Resources.Load("Image/electric_charging", typeof(Sprite)) as Sprite;
			batteryText.text = battery.Level + "%";
			break;
		case BatteryState.BATTERY_STATUS_DISCHARGING:
		case BatteryState.BATTERY_STATUS_NOT_CHARGING:
		case BatteryState.BATTERY_STATUS_FULL:
			float num = float.Parse (battery.Level) / 20;
			int level = Mathf.CeilToInt (num);
				batteryImage.sprite = Resources.Load("Image/electric_" + level.ToString(), typeof(Sprite)) as Sprite;
            batteryText.text = battery.Level + "%";
                break;
		}
	}

	/*
	 * 更新时间文本
	*/
	private void UpdateTimeUI()
	{
		timeText.text = systemTime.Time;
	}

	/*
	 * 更新音量
	*/
	private void UpdateVolumeUI()
	{
		if (startShowVolumeTime == 0.0f) {
			volumeObj.SetActive(true);
			volumeImage.fillAmount = float.Parse(volume.VolumeData)/(float)100;
			startShowVolumeTime = Time.time;
		}
	}

	public void UpdateNetworkState(string state)
	{
		wifi.NetworkState = (NetworkState)Enum.Parse (typeof(NetworkState), state);
		UpdateWifiUI ();
	}

	public void UpdateWifiState(string state)
	{
		wifi.WifiState = (WifiState)Enum.Parse (typeof(WifiState), state);
		UpdateWifiUI ();
	}

	public void UpdateWifiLevel(string level)
	{
		wifi.Level = level;
		UpdateWifiUI ();
	}

	public void UpdateBluetoothState(string state)
	{
		bluetooth.BluetoothState = (BluetoothState)Enum.Parse (typeof(BluetoothState), state);
		UpdateBluetoothUI ();
	}

	public void UpdateBluetoothConnectState(string state)
	{
		bluetooth.BluetoothProfile = (BluetoothProfile)Enum.Parse (typeof(BluetoothProfile), state);
		UpdateBluetoothUI ();
	}

	public void UpdateBatteryState(string state)
	{
		battery.BatteryState = (BatteryState)Enum.Parse (typeof(BatteryState), state);
		UpdateBatteryUI ();
	}
		
	public void UpdateBatteryLevel(string level)
	{
		battery.Level = level;
		UpdateBatteryUI ();
	}

	public void UpdateTime(string time)
	{
		systemTime.Time = time;
		UpdateTimeUI ();
	}

	public void UpdateVolume(string data)
	{
		volume.VolumeData = data;
		startShowVolumeTime = 0.0f;
        if (PUI_UnityAPI.GetDeviceMode() != DeviceMode.FalconCV2) {
            UpdateVolumeUI();
        }
	}

	public void UpdateHandle(bool isConnect)
	{
		handleImage.sprite = Resources.Load(isConnect?"Image/handle_connect":"Image/handle_close", typeof(Sprite)) as Sprite;
	}

	/*
	 * 点击wifi，打开vr设置wifi界面
	 */
	public void ClickWifi()
	{
		AppUtils.Instance.OpenAppByAction (Constant.SETTINGS_WIFI_ACTION);

        mGrid.enabled = false;
        mGrid.enabled = true;
    }

	/*
	 * 点击bluetooth，打开vr设置bluetooth界面
	 */
	public void ClickBluetooth()
	{
		AppUtils.Instance.OpenAppByAction (Constant.SETTINGS_BLUETOOTH_ACTION);

        mGrid.enabled = false;
        mGrid.enabled = true;
    }

	/*
	 * 点击手柄，打开vr设置手柄界面
	 */
	public void ClickHandle()
	{
		AppUtils.Instance.OpenAppByAction (Constant.SETTINGS_HANDLE_ACTION);

        mGrid.enabled = false;
        mGrid.enabled = true;
    }

	/*
	 * 点击设置，打开vr设置
	 */
	public void ClickSettings()
	{
		AppUtils.Instance.OpenAppByComponentName (Constant.SETTINGS_PACKAGE_NAME, Constant.SETTINGS_ACTIVITY_NAME);
		
        mGrid.enabled = false;
        mGrid.enabled = true;
    }

	/*
	 * 点击用户，打开用户中心
	 */
	public void ClickUserCenter()
	{
		AppUtils.Instance.OpenAppByComponentName (Constant.USERCENTER_PACKAGE_NAME, Constant.USERCENTER_ACTIVITY_NAME);
	}
}
