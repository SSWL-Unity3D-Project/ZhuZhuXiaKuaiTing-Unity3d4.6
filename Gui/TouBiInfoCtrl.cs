using System;
using UnityEngine;
using System.Collections;

public class TouBiInfoCtrl : MonoBehaviour {

	public UISprite CoinNumSetTex;
	public UISprite m_InsertNumS;
	public UISprite m_InsertNumG;
	public AudioSource m_TbSource;
	public GameObject FreeObj;
	public GameObject TouBiObj;

	private int m_InserNum = 0;
	private string CoinNumSet = "";
	private string InsertCoinNum = "";
	private string GameMode = "";
	public static bool IsCloseDongGan;
	public static bool IsCloseQiNang;

	// Use this for initialization
	void Start()
	{
		IsCloseQiNang = false;
		IsCloseDongGan = false;
		GameMode = ReadGameInfo.GetInstance ().ReadGameStarMode();
		if(GameMode == ReadGameInfo.GameMode.Oper.ToString())
		{
			CoinNumSet = ReadGameInfo.GetInstance ().ReadStarCoinNumSet();
			InsertCoinNum = ReadGameInfo.GetInstance ().ReadInsertCoinNum();
			CoinNumSetTex.spriteName = CoinNumSet;
			m_InserNum = Convert.ToInt32(InsertCoinNum);
			UpdateInsertCoin();

			TouBiObj.SetActive(true);
			FreeObj.SetActive(false);
		}
		else {
			TouBiObj.SetActive(false);
			FreeObj.SetActive(true);
		}
		
		InputEventCtrl.GetInstance().mListenPcInputEvent.ClickSetEnterBtEvent += ClickSetEnterBtEvent;
		InputEventCtrl.GetInstance().mListenPcInputEvent.ClickSetMoveBtEvent += ClickSetMoveBtEvent;
        InputEventCtrl.GetInstance().mListenPcInputEvent.ClickCloseDongGanBtEvent += ClickCloseDongGanBtEvent;
	}

    private void ClickSetMoveBtEvent(InputEventCtrl.ButtonState val)
    {
        if (val == InputEventCtrl.ButtonState.DOWN)
        {
            if (PlayerController.GetInstance() != null && PlayerController.GetInstance().m_UIController != null)
            {
                PlayerController.GetInstance().m_UIController.HiddenCardPrintUI();
            }
        }
    }

    // Update is called once per frame
    void Update()
	{
		if (pcvr.IsTestNoInput)
		{
			return;
		}

        if (pcvr.bIsHardWare)
		{
			if (GlobalData.GetInstance().CoinCur != m_InserNum && GameMode == ReadGameInfo.GameMode.Oper.ToString())
            {
				m_InserNum = GlobalData.GetInstance().CoinCur - 1;
				OnClickInsertBt();
			}
		}
		else
		{
			if (Input.GetKeyDown(KeyCode.T) && GameMode == ReadGameInfo.GameMode.Oper.ToString())
			{
				OnClickInsertBt();
			}
		}
	}
	
	void ClickSetEnterBtEvent(InputEventCtrl.ButtonState val)
	{
		if (val == InputEventCtrl.ButtonState.DOWN) {
			return;
		}

        if (PlayerController.GetInstance() != null)
        {
            PlayerController.GetInstance().m_UIController.HiddenCardPrintUI();
        }
		XkGameCtrl.IsLoadingLevel = true;
		Resources.UnloadUnusedAssets();
		GC.Collect();
		Application.LoadLevel(6);
	}
    
	void ClickCloseDongGanBtEvent(InputEventCtrl.ButtonState val)
	{
		if (val == InputEventCtrl.ButtonState.UP)
        {
			return;
		}
		
		if (PlayerController.GetInstance().m_IsFinished
		    || PlayerController.GetInstance().m_UIController.m_IsGameOver)
		{
			return;
		}

		if (DongGanCtrl.GetInstance() == null)
		{
			return;
		}

//        if (IsCloseDongGan)
//        {
//            //动感已经关闭,不允许再次打开.
//            return;
//        }
		IsCloseDongGan = !IsCloseDongGan;
		HandleDongGanUI();

        if (pcvr.bIsHardWare)
        {
            if (IsCloseDongGan)
            {
                pcvr.GetInstance().mPcvrTXManage.SetJiDianQiCmd(0, pcvrTXManage.JiDianQiCmd.Close);
            }
            else
            {
                pcvr.GetInstance().mPcvrTXManage.SetJiDianQiCmd(0, pcvrTXManage.JiDianQiCmd.Open);
                //pcvr.GetInstance().mPcvrTXManage.SetJiDianQiCmd(0, pcvrTXManage.JiDianQiCmd.Close); //test 为了安全暂时不打开继电器(即摇摇机).
            }
        }
    }

	void HandleDongGanUI()
	{
		if (DongGanCtrl.GetInstance() == null) {
			return;
		}

		if (!IsCloseDongGan) {
			DongGanCtrl.GetInstance().ShowDongGanOpen();
		}
		else {
			DongGanCtrl.GetInstance().ShowDongGanClose();
		}
	}

	void OnClickInsertBt()
	{
		m_TbSource.Play();
		m_InserNum++;
		ReadGameInfo.GetInstance().WriteInsertCoinNum(m_InserNum.ToString());
		UpdateInsertCoin();
	}

	void UpdateInsertCoin()
	{
		int n = 1;
		int num = m_InserNum;
		int temp = num;
		while (num > 9) {
			num /= 10;
			n++;
		}

		if (n > 2) {
			m_InsertNumS.spriteName = "9";
			m_InsertNumG.spriteName = "9";
		}
		else if (n==2) {
			int shiwei = (int)(temp/10);
			int gewei = (int)(temp-shiwei*10);
			m_InsertNumS.spriteName = shiwei.ToString();
			m_InsertNumG.spriteName = gewei.ToString();
		}
		else if (n == 1) {
			m_InsertNumS.spriteName = "0";
			m_InsertNumG.spriteName = temp.ToString();
		}
	}
}
