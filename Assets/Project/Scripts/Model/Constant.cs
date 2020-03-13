using UnityEngine;
using System.Collections;

public class Constant
{
	public const bool IS_DEBUG = true;//是否是debug模式
	public const string SP_IF_CAN_UPDATE = "ifCanUpdate";//是否能升级
	public const int CAN_UPDATE = 1;//可以升级
	public const int CANNOT_UPDATE = 0;//不能升级，默认
	//某些app的包名或类名
	public const string SYSTEM_UPDATE_PACKAGE_NAME = "com.picovr.updatesystem";//systemupdate的包名
	public const string SYSTEM_UPDATE_ACTIVITY_NAME = "com.picovr.updatesystem.MainActivity";//systemupdate的类名
	public const string USERCENTER_PACKAGE_NAME = "com.picovr.vrusercenter";//用户中心的包名
	public const string USERCENTER_ACTIVITY_NAME = "com.picovr.vrusercenter.MainActivity";//用户中心类名
	public const string SETTINGS_PACKAGE_NAME = "com.picovr.settings";//设置的包名
	public const string SETTINGS_ACTIVITY_NAME = "com.picovr.vrsettingslib.UnityActivity";//设置的类名
	public const string SETTINGS_WIFI_ACTION = "pui.settings.action.WIFI_SETTINGS";//wifi的action
	public const string SETTINGS_BLUETOOTH_ACTION = "pui.settings.action.BLUETOOTH_SETTINGS";//bluetooth的action
	public const string SETTINGS_HANDLE_ACTION = "pui.settings.action.CONTROLLER_SETTINGS";//handle的action
	public const string STORE_PACKAGE_NAME = "com.picovr.store";//store的包名
	public const string STORE_ACTIVITY_NAME = "com.picovr.store.UnityActivity";//store的类名
	public const string VIVEPORT_PACKAGE_NAME = "com.htc.viveport.store";//htc viveport包名
	public const string FILE_MANAGER_PACKAGE_NAME = "com.picovr.filemanager";//文件管理的包名
	public const string FILE_MANAGER2_PACKAGE_NAME = "com.pvr.filemanager";//新版文件管理的包名
	public const string X5_PACKAGE_NAME = "com.pvr.innerweb";//X5浏览器的包名
	//请求地址
	public static string BASE_URL = "http://pui.picovr.com/api/";
	public static string API_KEY = "6u2qBI2DF26a6tA6FbCm3vlA8azxAUl6";
	//数据接口的参数
	public const string API_HOME = "pui3/home";//主页推荐位
	public const string API_NOTICE = "announcement";//公告板
	public const string API_APP = "pui3/home/quick";//app推荐位
	//网络请求的几种返回状态
	public const int MSG_NORMAL = 0;
	public const int MSG_NONETWORK = -102;
	public const int MSG_ABORTED = -117;
	public const int MSG_CONNECT_TIMEOUT = -118;
	public const int MSG_TIMEOUT = -119;
	public const int MSG_ERROR = -10000;
	//数据库信息
	public const string DB_HOME_TABLE = "T_Config";//数据库中主页表的名字，推荐位和公告数据都在此表中
	//存在共享参数中的历史记录信息
	public const string PREFS_KEY = "history_records";
	//从scene app跳转到launcher
	public const string FROM_SCENE_APP = "scene_app";
	//当前使用的是哪个场景
	public const string SP_SCENE_INDEX = "scene_index";
	//天猫协议
	public const string SP_TMALL_PROTOCOL_KEY = "tmall_protocol";
	public const int SP_TMALL_PROTOCOL_DEF_VALUE = 0;
	public const int SP_TMALL_PROTOCOL_VALUE = 1;
	//数据库版本
	public const string SP_DB_VERSION = "dbVersion";
	public const int SP_DB_VERSION_DEF_VALUE = 1;
	//避光提示
	public const string SP_SUN_TIP = "sunTip";
	public const int SP_SUN_TIP_DEF_VALUE = 0;
	public const int SP_SUN_TIP_VALUE = 1;
	//系统里有新版本可用于更新
	public const int HAS_SYSTEM_UPDATE = 1;
	//系统里有app新版本可用于更新
	public const int HAS_APP_UPDATE = 1;
}

/*
 * 请求到的数据的类型
 */
public enum DataType
{
	UNKNOWN = 0,
	//视频
	VIDEO = 1,
	//游戏，store中的资源
	GAME = 2,
	//网址，默认浏览器打开
	URL = 3,
	//Gallery分类
	GALLERY_CATEGORY = 4,
	//Gallery单张图片
	GALLERY_IMAGE = 5,
	//在线影院剧集，TODO,该type暂未使用
	EPISODE = 6,
	//Gallery专题
	GALLERY_THEME = 7,
	//Wing分类
	WING_CATEGORY = 8,
	//Wing专题
	WING_THEME = 9,
	//打开应用，rom预制的
	APP = 10,
	//打开最近使用，TODO,无相关界面，该type暂未使用
	RECENT = 11,
	//有系统升级信息
	SYSTEM = 12,
	//Store分类
	STORE_CATEGORY = 13,
}

/*
 * 视频的类型
 */
public enum VideoType
{
	//2D
	VIDOE_2D = 0,
	//3D左右
	VIDOE_3D_LR = 1,
	//全景 2D
	VIDOE_360_2D = 2,
	//全景 3D 上下
	VIDOE_360_3D_TB = 3,
	//全景 3D 下上
	VIDOE_360_3D_BT = 4,
	//全景 3D 左右
	VIDOE_360_3D_LR = 5,
	//全景 3D 右左
	VIDOE_360_3D_RL = 6,
	//3D 上下
	VIDOE_3D_TB = 7,
	//3D 下上
	VIDOE_3D_BT = 8,
	//3D 右左
	VIDOE_3D_RL = 9,
	//180 2D
	VIDOE_180_2D = 10,
	//180 3D 上下
	VIDOE_180_3D_TB = 11,
	//180 3D 下上
	VIDOE_180_3D_BT = 12,
	//180 3D 左右
	VIDOE_180_3D_LR = 13,
	//180 3D 右左
	VIDOE_180_3D_RL = 14,
}

/*
 * 页面类型
 */
public enum ScreenId{
	HOME = 1,
	APP = 2,
	SCENE = 3,
}

/*
 * 页面的状态
 */
public enum ScreenState{
	UNKNOWN,
	ACTIVE,
	HIDDEN
}

public enum AppListType{
	APP = 0,
	RECENT = 1 //pui3.0上，无该列表，暂时无用
}
