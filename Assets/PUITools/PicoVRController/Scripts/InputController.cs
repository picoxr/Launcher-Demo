using UnityEngine;
using System.Collections.Generic;
using System;

public class InputController : MonoBehaviour {

    private static InputController m_Instance;


    private DeviceMode currentDevice = DeviceMode.Falcon;

    private Dictionary<string, LinkedList<Action>> _listners;

    private int m_Hand = 0;

    public int Hand
    {
        get
        {
            return m_Hand;
        }
		set
		{
            if (value < 0 || value > 1)
            {
                m_Hand = 0;
                Debug.Log("value: " + value + "   获取主手柄返回的值不在合理范围内，默认主手柄为0");
                return;
            }
            m_Hand = value;
            Hand_Second = 1 - m_Hand;
        }
    }


    private int m_Hand_Second = 1;

    public int Hand_Second
    {
        get
        {
            return m_Hand_Second;
        }
        private set
        {
            m_Hand_Second = value;
        }
    }

    



    public static InputController GetInstance()
    {
        if(m_Instance == null)
        {
            m_Instance = FindObjectOfType<InputController>();
            if (m_Instance == null)
                m_Instance = new GameObject("InputController").AddComponent<InputController>();
            m_Instance.GetDeviceInfo();
            try
            {
                if (m_Instance.currentDevice == DeviceMode.FalconCV || m_Instance.currentDevice == DeviceMode.FalconCV2)
                    m_Instance.GetMainController();
            }
            catch (System.Exception error)
            {
                Debug.Log("获取主手柄 出错：  " + error.Message);
            }

    }
        return m_Instance;
    }


    /// <summary>
    /// 当前设备
    /// </summary>
    private void GetDeviceInfo()
    {
        currentDevice = PUI_UnityAPI.GetDeviceMode();
    }


    /// <summary>
    /// 获取主手柄
    /// </summary>
    private void GetMainController()
    {
        m_Hand = Pvr_UnitySDKAPI.Controller.UPvr_GetMainHandNess();
    }


    /// <summary>
    /// 手柄切换 回调 
    /// </summary>
    /// <param name="hand"></param>
    private void OnChangeController(int hand)
    {
        this.m_Hand = hand;
    }




    /// <summary>
    /// 添加事件监听(不带参)
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="handler"></param>
    public void AddListener(ListenerEventType eventName, Action handler)
    {
        string key = eventName.ToString();
        if (_listners == null)
        {
            _listners = new Dictionary<string, LinkedList<Action>>();
        }
        if (_listners.ContainsKey(key))
        {
            _listners[key].AddLast(handler);
        }
        else
        {
            LinkedList<Action> handlers = new LinkedList<Action>();
            handlers.AddLast(handler);
            _listners.Add(key, handlers);
        }
    }


    /// <summary>
    /// 移除指定类型的单个事件监听(不带参)
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="handler"></param>
    public virtual void RemoveListener(ListenerEventType eventName, Action handler)
    {
        string key = eventName.ToString();
        if (_listners != null)
        {
            if (_listners.ContainsKey(key))
            {
                LinkedList<Action> handlers = _listners[key];
                handlers.Remove(handler);
                if (handlers.Count == 0)
                {
                    _listners.Remove(key);
                }
            }
        }
    }


    /// <summary>
    /// 派发事件(不带参)
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="eventParam"></param>
    public virtual void DispatchEvent(ListenerEventType eventName)
    {
        string key = eventName.ToString();
        if (_listners != null && _listners.ContainsKey(key))
        {
            LinkedList<Action> list = _listners[key];
            for (LinkedListNode<Action> handler = list.First; handler != null; handler = handler.Next)
            {
                handler.Value();
            }
        }
    }



    /// <summary>
    /// 移除指定类型的全部事件监听
    /// </summary>
    /// <param name="str"></param>
    public virtual void RemoveListeners(ListenerEventType eventName)
    {
        if (_listners != null && _listners.ContainsKey(eventName.ToString()))
        {
            _listners.Remove(eventName.ToString());
        }
    }

    /// <summary>
    /// 清空所有监听
    /// </summary>
    public virtual void ClearAllListener()
    {
        if (_listners != null)
        {
            _listners.Clear();
        }
    }




    // Use this for initialization
    void Start () {



    }
	
	// Update is called once per frame
	void Update () {

#if UNITY_EDITOR
        InputListenerPC();
#else
         switch(currentDevice)
        {
			case DeviceMode.Falcon:
                InputListenerDKBox();
                SlideListenerBox();
                break;
			case DeviceMode.Finch:
			case DeviceMode.FalconCV:
            case DeviceMode.FalconCV2:
                InputListenerController();
                SlideListenerController();
                InputListenerAndroid();  /**Finch、CV上执行该方法，DK/DKS上已经监听B键和A键**/
                break;
        }
#endif



    }



    /// <summary>
    /// Android 自带的两个按键监听  确认和取消   （DK/DKS上等同于A键 和 B键）
    /// </summary>
    public void InputListenerAndroid()
    {
        if (Input.GetKeyUp(KeyCode.JoystickButton0))  /**确认**/
        {
            Debug.Log("头盔上的确认键");
        }


        if (Input.GetKeyUp(KeyCode.JoystickButton1)) /**返回**/
        {
            Debug.Log("头盔上的返回键");
        }
    }


    /// <summary>
    /// Android 自带的Home键长按 短按  (需要在Android 端添加监听发消息到此)
    /// </summary>
    /// <param name="str"></param>
    public void OnHomeKeyEvent(string str)
    {
        Debug.Log("收到的字符： " + str); /*短按homekey 长按recentapps***/

       if(str.Equals("recentapps"))  /**与controller的长按Home键执行一致**/
        {
            DispatchEvent(ListenerEventType.LONGPRESS_HOME);  
        }
       else if(str.Equals("homekey"))  /**短按Home键返回Launcher  系统已实现**/
        {

        }

    }


    /// <summary>
    /// DK/DKS上   B键，X键，A键
    /// </summary>
    public void InputListenerDKBox()
    {
        if(Input.GetButtonUp("Cancel"))
        {
            DispatchEvent(ListenerEventType.CANCEL);
        }

        if(Input.GetButtonUp("Recenter"))
        {
            DispatchEvent(ListenerEventType.RECENTER);
        }

        if(Input.GetButtonUp("Submit"))
        {
            DispatchEvent(ListenerEventType.SUBMIT);
        }
    }





    /// <summary>
    /// 手柄的按键监听  （App、Touchpad、长按短按Home键）
    /// </summary>
    public void InputListenerController()
    {
        if (Pvr_UnitySDKAPI.Controller.UPvr_GetKeyUp(m_Hand, Pvr_UnitySDKAPI.Pvr_KeyCode.APP))  /**App键  暂定为返回、退出 **/
        {
            DispatchEvent(ListenerEventType.APP);
        }

        if (Pvr_UnitySDKAPI.Controller.UPvr_GetKeyUp(m_Hand, Pvr_UnitySDKAPI.Pvr_KeyCode.TOUCHPAD)) /**TouchPad键监听**/
        {
            DispatchEvent(ListenerEventType.TOUCHPAD);
        }

        if (Pvr_UnitySDKAPI.Controller.UPvr_GetKeyLongPressed(m_Hand, Pvr_UnitySDKAPI.Pvr_KeyCode.HOME)) /**长按Home键**/
        {
            DispatchEvent(ListenerEventType.LONGPRESS_HOME);
        }

		if (Pvr_UnitySDKAPI.Controller.UPvr_GetKeyUp(m_Hand, Pvr_UnitySDKAPI.Pvr_KeyCode.TRIGGER))
		{
			DispatchEvent(ListenerEventType.TRIGGER);
            Debug.Log("PUI________________TRIGGER   Click ...........................");
		}

        if(Pvr_UnitySDKAPI.Controller.UPvr_GetKeyUp(m_Hand_Second, Pvr_UnitySDKAPI.Pvr_KeyCode.TRIGGER))
        {
            DispatchEvent(ListenerEventType.TRIGGER_SECOND);
            Debug.Log("PUI________________TRIGGER_SECOND   Click ...........................");
        }

    }



    /// <summary>
    /// 手柄上的滑动监听
    /// </summary>
    public void SlideListenerController()
    {
        if (Pvr_UnitySDKAPI.Controller.UPvr_GetSwipeDirection(m_Hand) == Pvr_UnitySDKAPI.SwipeDirection.SwipeDown)
        {
            DispatchEvent(ListenerEventType.SLIDE_DOWN);
            Debug.Log("-----InputController----------- Slide Down");
        }
        else if (Pvr_UnitySDKAPI.Controller.UPvr_GetSwipeDirection(m_Hand) == Pvr_UnitySDKAPI.SwipeDirection.SwipeUp)
        {
            DispatchEvent(ListenerEventType.SLIDE_UP);
            Debug.Log("-----InputController----------- Slide Up");
        }
        else if (Pvr_UnitySDKAPI.Controller.UPvr_GetSwipeDirection(m_Hand) == Pvr_UnitySDKAPI.SwipeDirection.SwipeLeft)
        {
            DispatchEvent(ListenerEventType.SLIDE_LEFT);
            Debug.Log("-----InputController----------- Slide Left");
        }
        else if (Pvr_UnitySDKAPI.Controller.UPvr_GetSwipeDirection(m_Hand) == Pvr_UnitySDKAPI.SwipeDirection.SwipeRight)
        {
            DispatchEvent(ListenerEventType.SLIDE_RIGHT);
            Debug.Log("-----InputController----------- Slide Right");
        }

        //ToDo: 差一个把滑动归为No的接口,否则滑动会被一直执行;
    }


    private float lastTime;


    /// <summary>
    /// DK/DKS上的竖直水平上的监听
    /// </summary>
    public void SlideListenerBox()
    {
        if (Input.GetAxis("Vertical") > 0.9f)
        {
            if (Time.time - lastTime > 1.0f)
            {
                lastTime = Time.time;
                DispatchEvent(ListenerEventType.SLIDE_UP);
            }
        }
        else if (Input.GetAxis("Vertical") < -0.9f)
        {
            if (Time.time - lastTime > 1.0f)
            {
                lastTime = Time.time;
                DispatchEvent(ListenerEventType.SLIDE_DOWN);
            }
        }


        if (Input.GetAxis("Horizontal") > 0.9f)
        {
            if (Time.time - lastTime > 1.0f)
            {
                lastTime = Time.time;
                DispatchEvent(ListenerEventType.SLIDE_LEFT);
            }
        }
        else if (Input.GetAxis("Horizontal") < -0.9f)
        {
            if (Time.time - lastTime > 1.0f)
            {
                lastTime = Time.time;
                DispatchEvent(ListenerEventType.SLIDE_RIGHT);
            }
        }
    }


    /// <summary>
    /// 一些PC上的测试的监听
    /// </summary>
    private void InputListenerPC()
    {
        if(Input.GetKey(KeyCode.UpArrow))  /**模拟上滑**/
        {
            if (Time.time - lastTime > 1.0f)
            {
                DispatchEvent(ListenerEventType.SLIDE_UP);
                lastTime = Time.time;
            }
        }
        else if(Input.GetKey(KeyCode.DownArrow))   /**模拟下滑**/
        {
            DispatchEvent(ListenerEventType.SLIDE_DOWN);
            lastTime = Time.time;
        }
        else if(Input.GetKey(KeyCode.LeftArrow))   /**模拟左滑**/
        {
            if (Time.time - lastTime > 1.0f)
            {
                DispatchEvent(ListenerEventType.SLIDE_LEFT);
                lastTime = Time.time;
            }
        }
        else if(Input.GetKey(KeyCode.RightArrow))  /**模拟右滑**/
        {
            if (Time.time - lastTime > 1.0f)
            {
                DispatchEvent(ListenerEventType.SLIDE_RIGHT);
                lastTime = Time.time;
            }
        }

        if(Input.GetKeyUp(KeyCode.KeypadEnter))   /**模拟Box上的A键  controller上的 touchpad**/
        {
            DispatchEvent(ListenerEventType.SUBMIT); /*DispatchEvent(ListenerEventType.TOUCHPAD);*/
        }

        if(Input.GetKeyUp(KeyCode.Backspace))   /**模拟 Box上的B键   controller上的APP**/
        {
            DispatchEvent(ListenerEventType.CANCEL); /*DispatchEvent(ListenerEventType.APP);*/
        }

        if(Input.GetKeyUp(KeyCode.Space))  /**模拟Box上的X键  controller上的长按Home**/
        {
            DispatchEvent(ListenerEventType.RECENTER);/* DispatchEvent(ListenerEventType.LONGPRESS_HOME);*/
        }
    }


    public bool TriggerDown
    {
        get
        {

#if UNITY_EDITOR
            return Input.GetMouseButtonDown(0);
#else
            return Input.GetKeyDown(KeyCode.JoystickButton0) ||
                Pvr_UnitySDKAPI.Controller.UPvr_GetKeyDown(m_Hand, Pvr_UnitySDKAPI.Pvr_KeyCode.TOUCHPAD) ||
                  Pvr_UnitySDKAPI.Controller.UPvr_GetKeyDown(m_Hand, Pvr_UnitySDKAPI.Pvr_KeyCode.TRIGGER) ||
                Input.GetButtonDown("Submit");
#endif
        }
    }



    public bool Triggering
    {
        get
        {
#if UNITY_EDITOR
            return Input.GetMouseButton(0);
#else
            return Input.GetKey(KeyCode.Joystick1Button0) ||
                Pvr_UnitySDKAPI.Controller.UPvr_GetKey(m_Hand, Pvr_UnitySDKAPI.Pvr_KeyCode.TOUCHPAD) ||
                Pvr_UnitySDKAPI.Controller.UPvr_GetKey(m_Hand, Pvr_UnitySDKAPI.Pvr_KeyCode.TRIGGER) ||
                Input.GetButton("Submit");
#endif

        }
    }
}


/// <summary>
/// 按键事件监听
/// </summary>
public enum ListenerEventType
{
    /// <summary>
    ///  APP 按键事件
    /// </summary>
    APP,

    /// <summary>
    /// Touchpad事件
    /// </summary>
    TOUCHPAD,

    /// <summary>
    /// 长按Home
    /// </summary>
    LONGPRESS_HOME,

	/// <summary>
	/// The TRIGGE.
	/// </summary>
	TRIGGER,

    /// <summary>
    /// Box  上的B键
    /// </summary>
    CANCEL,

    /// <summary>
    /// Box 上的X键
    /// </summary>
    RECENTER,

    /// <summary>
    /// Box 上的A键
    /// </summary>
    SUBMIT,

    /// <summary>
    /// 上滑动
    /// </summary>
    SLIDE_UP,

    /// <summary>
    /// 下滑动
    /// </summary>
    SLIDE_DOWN,

    /// <summary>
    /// 左滑动
    /// </summary>
    SLIDE_LEFT,

    /// <summary>
    /// 右滑动
    /// </summary>
    SLIDE_RIGHT,

    /// <summary>
    ///  副手柄Trigger键
    /// </summary>
    TRIGGER_SECOND
}