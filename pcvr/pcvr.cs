using UnityEngine;
using System.Collections;

public class pcvr : MonoBehaviour
{
	/// <summary>
	/// 是否开启精锐加密校验.
	/// </summary>
	public static bool IsOpenJingRuiJiaMi = true;
    /// <summary>
    /// 是否是硬件版.
    /// </summary>
	static public bool bIsHardWare = true;
	/// <summary>
	/// 测试去掉输入.
	/// </summary>
	public static bool IsTestNoInput = false;
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
			GameObject obj = new GameObject("_PCVR");
			DontDestroyOnLoad(obj);
			Instance = obj.AddComponent<pcvr>();
            Instance.mPcvrTXManage = obj.AddComponent<pcvrTXManage>();
            ScreenLog.init();
			if (bIsHardWare)
			{
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
			ChangDongGanBtLedState();
        }
        UpdatePlayerCoinDt();
    }

    void Update()
    {
        if (!bIsHardWare)
        {
            UpdatePcvrSteerVal(0);
        }

//		if (Input.GetKeyUp(KeyCode.P))
//		{
//			OnGameOverCheckJingRuiJiaMi(); //test
//		}
    }

	public void OnGameOverCheckJingRuiJiaMi()
	{
		if (!IsOpenJingRuiJiaMi)
		{
			return;
		}

		//进入待机画面做一次完全安全验证
		//如果验证失败了游戏就会被挂起
		//做校验
		GameRoot.CheckCipherText(StandbyProcess.VerifyEnvironmentKey_LogoVideo);
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

	/// <summary>
	/// 动感按键Led灯的时间记录信息.
	/// </summary>
	float m_TimeDongGanBtLed = 0f;
	/// <summary>
	/// 改变动感控制按键闪烁状态.
	/// </summary>
	void ChangDongGanBtLedState()
	{
		if (PlayerController.GetInstance() != null)
		{
			//游戏场景中.
			if (pcvr.GetInstance().mPcvrTXManage.JiDianQiCmdArray[0] == pcvrTXManage.JiDianQiCmd.Close)
			{
				//动感关闭状态下关闭提示灯.
				mPcvrTXManage.LedState[0] = false;
				return;
			}
		}

		if (Time.realtimeSinceStartup - m_TimeDongGanBtLed > 0.25f)
		{
			m_TimeDongGanBtLed = Time.realtimeSinceStartup;
			mPcvrTXManage.LedState[0] = !mPcvrTXManage.LedState[0];
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