using UnityEngine;
using System.Collections;
using System.IO;

public class LauncherUtils
{
	/*
	 * 接口中需要语言参数
	 */
	public static string GetLanguage ()
	{
		string result = Localization.language;
		if (Localization.language == "zh") {
			result = "cn";
		}
		return result;
	}

	/*
	 * 获取PUI版本号
	 */
	public static string GetDisplayId ()
	{
		return GetSystemProperties("ro.pui.build.version", "2.4.0");
	}

	/*
	 * 获取HMD软件版本号
	 */
	public static string GetInternalVersion ()
	{
		return GetSystemProperties("ro.picovr.internal.version", "C086_RF01_BV2.0_SV0.88_20180120_B113");
	}

	/*
	 * 设备产品名
	 */
	public static string GetBuildProduct ()
	{
		if (Application.platform == RuntimePlatform.Android) {
			AndroidJavaClass jc = new AndroidJavaClass ("android.os.Build");
			string device = jc.GetStatic<string> ("PRODUCT");
			if (!string.IsNullOrEmpty (device) && device == "Pico Neo DK") {
				device = "Falcon";
			}
			return device;
		}
		return "A7210";
	}

	/*
	 * sn号
	 */
	public static string GetSnCode ()
	{
		if (Application.platform == RuntimePlatform.Android) {
			AndroidJavaClass jc = new AndroidJavaClass ("android.os.Build");
			string sn = jc.GetStatic<string> ("SERIAL");
			return sn;
		}
		return "dev000000000wayne";
	}

	/*
	 * 图片缓存目录 
	 */
	public static string GetImageCachePath ()
	{
		return imageCachePath;
	}
	private static string imageCachePath = "";
	public static void SetImageCachePath(string path)
	{
		imageCachePath = path;
	}

	/*
	 * 获取主页快速入口位在数据库中的字段
	 */
	public static string GetHomeAppField ()
	{
		string appField = "HomeApp";
		string countryCode = OtherUtils.Instance.GetCountryCode ();//国家码
		string productName = GetBuildProduct ();//产品名
		if (productName == "A7510_KT") {
			//ToB KT项目
			appField = "HomeApp_KT";
		} else {
			if (countryCode == "CN") {
				appField = "HomeApp";
			} else {
				appField = "HomeApp_US";
			}
		}
		LogUtils.Log ("GetHomeAppField--->countryCode = " + countryCode + ", productName = " + productName + ", appField = " + appField);
		return appField;
	}

	/*
	 * 获取主页推荐位在数据库中的字段
	 */
	public static string GetHomeRecField ()
	{
		string homeField = "HomeRec";
		string countryCode = OtherUtils.Instance.GetCountryCode ();//国家码
		string productName = GetBuildProduct ();//产品名
		if (productName == "A7510_KT") {
			//ToB KT项目
			homeField = "HomeRec_KT";
		} else {
			if (countryCode == "CN") {
				homeField = "HomeRec";
			} else {
				homeField = "HomeRec_US";
			}
		}
		LogUtils.Log ("GetHomeRecField--->countryCode = " + countryCode + ", productName = " + productName + ", homeField = " + homeField);
		return homeField;
	}

	/*
	 * 获取使用记录的key
	 */
	public static string GetRecentKey()
	{
		string key = Constant.PREFS_KEY + "_" + OtherUtils.Instance.GetCountryCode();
		LogUtils.Log ("RecentKey = " + key);
		return key;
	}

	/*
	 * 手柄是否连接
	 */
	public static bool IsControllerConnected ()
	{
		return Pvr_UnitySDKAPI.Controller.UPvr_GetControllerState (0) == Pvr_UnitySDKAPI.ControllerState.Connected ||
			Pvr_UnitySDKAPI.Controller.UPvr_GetControllerState (1) == Pvr_UnitySDKAPI.ControllerState.Connected;
	}


	public static string GetSystemProperties(string key,string def)
	{
		if (Application.platform == RuntimePlatform.Android) {
			AndroidJavaClass jc = new AndroidJavaClass ("android.os.SystemProperties");
			string value = jc.CallStatic<string> ("get",key,def);
			return value;
		}
		return def;
	}

	/*
	 * 点击推荐位触发的事件
	*/
	public static void DoSomething(BaseModel model)
	{
		if (model == null) {
			LogUtils.LogError ("BaseModel is null !!!");
			return;
		}
		Debug.Log (model.ToString());
		switch (model.dataType) {
		case DataType.VIDEO:
			//视频
			OpenVideo(model);
			break;
		case DataType.GAME:
			//Store中的游戏
			OpenStoreGame(model);
			break;
		case DataType.URL:
			//网页
			if (model.packageName.Equals (Constant.X5_PACKAGE_NAME) && AppUtils.Instance.IsInstall (Constant.X5_PACKAGE_NAME)) {
				AppUtils.Instance.OpenBrowserByUrlByX5 (model.url);
			} else {
				AppUtils.Instance.OpenBrowserByUrl (model.url);
			}
			break;
		case DataType.GALLERY_CATEGORY:
			//Gallery 分类
			AppUtils.Instance.StartGalleryCategory(model.mid.ToString());
			break;
		case DataType.GALLERY_IMAGE:
			//Gallery 全景图片
			AppUtils.Instance.StartGalleryPalyer(model.mid.ToString(), model.title, model.imageUrl, model.url);
			break;
		case DataType.GALLERY_THEME:
			//Gallery 专题
			AppUtils.Instance.StartGalleryTheme(model.mid.ToString());
			break;
		case DataType.WING_CATEGORY:
			//Wing 分类
			AppUtils.Instance.StartVRWingCategory(model.pid.ToString(), model.mid.ToString());
			break;
		case DataType.WING_THEME:
			//Wing 专题
			AppUtils.Instance.StartVRWingTheme(model.pid.ToString());
			break;
		case DataType.APP:
			//系统预制的App
			OpenApp(model);
			break;
		case DataType.RECENT:
			//TODO 最近使用
			break;
		case DataType.SYSTEM:
			//系统更新
			OpenSystemUpdate();
			break;
		case DataType.STORE_CATEGORY:
			//Store 分类
			AppUtils.Instance.StartStoreCategory(model.mid, model.pid);
			break;
		}
	}

	/*
	 * 播放视频 
	 * 1、本地存在，直接播放
	 * 2、本地不存在，查看网络
	 *		网络连接，打开播放器播放/打开Store详情页
	 *		网络未连接，弹出提示
	 */
	public static void OpenVideo(BaseModel model)
	{
		string path = model.url;
		if (File.Exists (path)) {
			AppUtils.Instance.StartVideo (path, model.mid.ToString (), "", model.videoType.ToString (), model.title);
		} else {
			if (!OtherUtils.Instance.GetNetStatus ()) {
				Toast.Show (Localization.Get ("Net_TimeOut"));
			} else {
				AppUtils.Instance.StartVideo (path, model.mid.ToString (), "", model.videoType.ToString (), model.title);
			}
		}
	}

	/*
	 * 打开游戏 
	 * 1、本地安装，直接打开
	 * 2、本地未安装，查看网络
	 *		网络连接，打开Store详情页
	 *		网络未连接，弹出提示
	 */
	public static void OpenStoreGame(BaseModel model)
	{
		string packageName = model.packageName;
		bool isInstalled = AppUtils.Instance.IsInstall(packageName);
		if(isInstalled){
			AppUtils.Instance.OpenAppByPackageName (packageName);
		}else{
			if (!OtherUtils.Instance.GetNetStatus ()) {
				Toast.Show (Localization.Get ("Net_TimeOut"));
			} else {
				AppUtils.Instance.StartStoreAppDetail (model.mid.ToString(), packageName);
			}
		}
	}

	/*
	 * 打开系统预制的app
	 */
	public static void OpenApp(BaseModel model)
	{
		string packageName = model.packageName;
		if(packageName.Contains("/")){
			string[] splits = packageName.Split ('/');
			string s1 = splits [0];
			string s2 = splits [1];
			bool isInstalled = AppUtils.Instance.IsInstall (s1);
			if (isInstalled) {
				AppUtils.Instance.OpenAppByComponentName (s1, s2);
			}
		}else{
			bool isInstalled = AppUtils.Instance.IsInstall (packageName);
			if (isInstalled) {
				AppUtils.Instance.OpenAppByPackageName (packageName);
			}
		}
	}

	/*
	 * 1.有应用升级->打开版本管理
	 * 
	 * 2.无应用升级->打开系统更新
	 * 		systemupdate和systemupdate2包名不一样，根据判断，哪个安装就打开哪个。
	 */
	public static void OpenSystemUpdate()
	{
		if (OtherUtils.Instance.HasAppUpdate() == Constant.HAS_APP_UPDATE) {
			OtherUtils.Instance.StartVersionManager ();
		} else {
			AppUtils.Instance.OpenAppByComponentName (Constant.SYSTEM_UPDATE_PACKAGE_NAME, Constant.SYSTEM_UPDATE_ACTIVITY_NAME);
		}
	}

	public static string WrapText(UnityEngine.UI.Text text, string content, int lines = 1, string endValue = "...")
	{
		string outstr = "";
		if (string.IsNullOrEmpty(content))
			return "";

		Font font = text.font;
		int size = text.fontSize;
		CharacterInfo characterInfo = new CharacterInfo();

		char[] endValueChar = endValue.ToCharArray();
		int endValueWidth = 0;

		if(endValueChar.Length > 0) 
			foreach(char c in endValueChar)
			{
				font.GetCharacterInfo(c, out characterInfo, size, FontStyle.Normal);
				endValueWidth += characterInfo.advance;
			}

		int textWidth = (int)text.rectTransform.sizeDelta.x;
		char[] contentChar = content.ToCharArray();

		int tempWidth = 0;
		foreach(char c in contentChar)
		{
			font.GetCharacterInfo(c, out characterInfo, size, FontStyle.Normal);
			tempWidth += characterInfo.advance;
			if(lines == 1) /**最后一行时**/
			{
				if (tempWidth <= textWidth - endValueWidth)  /**字符占据的宽度小于 text的宽度减去后缀的宽度，为可以显示的字符；否则，加上后缀返回**/
					outstr += c;
				else   
				{
					outstr += endValue;
					break;
				}
			}
			else   /**非最后一行**/
			{
				if(tempWidth <= textWidth)  /**字符占据的宽度小于 text的宽度，为可以显示的字符**/
				{
					outstr += c;
					if (tempWidth == textWidth)  /**字符占据的宽度等于 text的宽度，为可以显示的字符，此时行数减一，并将tempWidth设置为0**/
					{
						tempWidth = 0;
						lines -= 1;
					}
				}
				else   /**字符占据的宽度大于 text的宽度，仍为可以显示的字符，但是需要行数减一，并且tempWidth设置为当前字符的宽度**/
				{
					outstr += c;
					tempWidth = characterInfo.advance;
					lines -= 1;
				}
			}
		}
		return outstr;
	}

	/*
	* 获取语音助手的绑定状态
	*/
	public static bool getBindVoiceStatus()
	{
		bool status = false;
		#if UNITY_EDITOR
		status = true;
		#elif UNITY_ANDROID
		PicoUnityActivity.CallObjectMethod<bool> (ref status, "getBindVoiceStatus");
		#endif
		LogUtils.Log ("Bind VoiceAssistant Status : " + status);
		return status;
	}

}
