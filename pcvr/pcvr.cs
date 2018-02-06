#define SHI_ZI_QI_NANG

using UnityEngine;
using System.Collections;

public class pcvr : MonoBehaviour
{
    /// <summary>
    /// 是否是硬件版.
    /// </summary>
    static public bool bIsHardWare = true;
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
		if (Instance == null) {
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
		UpdatePcvrSteerVal();
        UpdatePlayerCoinDt();
    }

    /// <summary>
    /// 更新玩家币值信息.
    /// </summary>
    void UpdatePlayerCoinDt()
    {
        if (bIsHardWare)
        {
            if (GlobalData.CoinCur != mPcvrTXManage.PlayerCoinArray[0])
            {
                GlobalData.CoinCur = mPcvrTXManage.PlayerCoinArray[0];
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
    float TimeLastSteer = 0f;
    /// <summary>
    /// 更新转向信息.
    /// </summary>
	public void UpdatePcvrSteerVal()
    {
        if (!bIsHardWare)
        {
            mGetSteer = Input.GetAxis("Horizontal");
            return;
        }

        SteerEnum steerState = (SteerEnum)(MyCOMDevice.ComThreadClass.ReadByteMsg[30]);
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
                    if (Time.time - TimeLastSteer > 0.2f)
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
}