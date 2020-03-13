public class BaseModel
{

	//资源id(对用字段：mid)
	public int mid = -1;
	//数据类型(对用字段：type)
	public DataType dataType = DataType.UNKNOWN;
	//标题(对用字段：title)
	public string title = "pico";
	//图片地址(对用字段：focusimage)
	public string imageUrl = "";
	//VIDEO视频的播放地址、URL网址、EPISODE剧集的播放地址、GALLERY_IMAGE大图片地址(对用字段：url)
	public string url = "";
	//VIDEO视频类型(对用字段：newsubtype)
	public int videoType = -1;
	//GAME包名、APP包名或包名/类名(对用字段：packagename)
	public string packageName = "";
	//WING_CATEGORY一级分类id、STORE_CATEGORY二级分类(对用字段：pid)
	public int pid = -1;
	//内容详情(对用字段：content，公告栏数据特有)
	public string content = "";
	//时间(对用字段：date，公告栏数据特有)
	public string date = "";
	//button上的字符(对用字段：button，公告栏数据特有)
	public string button = "";

	public string ToString ()
	{
		return this.mid + "," + this.dataType + "," + this.title + "," +
		this.imageUrl + "," + this.url + "," + this.videoType + "," +
		this.packageName;
	}

	public bool Equals (BaseModel otherModel)
	{
		return this.ToString ().Equals (otherModel.ToString ());
	}

}