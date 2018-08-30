using UnityEngine;
using System.Collections;
using System;

public class UIController : SSGameMono
{
    /// <summary>
    /// 打印彩票UI预制.
    /// </summary>
    public GameObject m_PrintCardUIPrefab;
    /// <summary>
    /// UI摄像机.
    /// </summary>
    public Camera m_UICamera;
    /// <summary>
    /// 通过声音控制led.
    /// </summary>
    [HideInInspector]
    public SSLedByAudioCtrl mLedAudioScript;
    /// <summary>
    /// 积分动画控制.
    /// </summary>
    public Animator JiFenAni;
    /// <summary>
    /// 进度条船图标.
    /// ChuanTuBiaoImgArray[x] -> 0 猪猪侠, 1 波比, 2 超人强, 3 菲菲.
    /// </summary>
    public Texture[] ChuanTuBiaoImgArray;
    /// <summary>
    /// 游戏结束时图片数据.
    /// </summary>
     [Serializable]
    public class GameOverImgData
    {
        /// <summary>
        /// 游戏结束图片.
        /// </summary>
        public Texture GameOverImg;
        /// <summary>
        /// 到达终点图片.
        /// </summary>
        public Texture FinishImg;
        /// <summary>
        /// 创记录图片.
        /// </summary>
        public Texture CongratulationImg;
    }

    /// <summary>
    /// 游戏结束时图片数据列表.
    /// GameOverImgDtArray[x] -> 0 猪猪侠, 1 波比, 2 超人强, 3 菲菲.
    /// </summary>
    public GameOverImgData[] GameOverImgDtArray;
    public GameOverImgData[] GameOverImgDtArray_En;

    /// <summary>
    /// 游戏结束图片.
    /// </summary>
    public UITexture GameOverBkUI;
    /// <summary>
    /// 到达终点图片.
    /// </summary>
    public UITexture FinishBkUI;
    /// <summary>
    /// 创记录图片.
    /// </summary>
    public UITexture CongratulationBkUI;

    /// <summary>
    /// 排行榜数据UI.
    /// </summary>
    public RankListUICtrl mRankListUI;
    /// <summary>
    /// 游戏结束时要关闭的对象.
    /// </summary>
    public GameObject[] HiddenObjArray;
    /// <summary>
    /// 结算积分对象.
    /// </summary>
    public GameObject JieSuanJiFenObj;
    /// <summary>
    /// 结算积分图集列表.
    /// </summary>
    public UISprite[] JieSuanJiFenSpriteArray;
    /// <summary>
    /// 积分图集列表.
    /// </summary>
    public UISprite[] JiFenSpriteArray;
    [HideInInspector]
    public float m_pGameTime = 300.0f;
	public UISprite m_pMiaoBaiwei;
	public UISprite m_pMiaoshiwei;
	public UISprite m_pMiaogewei;
	public UISprite m_pMiaobiaozhi;
	public UIAtlas m_pNormalAtlas;
	public UIAtlas m_pWarnAtlas;
	public TweenScale m_pScale;
	private bool m_pHasChange = false;
	public PlayerController m_Player;
	public GameObject m_CongratulateJiemian;
	public GameObject m_FinishiJiemian;
	public GameObject m_OverJiemian;
	public GameObject m_CongratulateZitiObj;
	public GameObject m_FinishiZitiObj;
	public GameObject m_OverZitiObj;
	public GameObject m_JiluObj;
	public GameObject m_JindutiaoObj;
	public GameObject m_daojishiObj;
	public GameObject m_biaodituObj;
	private float m_CongratulateTimmer = 0.0f;
	private bool m_IsCongratulate = false;
	public CameraShake m_CameraShake;
	private bool m_HasShake = false;
	public bool m_IsGameOver = false;
	private int m_Score = 0;
	private int m_totalTime = 0;
	private int m_JiluRecord = 0;
	
	public UISprite m_ScoreFenGewei;
	public UISprite m_ScoreMiaoShiwei;
	public UISprite m_ScoreMiaoGewei;
	public UISprite m_RecordFenGewei;
	public UISprite m_RecordMiaoShiwei;
	public UISprite m_RecordMiaoGewei;
    [HideInInspector]
	public float Distance = 6400;
	public UISprite m_JinduTiao;
	public Transform m_ChuanTuBiao;

	public UITexture m_BeginDaojishi;
	//public Texture[] m_BeginDaojishiTexture;

	//youmentishi
	public UITexture m_YoumenTishi;
	public Texture[] m_YoumenTishiTexture;
	private float m_YoumenTimmer = 0.0f;
	public AudioSource m_GameOverAudio;
	public AudioSource m_FinishiAudio;
	public AudioSource m_NewRecordAudio;
	public AudioSource m_NewRecordHitAudio;
	private bool m_HasTishi = false;
	public AudioSource m_BeginDaojishiAudio;
	private bool m_HasPlay = false;

	void Start ()
    {
        chile = 0;
		m_pScale.enabled = false;

        if (SSGameDataCtrl.GetInstance() != null)
        {
            Debug.Log("fix game ui data...");
            m_pGameTime = SSGameDataCtrl.GetInstance().m_UIData.m_pGameTime;
            Distance = SSGameDataCtrl.GetInstance().m_UIData.Distance;
        }
        else
        {
            Debug.LogWarning("not find SSGameDataCtrl...");
        }

        int gradeVal = ReadGameInfo.GetInstance().ReadGrade();
        switch (gradeVal)
        {
            case 1: //低.
                {
                    m_pGameTime += 15f;
                    break;
                }
            case 2: //中.
                {
                    break;
                }
            case 3: //高.
                {
                    m_pGameTime -= 10f;
                    break;
                }
            default:
                {
                    break;
                }
        }

        m_pGameTime += 1;
		m_totalTime = (int)m_pGameTime;
		XkGameCtrl.IsLoadingLevel = false;
        ShowJiFenInfo(0);
		UpdateGameTime();
        InputEventCtrl.GetInstance().OnCaiPiaJiChuPiaoEvent += OnCaiPiaJiChuPiaoEvent;
        InputEventCtrl.GetInstance().OnCaiPiaJiWuPiaoEvent += OnCaiPiaJiWuPiaoEvent;
    }

    void OnCaiPiaJiWuPiaoEvent(pcvrTXManage.CaiPiaoJi val)
    {
        Debug.Log(val + ":: CaiPiaoJi wuPiao!");
        if (m_PrintCardCom != null)
        {
            m_PrintCardCom.ShowCardEmpty();
        }
    }

    void OnCaiPiaJiChuPiaoEvent(pcvrTXManage.CaiPiaoJi val)
    {
        GlobalData.GetInstance().CaiPiaoCur--;
        Debug.Log(val + ":: CaiPiaoCur == " + GlobalData.GetInstance().CaiPiaoCur);
        if (m_PrintCardCom != null)
        {
            m_PrintCardCom.ShowGameCardInfo(GlobalData.GetInstance().CaiPiaoCur);
        }
    }

    bool IsCloseYouMenTiShi;
	/*void ClickPlayerYouMenBtEvent(ButtonState val)
	{
		if (val == ButtonState.UP) {
			return;
		}
		IsCloseYouMenTiShi = true;
	}*/

	void Update ()
	{
		if(PlayerController.GetInstance().timmerstar < 5.0f)
		{
            UpdateBeginDaojishi();
		}
		else
		{
			if(m_BeginDaojishi.enabled)
			{
				m_BeginDaojishi.enabled = false;
				m_BeginDaojishiAudio.Stop();
				
				if (pcvr.bIsHardWare)
				{
					pcvr.GetInstance().mPcvrTXManage.SetJiDianQiCmd(0, pcvrTXManage.JiDianQiCmd.Open);
					//pcvr.GetInstance().mPcvrTXManage.SetJiDianQiCmd(0, pcvrTXManage.JiDianQiCmd.Close); //test 为了安全暂时不打开继电器(即摇摇机).
				}
			}

            if (Mathf.Abs(pcvr.GetInstance().mGetSteer) > 0f && !IsCloseYouMenTiShi)
            {
                //关闭方向盘提示.
                IsCloseYouMenTiShi = true;
                m_HasTishi = true;
                m_YoumenTishi.enabled = false;
                m_YoumenTimmer = 0.0f;
            }

            if (IsCloseYouMenTiShi && m_HasTishi)
            {
                if (Mathf.Abs(pcvr.GetInstance().mGetSteer) == 0f)
                {
                    m_YoumenTimmer += Time.deltaTime;
                    if (m_YoumenTimmer >= 5f)
                    {
                        //打开方向盘提示.
                        IsCloseYouMenTiShi = false;
                        m_HasTishi = false;
                    }
                }
                else
                {
                    m_YoumenTimmer = 0.0f;
                }
            }

            if (!IsCloseYouMenTiShi && !m_HasTishi)
			{
				m_YoumenTishi.enabled = true;
				UpdateYoumenTishi();
			}

			if(m_pGameTime>=0.0f && !m_Player.m_IsFinished)
			{
				UpdateJinduTiao();
				UpdateGameTime();
			}
			else
			{
				if(m_pGameTime<=0.0f)
				{
					m_IsGameOver = true;
					TouBiInfoCtrl.IsCloseQiNang = true;
                    PlayerController.GetInstance().SortPlayerRankList();
				}
				m_pScale.enabled = false;
			}
			if(m_Player.m_timmerFinished>2.0f && !m_IsCongratulate)
			{
				if(m_Player.m_IsFinished)
				{
					m_Score =  (int)(m_totalTime + chile * addChiLe - m_pGameTime);
					Debug.Log("UIController -> m_Score " + m_Score);
					m_JiluRecord = ReadGameInfo.GetInstance().ReadGameRecord();
					if(m_JiluRecord == 0 || m_Score < m_JiluRecord)
					{
						if(!m_NewRecordAudio.isPlaying)
							m_NewRecordAudio.Play();
						m_CongratulateJiemian.SetActive(true);
						ReadGameInfo.GetInstance().WriteGameRecord(m_Score);
					}
					else
					{
						if(!m_FinishiAudio.isPlaying)
							m_FinishiAudio.Play();
						m_FinishiJiemian.SetActive(true);
					}
					m_JiluObj.SetActive(true);
					UpdateMyScore();
					UpdateRecord();
				}
				else
				{
					if(!m_GameOverAudio.isPlaying)
						m_GameOverAudio.Play();
					m_OverJiemian.SetActive(true);
				}
				m_IsCongratulate = true;
				m_JindutiaoObj.SetActive(false);
				m_daojishiObj.SetActive(false);
				m_biaodituObj.SetActive(false);
                HiddenJiFen();
                HiddenUi();
                JieSuanJiFenObj.SetActive(true);
                mRankListUI.ShowRankListUI();

                GlobalData.GetInstance().CaiPiaoCur = 0;
                if (ReadGameInfo.GetInstance().ReadGameIsPrintCaiPiao() && ReadGameInfo.GetInstance().ReadGameStarMode() == ReadGameInfo.GameMode.Oper.ToString())
                {
                    //打印彩票.
                    int coinToCard = ReadGameInfo.GetInstance().ReadGamePrintCaiPiaoNum();
                    int maxScore = SSGameDataCtrl.GetInstance().m_UIData.MaxScore;
                    int scoreToCard = maxScore / coinToCard;
					int caiPiaoNum = m_JiFenPlayer / scoreToCard;
                    int chuPiaoLv = ReadGameInfo.GetInstance().ReadChuPiaoLv();
                    caiPiaoNum = (int)(caiPiaoNum * (chuPiaoLv / 100f));
                    if (caiPiaoNum <= 0)
                    {
                        caiPiaoNum = 1;
                    }
                    GlobalData.GetInstance().CaiPiaoCur = caiPiaoNum;
					Debug.Log("should print " + caiPiaoNum + " cards, JiFenPlayer == " + m_JiFenPlayer);
                    if (pcvr.bIsHardWare)
                    {
                        pcvr.GetInstance().mPcvrTXManage.SetCaiPiaoPrintCmd(pcvrTXManage.CaiPiaoPrintCmd.BanPiaoPrint, pcvrTXManage.CaiPiaoJi.Num01, caiPiaoNum);
                    }
                    else
                    {
                        //软件版本不用出彩票.
                        //GlobalData.GetInstance().CaiPiaoCur = 12;
                    }
                }

                if (GlobalData.GetInstance().CaiPiaoCur > 0 && !IsCreatPrintCardUI)
                {
                    //产生出彩票UI界面.
                    IsCreatPrintCardUI = true;
                    SpawnPrintCardUI();
                }

                //开启芯片加密校验.
                if (pcvr.bIsHardWare)
                {
                    //pcvr.GetInstance().mPcvrTXManage.GamePlayCount = 500; //test
                    pcvr.GetInstance().mPcvrTXManage.GameJiaoYanJiaMiXinPian();
                    pcvr.GetInstance().mPcvrTXManage.SetJiDianQiCmd(0, pcvrTXManage.JiDianQiCmd.Close);
                }
            }

			if(m_IsCongratulate)
			{
				m_CongratulateTimmer+=Time.deltaTime;
			}

			if(m_CongratulateTimmer > 1.0f)
			{
				if(m_Player.m_IsFinished)
				{
					if(m_Score < m_JiluRecord || m_JiluRecord == 0)
					{
						if(!m_NewRecordHitAudio.isPlaying && !m_HasPlay)
						{
							m_HasPlay = true;
							m_NewRecordHitAudio.Play();
						}
							
						m_CongratulateZitiObj.SetActive(true);
					}
					else
					{
						m_FinishiZitiObj.SetActive(true);
					}
				}
				else
				{
					m_OverZitiObj.SetActive(true);
				}
			}

			if(m_Player.m_IsFinished && m_CongratulateTimmer>1.2f && !m_HasShake)
			{
				m_HasShake = true;
				m_CameraShake.setCameraShakeImpulseValue();
			}

			if(m_CongratulateTimmer > 5.0f && !IsCheckLoadingMovieLevel)
			{
                IsCheckLoadingMovieLevel = true;
                StartCoroutine(CheckLoadingMovieLevel());
			}
		}
	}

    bool IsCreatPrintCardUI = false;
    SSPrintCardCtrl m_PrintCardCom = null;
    /// <summary>
    /// 产生打印彩票UI界面.
    /// </summary>
    void SpawnPrintCardUI()
    {
        Debug.Log("SpawnPrintCardUI...");
        GameObject obj = (GameObject)Instantiate(m_PrintCardUIPrefab, m_UICamera.transform);
        m_PrintCardCom = obj.GetComponent<SSPrintCardCtrl>();
        m_PrintCardCom.Init(GlobalData.GetInstance().CaiPiaoCur);
    }

    public void HiddenCardPrintUI()
    {
        if (m_PrintCardCom != null)
        {
            Debug.Log("HiddenCardPrintUI...");
            GlobalData.GetInstance().CaiPiaoCur = 0;
            m_PrintCardCom.HiddenCardPrintUI();
        }
    }

    bool IsCheckLoadingMovieLevel = false;
    IEnumerator CheckLoadingMovieLevel()
    {
		int count = 0;
        do
        {
			count++;
			if (!pcvr.bIsHardWare && count >= 10)
			{
				GlobalData.GetInstance().CaiPiaoCur = 0;
			}
            yield return new WaitForSeconds(0.5f);
        } while (GlobalData.GetInstance().CaiPiaoCur > 0);

        //MyCOMDevice.GetInstance().ForceRestartComPort();
        if (!XkGameCtrl.IsLoadingLevel)
        {
            XkGameCtrl.IsLoadingLevel = true;
            LoadMovieLevel();
        }
    }

	bool IsLoadMovie;
	void LoadMovieLevel()
	{
		if (IsLoadMovie) {
			return;		
		}
		IsLoadMovie = true;

		pcvr.GetInstance().OnGameOverCheckJingRuiJiaMi();
		StartCoroutine(CheckUnloadUnusedAssets());
	}

	IEnumerator CheckUnloadUnusedAssets()
	{
		bool isLoop = true;
		GC.Collect();
		AsyncOperation asyncVal = Resources.UnloadUnusedAssets();
		float timeLast = Time.realtimeSinceStartup;
		yield return new WaitForSeconds(2.5f);

		do
		{
			if (GameRoot.m_VerifyEnvironmentObj != null)
			{
				//精锐加密校验未结束.
				yield return new WaitForSeconds(0.5f);
				continue;
			}

			yield return new WaitForSeconds(0.5f);
			if (Time.realtimeSinceStartup - timeLast > 5f) {
				isLoop = false;
				XkGameCtrl.IsLoadingLevel = true;
				//Debug.Log("CheckUnloadUnusedAssets -> loading movie level, asyncVal.isDone "+asyncVal.isDone);
				Application.LoadLevel(1);
				yield break;
			}

			if (!asyncVal.isDone) {
				yield return new WaitForSeconds(0.5f);
			}
			else {
				isLoop = false;
				XkGameCtrl.IsLoadingLevel = true;
				Application.LoadLevel(1);
				yield break;
			}
		} while (isLoop);
	}

	void UpdateGameTime()
	{
		m_pGameTime -= Time.deltaTime;
		int TimeMiaoBaiwei = (int)(m_pGameTime / 100);
		int TimeMiaoshiwei = (int)((m_pGameTime - TimeMiaoBaiwei*100)/10);
		int TimeMiaogewei = (int)(m_pGameTime - TimeMiaoBaiwei*100 - TimeMiaoshiwei*10);
		if(m_pGameTime <= 11.0f && !m_pHasChange)
		{
			m_pScale.enabled = true;
			m_pMiaoBaiwei.atlas = m_pWarnAtlas;
			m_pMiaoshiwei.atlas = m_pWarnAtlas;
			m_pMiaogewei.atlas = m_pWarnAtlas;
			m_pMiaobiaozhi.atlas = m_pWarnAtlas;
			m_pHasChange = true;
		}
		m_pMiaoBaiwei.spriteName = TimeMiaoBaiwei.ToString();
		m_pMiaoshiwei.spriteName = TimeMiaoshiwei.ToString();
		m_pMiaogewei.spriteName = TimeMiaogewei.ToString();
	}


	void UpdateMyScore()
	{
		int fen = m_Score/60;
		if(fen > 0)
		{
			m_ScoreFenGewei.spriteName = fen.ToString();
		}
		else
		{
			m_ScoreFenGewei.spriteName = "0";
		}
		int miao = m_Score - fen*60;
		int miaoshiwei = miao/10;
		int miaogewei = miao - miaoshiwei*10;
		m_ScoreMiaoShiwei.spriteName = miaoshiwei.ToString();
		m_ScoreMiaoGewei.spriteName = miaogewei.ToString();
	}


	void UpdateRecord()
	{
		int fen = m_JiluRecord/60;
		if(fen > 0)
		{
			m_RecordFenGewei.spriteName = fen.ToString();
		}
		else
		{
			m_RecordFenGewei.spriteName = "0";
		}
		int miao = m_JiluRecord - fen*60;
		int miaoshiwei = miao/10;
		int miaogewei = miao - miaoshiwei*10;
		m_RecordMiaoShiwei.spriteName = miaoshiwei.ToString();
		m_RecordMiaoGewei.spriteName = miaogewei.ToString();
	}

	void UpdateJinduTiao()
	{
		if (m_pGameTime <= 11f)
		{
			if (m_JinduTiao.spriteName != "jindu2")
			{
				m_JinduTiao.spriteName = "jindu2";
			}
		}

		m_JinduTiao.fillAmount = (m_Player.m_distance)/Distance;
		if(m_JinduTiao.fillAmount > 1.0f)
		{
			m_JinduTiao.fillAmount = 1.0f;
		}
		//m_ChuanTuBiao.localPosition = new Vector3(m_JinduTiao.fillAmount *(355+375.0f)-375.0f ,-25.0f,0.0f);
	}

	void UpdateBeginDaojishi()
	{
		if(!m_BeginDaojishi.enabled)
		{
			m_BeginDaojishi.enabled = true;
		}
        if (!m_BeginDaojishiAudio.isPlaying)
        {
            m_BeginDaojishiAudio.Play();
        }

//		int index = (int)(6.0f - PlayerController.GetInstance().timmerstar);
//		if(index>=6)
//		{
//			index = 5;
//		}
		//m_BeginDaojishi.mainTexture = m_BeginDaojishiTexture[0];
	}

	void UpdateYoumenTishi()
	{
		m_YoumenTimmer+=Time.deltaTime;
		if(m_YoumenTimmer<0.3f)
		{
			m_YoumenTishi.mainTexture =  m_YoumenTishiTexture[0];
		}
		else if(m_YoumenTimmer>=0.3f &&m_YoumenTimmer<0.6f)
		{
			m_YoumenTishi.mainTexture =  m_YoumenTishiTexture[1];
		}
		else if(m_YoumenTimmer>=0.6f &&m_YoumenTimmer<0.9f)
		{
			m_YoumenTishi.mainTexture =  m_YoumenTishiTexture[2];
		}
		else if(m_YoumenTimmer>=0.9f &&m_YoumenTimmer<1.2f)
		{
			m_YoumenTishi.mainTexture =  m_YoumenTishiTexture[3];
		}
		else
		{
			m_YoumenTimmer = 0.0f;
		}
	}
	public GameObject m_JiashiTexture;
	public TweenScale m_Scale;
	public TweenPosition m_Position;
	private int chile = 0;
	private float addChiLe = 5.0f;
	public void ResetJiashi()
	{
		m_JiashiTexture.transform.localPosition = new Vector3 (0.0f, 0.0f, 500.0f);
		m_JiashiTexture.transform.localScale = new Vector3 (1.0f,1.0f,1.0f);
		if (!m_IsGameOver) {
			m_pGameTime += addChiLe;
			chile ++;
		}
		//Debug.Log ("chileeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee " +chile);
		m_Scale.ResetToBeginning ();
		m_Position.ResetToBeginning ();
		m_Scale.enabled = true;
		m_Position.enabled = true;
		m_JiashiTexture.SetActive (false);

		if (m_DaoJiShiTr != null)
		{
			m_DaoJiShiTr.localScale = DaoJiShiMaxScale;
			Invoke("ResetDaoJiShiScale", 0.2f);
		}
	}

	public Transform m_DaoJiShiTr;
	public Vector3 DaoJiShiMaxScale = new Vector3(2f, 2f, 1f);
	void ResetDaoJiShiScale()
	{
		m_DaoJiShiTr.localScale = new Vector3(1f, 1f, 1f);
	}

	int m_JiFenPlayer = 0;
    /// <summary>
    /// 显示玩家积分.
    /// </summary>
    public void ShowJiFenInfo(int jiFen)
    {
		m_JiFenPlayer = jiFen;
        if (jiFen != 0)
        {
            JiFenAni.SetTrigger("IsTrigger");
		}
		mRankListUI.ShowJiFenInfo(jiFen);

        int jiFenTmp = 0;
        string jiFenStr = jiFen.ToString();
        for (int i = 0; i < 6; i++)
        {
            if (jiFenStr.Length > i)
            {
                jiFenTmp = jiFen % 10;
                JiFenSpriteArray[i].spriteName = jiFenTmp.ToString();
                JieSuanJiFenSpriteArray[i].spriteName = jiFenTmp.ToString();
                jiFen = (int)(jiFen / 10f);
                JiFenSpriteArray[i].enabled = true;
                JieSuanJiFenSpriteArray[i].enabled = true;
            }
            else
            {
                JiFenSpriteArray[i].enabled = false;
                JieSuanJiFenSpriteArray[i].enabled = false;
            }
        }
    }

    void HiddenJiFen()
    {
        for (int i = 0; i < JiFenSpriteArray.Length; i++)
        {
            JiFenSpriteArray[i].gameObject.SetActive(false);
        }
    }

    void HiddenUi()
    {
        for (int i = 0; i < HiddenObjArray.Length; i++)
        {
            HiddenObjArray[i].SetActive(false);
        }
    }

    /// <summary>
    /// 设置游戏结束时大背景的图片信息.
    /// </summary>
    public void SetGameOverUIDt(RankManage.RankEnum indexRank)
    {
        Debug.Log("SetGameOverUIDt -> indexRank " + indexRank);
        int indexVal = (int)indexRank;
        if (GlobalData.GetInstance().GetGameTextMode() == GameTextType.English)
        {
            GameOverBkUI.mainTexture = GameOverImgDtArray_En[indexVal].GameOverImg;
            FinishBkUI.mainTexture = GameOverImgDtArray_En[indexVal].FinishImg;
            CongratulationBkUI.mainTexture = GameOverImgDtArray_En[indexVal].CongratulationImg;
        }
        else
        {
            GameOverBkUI.mainTexture = GameOverImgDtArray[indexVal].GameOverImg;
            FinishBkUI.mainTexture = GameOverImgDtArray[indexVal].FinishImg;
            CongratulationBkUI.mainTexture = GameOverImgDtArray[indexVal].CongratulationImg;
        }
    }

    /// <summary>
    /// 设置进度条船图标.
    /// </summary>
    /// <param name="indexRank"></param>
    public void SetChuanTuBiaoImg(RankManage.RankEnum indexRank)
    {
        int indexVal = (int)indexRank;
        UITexture uiTextureCom = m_ChuanTuBiao.GetComponent<UITexture>();
        uiTextureCom.mainTexture = ChuanTuBiaoImgArray[indexVal];
    }
}