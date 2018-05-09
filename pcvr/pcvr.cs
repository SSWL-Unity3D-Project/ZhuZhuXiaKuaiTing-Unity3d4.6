#define USE_JING_RUI4_JIA_MI

using UnityEngine;

public class pcvr : MonoBehaviour
{
    /// <summary>
    /// 是否是硬件版.
    /// </summary>
	static public bool bIsHardWare = false;
    /// <summary>
    /// 是否校验hid.
    /// </summary>
    public static bool IsJiaoYanHid;
    /// <summary>
    /// pcvr通信数据管理.
    /// </summary>
    [HideInInspector]
    public pcvrTXManage mPcvrTXManage;
    static private pcvr Instance = null;
	static public pcvr GetInstance()
	{
		if (Instance == null)
		{
#if USE_JING_RUI4_JIA_MI
			Debug.Log("Start JingRui JiaMi test...");
			//GameRoot.StartInitialization();
#endif
			GameObject obj = new GameObject("_PCVR");
			DontDestroyOnLoad(obj);
			Instance = obj.AddComponent<pcvr>();
            Instance.mPcvrTXManage = obj.AddComponent<pcvrTXManage>();
            ScreenLog.init();
			if (bIsHardWare) {
				MyCOMDevice.GetInstance();
			}
		}
		return Instance;
	}

	void FixedUpdate()
	{
        byte[] readBuf = MyCOMDevice.ComThreadClass.ReadByteMsg;
        if (bIsHardWare)
        {
            UpdatePcvrSteerVal(readBuf[30]);
        }
        UpdatePlayerCoinDt();
    }

    void Update()
    {
        if (!bIsHardWare)
        {
            UpdatePcvrSteerVal(0);
        }
    }

    /// <summary>
    /// 更新玩家币值信息.
    /// </summary>
    void UpdatePlayerCoinDt()
    {
        if (bIsHardWare)
        {
            if (GlobalData.GetInstance().CoinCur != mPcvrTXManage.PlayerCoinArray[0])
            {
                GlobalData.GetInstance().CoinCur = mPcvrTXManage.PlayerCoinArray[0];
            }
        }
    }
    
    enum SteerEnum
    {
        //Left = 0x55,
        //Right = 0xaa,
        Left = 0xaa,
        Right = 0x55,
        Center = 0x00,
    }

    [HideInInspector]
    public float mGetSteer = 0f;
    [HideInInspector]
    public byte mPcvrSteerCur = 0;
    float TimeLastSteer = 0f;
    InputEventCtrl.ButtonState KeyBtA = InputEventCtrl.ButtonState.UP;
    InputEventCtrl.ButtonState KeyBtD = InputEventCtrl.ButtonState.UP;
    /// <summary>
    /// 更新转向信息.
    /// </summary>
    public void UpdatePcvrSteerVal(byte pcvrSteerVal)
    {
        if (!bIsHardWare)
        {
            //mGetSteer = Input.GetAxis("Horizontal");
            //return;
            if (Input.GetKeyDown(KeyCode.A))
            {
                KeyBtA = InputEventCtrl.ButtonState.DOWN;
                pcvrSteerVal = (byte)SteerEnum.Left;
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                KeyBtD = InputEventCtrl.ButtonState.DOWN;
                pcvrSteerVal = (byte)SteerEnum.Right;
            }

            if (Input.GetKeyUp(KeyCode.A))
            {
                KeyBtA = InputEventCtrl.ButtonState.UP;
            }

            if (Input.GetKeyUp(KeyCode.D))
            {
                KeyBtD = InputEventCtrl.ButtonState.UP;
            }

            if (KeyBtA == InputEventCtrl.ButtonState.UP && KeyBtD == InputEventCtrl.ButtonState.UP)
            {
                //A和D按键都弹起.
                pcvrSteerVal = (byte)SteerEnum.Center;
            }
            else
            {
                if (KeyBtA == InputEventCtrl.ButtonState.DOWN && KeyBtD == InputEventCtrl.ButtonState.DOWN)
                {
                    //A和D按键都按下.
                }
                else if (KeyBtA == InputEventCtrl.ButtonState.DOWN)
                {
                    pcvrSteerVal = (byte)SteerEnum.Left;
                }
                else if (KeyBtD == InputEventCtrl.ButtonState.DOWN)
                {
                    pcvrSteerVal = (byte)SteerEnum.Right;
                }
            }
        }

        mPcvrSteerCur = pcvrSteerVal;
        SteerEnum steerState = (SteerEnum)(mPcvrSteerCur);
        switch (steerState)
        {
            case SteerEnum.Left:
                {
                    mGetSteer = -1f;
                    TimeLastSteer = Time.time;
                    break;
                }
            case SteerEnum.Center:
                {
                    if (Time.time - TimeLastSteer > 0.34f)
                    {
                        mGetSteer = 0f;
                    }
                    break;
                }
            case SteerEnum.Right:
                {
                    mGetSteer = 1f;
                    TimeLastSteer = Time.time;
                    break;
                }
        }
    }

    /// <summary>
    /// 闪光灯控制命令.
    /// </summary>
    [System.Serializable]
    public class ShanGuangDengCmd
    {
        public bool[] LedCmd = new bool[7];
    }

    /// <summary>
    /// 改变闪关灯.
    /// </summary>
    public void ChangeShanGuangDeng(ShanGuangDengCmd shanGuangDengCmd)
    {
        for (int i = 0; i < 7; i++)
        {
            mPcvrTXManage.LedState[i] = shanGuangDengCmd.LedCmd[i];
        }
    }

    public void CloseShanGuangDeng()
    {
        for (int i = 0; i < 7; i++)
        {
            mPcvrTXManage.LedState[i] = false;
        }
    }

    //void OnGUI()
    //{
    //    string info = "ledState:  ";
    //    for (int i = 0; i < 7; i++)
    //    {
    //        info += mPcvrTXManage.LedState[i] == true ? "1  " : "0  ";
    //    }
    //    GUI.Box(new Rect(10f, 100f, Screen.width - 20f, 30f), info);
    //}
}