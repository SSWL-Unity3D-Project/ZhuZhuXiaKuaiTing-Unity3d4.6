using UnityEngine;
using System;

public class SetPanel : MonoBehaviour
{
    enum SetEnum
    {
        CoinStart,  //启动币数.
        OperMode,   //运营模式.
        FreeMode,   //免费模式.
        CaiPiao,    //彩票设置.
        ChuPiaoLv1,        //出票率1.
        ChuPiaoLv2,        //出票率2.
        ChuPiaoLv3,        //出票率3.
        PrintCaiPiao,      //打印彩票.
        NoPrintCaiPiao,    //不打印彩票.
        Audio,      //音量设置.
        ResetAudio, //音量重置.
        Grade1,     //难度低.
        Grade2,     //难度中.
        Grade3,     //难度高.
        Reset,      //恢复出厂设置.
        LanguageCH,   //中文语言设置.
        LanguageEN,   //英文语言设置.
        Exit,         //退出.
    }

	public GameObject m_ZhujiemianObject;
	public Transform m_ZhujiemianXingXing;
	private int m_IndexZhujiemian = 0;
    /// <summary>
    /// 游戏版本.
    /// </summary>
    //public UILabel mGameVersionLb;
	public UILabel m_CoinForStar;
    public UILabel PlayerMinSpeed;
	public UITexture m_GameModeDuigou1;
	public UITexture m_GameModeDuigou2;
	public UITexture[] GameGradeDuiGou;
	public UITexture[] ChuPiaoLvDuiGou;
	public UITexture[] ChuPiaoDuiGou;
	public UITexture[] LanguageDuiGou;
    public UILabel InsertCoinNumLabel;
	public UILabel BtInfoLabel;
	public UILabel FangXiangInfoLabel;
    /// <summary>
    /// 彩票信息.
    /// </summary>
    public UILabel CaiPiaoInfoLb;
    private int m_InserNum = 0;
	int GameAudioVolume;
	void Start ()
    {
        //mGameVersionLb.text = "Version: 20180628";
		XkGameCtrl.IsLoadingLevel = false;
		AudioListener.volume = (float)ReadGameInfo.GetInstance().ReadGameAudioVolume() / 10f;
        if (pcvr.bIsHardWare)
        {
            pcvr.GetInstance().mPcvrTXManage.SetJiDianQiCmd(0, pcvrTXManage.JiDianQiCmd.Close);
        }
        GameAudioVolume = ReadGameInfo.GetInstance().ReadGameAudioVolume();
		GameAudioVolumeLB.text = GameAudioVolume.ToString();
        
		m_InserNum = Convert.ToInt32(ReadGameInfo.GetInstance().ReadInsertCoinNum());
		UpdateInsertCoin();

		BtInfoLabel.text = "";
        m_ZhujiemianXingXing.localPosition = new Vector3(-515f, 140f, 0f);
		string GameMode = ReadGameInfo.GetInstance().ReadGameStarMode();
		if (GameMode == "" || GameMode == null)
        {
			GameMode = ReadGameInfo.GameMode.Oper.ToString();
		}

		m_CoinForStar.text = ReadGameInfo.GetInstance().ReadStarCoinNumSet();
		if(GameMode == ReadGameInfo.GameMode.Oper.ToString())
		{
			m_GameModeDuigou1.enabled = true;
			m_GameModeDuigou2.enabled = false;
		}
		else
		{
			m_GameModeDuigou1.enabled = false;
			m_GameModeDuigou2.enabled = true;
		}

        int caiPiaoNum = ReadGameInfo.GetInstance().ReadGamePrintCaiPiaoNum();
        CaiPiaoInfoLb.text = caiPiaoNum.ToString();

        int chuPiaoLv = ReadGameInfo.GetInstance().ReadChuPiaoLv();
        ChuPiaoLvDuiGou[0].enabled = chuPiaoLv == 80 ? true : false;
        ChuPiaoLvDuiGou[1].enabled = chuPiaoLv == 100 ? true : false;
        ChuPiaoLvDuiGou[2].enabled = chuPiaoLv == 120 ? true : false;

        bool isPrintCaiPiao = ReadGameInfo.GetInstance().ReadGameIsPrintCaiPiao();
        ChuPiaoDuiGou[0].enabled = isPrintCaiPiao;
        ChuPiaoDuiGou[1].enabled = !isPrintCaiPiao;
        
        int grade = ReadGameInfo.GetInstance().ReadGrade();
        GameGradeDuiGou[0].enabled = grade == 1 ? true : false;
        GameGradeDuiGou[1].enabled = grade == 2 ? true : false;
        GameGradeDuiGou[2].enabled = grade == 3 ? true : false;
        InitGameLanguage();

        InputEventCtrl.GetInstance().mListenPcInputEvent.ClickSetEnterBtEvent += ClickSetEnterBtEvent;
		InputEventCtrl.GetInstance().mListenPcInputEvent.ClickSetMoveBtEvent += ClickSetMoveBtEvent;
		InputEventCtrl.GetInstance().mListenPcInputEvent.ClickCloseDongGanBtEvent += ClickCloseDongGanBtEvent;
    }

	void Update () 
	{
		if (pcvr.bIsHardWare)
        {
			if (GlobalData.GetInstance().CoinCur > m_InserNum)
            {
				m_InserNum = GlobalData.GetInstance().CoinCur - 1;
				OnClickInsertBt();
			}

			FangXiangInfoLabel.text = pcvr.GetInstance().mPcvrSteerCur.ToString("X2");
            float offsetSteer = 0.05f;
            if (pcvr.GetInstance().mGetSteer < -offsetSteer)
            {
                FangXiangInfoLabel.text += ", Turn Left";
            }
            else if (pcvr.GetInstance().mGetSteer > offsetSteer)
            {
                FangXiangInfoLabel.text += ", Turn Right";
            }
            else
            {
                FangXiangInfoLabel.text += ", Turn Middle";
            }
        }
		else
        {
			if (Input.GetKeyDown(KeyCode.T))
            {
				OnClickInsertBt();
			}

			int val = (int)(pcvr.GetInstance().mGetSteer * 100);
			FangXiangInfoLabel.text = val.ToString();
            if (val < 0)
            {
                FangXiangInfoLabel.text += ", Turn Left";
            }
            else if (val > 0)
            {
                FangXiangInfoLabel.text += ", Turn Right";
            }
            else
            {
                FangXiangInfoLabel.text += ", Turn Middle";
            }
        }
	}

	void OnClickInsertBt()
	{
		m_InserNum++;
		ReadGameInfo.GetInstance().WriteInsertCoinNum(m_InserNum.ToString());
		UpdateInsertCoin();
	}
	
	void UpdateInsertCoin()
	{
		InsertCoinNumLabel.text = m_InserNum.ToString();
	}

	void ClickSetMoveBtEvent(InputEventCtrl.ButtonState val)
	{
		if (val == InputEventCtrl.ButtonState.UP) {
			return;
		}
		OnClickMoveBtInZhujiemian();
	}

	void ClickSetEnterBtEvent(InputEventCtrl.ButtonState val)
	{
		if (val == InputEventCtrl.ButtonState.UP) {
			return;
		}
		OnClickSelectBtInZhujiemian();
	}

	void ClickCloseDongGanBtEvent(InputEventCtrl.ButtonState val)
	{
		if (val == InputEventCtrl.ButtonState.DOWN) {
			BtInfoLabel.text = "动感按键按下";
		}
		else {
			BtInfoLabel.text = "动感按键弹起";
		}
	}

    void OnClickMoveBtInZhujiemian()
	{
        m_IndexZhujiemian++;
        if (m_IndexZhujiemian > (int)SetEnum.Exit)
        {
            m_IndexZhujiemian = (int)SetEnum.CoinStart;
        }

        SetEnum enumSet = (SetEnum)m_IndexZhujiemian;
        switch (enumSet)
        {
            case SetEnum.CoinStart:
                {
                    m_ZhujiemianXingXing.localPosition = new Vector3(-515f, 140f, 0f);
                    break;
                }
            case SetEnum.OperMode:
                {
                    m_ZhujiemianXingXing.localPosition = new Vector3(-640f, 60f, 0f);
                    break;
                }
            case SetEnum.FreeMode:
                {
                    m_ZhujiemianXingXing.localPosition = new Vector3(-280f, 60f, 0f);
                    break;
                }
            case SetEnum.CaiPiao:
                {
                    m_ZhujiemianXingXing.localPosition = new Vector3(-535f, -10f, 0f);
                    break;
                }
            case SetEnum.ChuPiaoLv1:
                {
                    m_ZhujiemianXingXing.localPosition = new Vector3(-510f, -80f, 0f);
                    break;
                }
            case SetEnum.ChuPiaoLv2:
                {
                    m_ZhujiemianXingXing.localPosition = new Vector3(-410f, -80f, 0f);
                    break;
                }
            case SetEnum.ChuPiaoLv3:
                {
                    m_ZhujiemianXingXing.localPosition = new Vector3(-310f, -80f, 0f);
                    break;
                }
            case SetEnum.PrintCaiPiao:
                {
                    m_ZhujiemianXingXing.localPosition = new Vector3(-510f, -150f, 0f);
                    break;
                }
            case SetEnum.NoPrintCaiPiao:
                {
                    m_ZhujiemianXingXing.localPosition = new Vector3(-310f, -150f, 0f);
                    break;
                }
            case SetEnum.Audio:
                {
                    m_ZhujiemianXingXing.localPosition = new Vector3(-530f, -230f, 0f);
                    break;
                }
            case SetEnum.ResetAudio:
                {
                    m_ZhujiemianXingXing.localPosition = new Vector3(-350f, -230f, 0f);
                    break;
                }
            case SetEnum.Grade1:
                {
                    m_ZhujiemianXingXing.localPosition = new Vector3(40f, 135f, 0f);
                    break;
                }
            case SetEnum.Grade2:
                {
                    m_ZhujiemianXingXing.localPosition = new Vector3(180f, 135f, 0f);
                    break;
                }
            case SetEnum.Grade3:
                {
                    m_ZhujiemianXingXing.localPosition = new Vector3(360f, 135f, 0f);
                    break;
                }
            case SetEnum.Reset:
                {
                    m_ZhujiemianXingXing.localPosition = new Vector3(135f, -155f, 0f);
                    break;
                }
            case SetEnum.LanguageCH:
				{
					m_ZhujiemianXingXing.localPosition = new Vector3(150f, -225f, 0f);
                    break;
                }
            case SetEnum.LanguageEN:
                {
                    m_ZhujiemianXingXing.localPosition = new Vector3(380f, -225f, 0f);
                    break;
                }
            case SetEnum.Exit:
                {
                    m_ZhujiemianXingXing.localPosition = new Vector3(-175f, -295f, 0f);
                    break;
                }
        }
    }

    void OnClickSelectBtInZhujiemian()
    {
        SetEnum enumSet = (SetEnum)m_IndexZhujiemian;
        switch (enumSet)
        {
            case SetEnum.CoinStart:
                {
                    int CoinNum = Convert.ToInt32(m_CoinForStar.text);
                    CoinNum++;
                    if (CoinNum > 9)
                    {
                        CoinNum = 1;
                    }
                    m_CoinForStar.text = CoinNum.ToString();
                    ReadGameInfo.GetInstance().WriteStarCoinNumSet(CoinNum.ToString());
                    break;
                }
            case SetEnum.OperMode:
                {
                    m_GameModeDuigou1.enabled = true;
                    m_GameModeDuigou2.enabled = false;
                    ReadGameInfo.GetInstance().WriteGameStarMode(ReadGameInfo.GameMode.Oper.ToString());
                    break;
                }
            case SetEnum.FreeMode:
                {
                    m_GameModeDuigou1.enabled = false;
                    m_GameModeDuigou2.enabled = true;
                    ReadGameInfo.GetInstance().WriteGameStarMode(ReadGameInfo.GameMode.Free.ToString());
                    break;
                }
            case SetEnum.CaiPiao:
                {
                    int caiPiaoNum = ReadGameInfo.GetInstance().ReadGamePrintCaiPiaoNum();
                    caiPiaoNum += 5;
                    if (caiPiaoNum > 50)
                    {
                        caiPiaoNum = 5;
                    }
                    CaiPiaoInfoLb.text = caiPiaoNum.ToString();
                    ReadGameInfo.GetInstance().WriteGamePrintCaiPiaoNum(caiPiaoNum);
                    break;
                }
            case SetEnum.ChuPiaoLv1:
                {
                    ChuPiaoLvDuiGou[0].enabled = true;
                    ChuPiaoLvDuiGou[1].enabled = false;
                    ChuPiaoLvDuiGou[2].enabled = false;
                    ReadGameInfo.GetInstance().WriteChuPiaoLv(80);
                    break;
                }
            case SetEnum.ChuPiaoLv2:
                {
                    ChuPiaoLvDuiGou[0].enabled = false;
                    ChuPiaoLvDuiGou[1].enabled = true;
                    ChuPiaoLvDuiGou[2].enabled = false;
                    ReadGameInfo.GetInstance().WriteChuPiaoLv(100);
                    break;
                }
            case SetEnum.ChuPiaoLv3:
                {
                    ChuPiaoLvDuiGou[0].enabled = false;
                    ChuPiaoLvDuiGou[1].enabled = false;
                    ChuPiaoLvDuiGou[2].enabled = true;
                    ReadGameInfo.GetInstance().WriteChuPiaoLv(120);
                    break;
                }
            case SetEnum.PrintCaiPiao:
                {
                    ChuPiaoDuiGou[0].enabled = true;
                    ChuPiaoDuiGou[1].enabled = false;
                    ReadGameInfo.GetInstance().WriteGameIsPrintCaiPiao(true);
                    break;
                }
            case SetEnum.NoPrintCaiPiao:
                {
                    ChuPiaoDuiGou[0].enabled = false;
                    ChuPiaoDuiGou[1].enabled = true;
                    ReadGameInfo.GetInstance().WriteGameIsPrintCaiPiao(false);
                    break;
                }
            case SetEnum.Audio:
                {
                    GameAudioVolume++;
                    if (GameAudioVolume > 10)
                    {
                        GameAudioVolume = 0;
                    }
                    GameAudioVolumeLB.text = GameAudioVolume.ToString();
                    ReadGameInfo.GetInstance().WriteGameAudioVolume(GameAudioVolume);
                    break;
                }
            case SetEnum.ResetAudio:
                {
                    GameAudioVolume = 7;
                    GameAudioVolumeLB.text = GameAudioVolume.ToString();
                    ReadGameInfo.GetInstance().WriteGameAudioVolume(GameAudioVolume);
                    break;
                }
            case SetEnum.Grade1:
                {
                    GameGradeDuiGou[0].enabled = true;
                    GameGradeDuiGou[1].enabled = false;
                    GameGradeDuiGou[2].enabled = false;
                    ReadGameInfo.GetInstance().WriteGrade(1);
                    break;
                }
            case SetEnum.Grade2:
                {
                    GameGradeDuiGou[0].enabled = false;
                    GameGradeDuiGou[1].enabled = true;
                    GameGradeDuiGou[2].enabled = false;
                    ReadGameInfo.GetInstance().WriteGrade(2);
                    break;
                }
            case SetEnum.Grade3:
                {
                    GameGradeDuiGou[0].enabled = false;
                    GameGradeDuiGou[1].enabled = false;
                    GameGradeDuiGou[2].enabled = true;
                    ReadGameInfo.GetInstance().WriteGrade(3);
                    break;
                }
            case SetEnum.Reset:
                {
                    ResetFactory();
                    break;
                }
            case SetEnum.LanguageCH:
            case SetEnum.LanguageEN:
                {
                    SetGameLanguage(enumSet);
                    break;
                }
            case SetEnum.Exit:
                {
                    XkGameCtrl.IsLoadingLevel = true;
                    Resources.UnloadUnusedAssets();
                    GC.Collect();
                    Application.LoadLevel(1);
                    break;
                }
        }
    }

    void InitGameLanguage()
    {
        GameTextType type = GlobalData.GetInstance().GetGameTextMode();
        LanguageDuiGou[0].enabled = type == GameTextType.Chinese ? true : false;
        LanguageDuiGou[1].enabled = !LanguageDuiGou[0].enabled;
    }
    
    void SetGameLanguage(SetEnum val)
    {
        switch (val)
        {
            case SetEnum.LanguageCH:
                {
                    ReadGameInfo.GetInstance().WriteGameLanguage((int)GameTextType.Chinese);
                    break;
                }
            case SetEnum.LanguageEN:
                {
                    ReadGameInfo.GetInstance().WriteGameLanguage((int)GameTextType.English);
                    break;
                }
        }
        LanguageDuiGou[0].enabled = val == SetEnum.LanguageCH ? true : false;
        LanguageDuiGou[1].enabled = !LanguageDuiGou[0].enabled;
    }

    void ResetGameLanguage()
    {
        LanguageDuiGou[0].enabled = true;
        LanguageDuiGou[1].enabled = !LanguageDuiGou[0].enabled;
    }

	public UILabel GameAudioVolumeLB;
	void ResetFactory()
	{
		ReadGameInfo.GetInstance().FactoryReset();
		m_CoinForStar.text = "2";

		m_GameModeDuigou1.enabled = true;
		m_GameModeDuigou2.enabled = false;

		GameAudioVolume = 7;
		GameAudioVolumeLB.text = GameAudioVolume.ToString();

		if (pcvr.bIsHardWare)
        {
			pcvr.GetInstance().mPcvrTXManage.SubPlayerCoin(m_InserNum, pcvrTXManage.PlayerCoinEnum.player01);
		}
		m_InserNum = 0;
		UpdateInsertCoin();

        CaiPiaoInfoLb.text = "30";

        ChuPiaoLvDuiGou[0].enabled = false;
        ChuPiaoLvDuiGou[1].enabled = true;
        ChuPiaoLvDuiGou[2].enabled = false;

        ChuPiaoDuiGou[0].enabled = false;
        ChuPiaoDuiGou[1].enabled = true;

        GameGradeDuiGou[0].enabled = false;
        GameGradeDuiGou[1].enabled = true;
        GameGradeDuiGou[2].enabled = false;
        ResetGameLanguage();
    }
}