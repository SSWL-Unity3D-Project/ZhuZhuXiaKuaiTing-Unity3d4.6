#define USE_HANDLE_JSON
using UnityEngine;
using System.Collections;
using System;

public class ReadGameInfo : MonoBehaviour 
{
	static private ReadGameInfo Instance = null;
	private HandleJson handleJsonObj;
	public string m_pStarCoinNum = "";
	public string m_pGameMode = "";
	public string m_pInsertCoinNum = "0";
	int GameRecordVal;
	int PlayerMinSpeedVal = 0;
	/**
	 * 游戏音量(0-10).
	 */
	int GameAudioVolume;
    /// <summary>
    /// 是否出彩票.
    /// </summary>
    public bool IsPrintCaiPiao = false;
    /// <summary>
    /// 彩票数量(1局游戏可以出多少彩票) CaiPiaoNum = [1, 10].
    /// </summary>
    public int CaiPiaoNum = 1;
#if USE_HANDLE_JSON
    static HandleJson PHandleJson;
	static string FileName = "SSGameConfig.xml";
	#endif
	static public ReadGameInfo GetInstance()
	{
		if (Instance == null) {
			#if USE_HANDLE_JSON
			PHandleJson = HandleJson.GetInstance();
			#endif
			GameObject obj = new GameObject("_ReadGameInfo");
			DontDestroyOnLoad(obj);
			Instance = obj.AddComponent<ReadGameInfo>();
			Instance.InitGameInfo();
		}
		return Instance;
	}
	void InitGameInfo()
	{
		m_pInsertCoinNum = "0";
		
		int gameModeSt = PlayerPrefs.GetInt("GAME_MODE");
		if (gameModeSt != 0 && gameModeSt != 1) {
			gameModeSt = 1; //0->运营模式, 1->免费模式.
			PlayerPrefs.SetInt("GAME_MODE", gameModeSt);
		}
		m_pGameMode = gameModeSt == 0 ? "oper" : "FREE";

		int coinStart = PlayerPrefs.GetInt("START_COIN");
		if (coinStart == 0) {
			coinStart = 1;
			PlayerPrefs.SetInt("START_COIN", coinStart);
		}
		m_pStarCoinNum = coinStart.ToString();

		GameRecordVal = PlayerPrefs.GetInt("GAME_RECORD");
		
		int value = PlayerPrefs.GetInt("PlayerMinSpeedVal");
		if (value < 0) {
			value = 0;
		}
		PlayerMinSpeedVal = value;

		if (!PlayerPrefs.HasKey("GameAudioVolume")) {
			PlayerPrefs.SetInt("GameAudioVolume", 7);
			PlayerPrefs.Save();
		}
		
#if USE_HANDLE_JSON
        //游戏音量信息.
		string readInfo = PHandleJson.ReadFromFileXml(FileName, "GameAudioVolume");
		if (readInfo == null || readInfo == "") {
			readInfo = "7";
			PHandleJson.WriteToFileXml(FileName, "GameAudioVolume", readInfo);
		}

		value = Convert.ToInt32(readInfo);
		if (value < 0 || value > 10) {
			value = 7;
			PHandleJson.WriteToFileXml(FileName, "GameAudioVolume", value.ToString());
        }
        GameAudioVolume = value;

        //游戏出彩票信息.
        readInfo = PHandleJson.ReadFromFileXml(FileName, "IsPrintCaiPiao");
        if (readInfo == null || readInfo == "")
        {
            readInfo = "1";
            PHandleJson.WriteToFileXml(FileName, "IsPrintCaiPiao", readInfo);
        }

        value = Convert.ToInt32(readInfo);
        if (value == 1)
        {
            IsPrintCaiPiao = true;
        }
        else
        {
            IsPrintCaiPiao = false;
        }

        readInfo = PHandleJson.ReadFromFileXml(FileName, "CaiPiaoNum");
        if (readInfo == null || readInfo == "")
        {
            readInfo = "5";
            PHandleJson.WriteToFileXml(FileName, "CaiPiaoNum", readInfo);
        }

        value = Convert.ToInt32(readInfo);
        if (value < 0)
        {
            value = 5;
            PHandleJson.WriteToFileXml(FileName, "CaiPiaoNum", value.ToString());
        }
        CaiPiaoNum = value;
#else
		value = PlayerPrefs.GetInt("GameAudioVolume");
		if (value < 0 || value > 10) {
			value = 7;
			PlayerPrefs.SetInt("GameAudioVolume", value);
			PlayerPrefs.Save();
		}
        GameAudioVolume = value;
#endif
    }
    public void FactoryReset()
	{
		WriteStarCoinNumSet("1");
		WriteGameStarMode("oper");
		WriteInsertCoinNum("0");
		WriteGameRecord(180);
		WritePlayerMinSpeedVal(0);
		WriteGameAudioVolume(7);
        WriteGameIsPrintCaiPiao(true);
        WriteGamePrintCaiPiaoNum(5);
    }

    /// <summary>
    /// 读取游戏一局可以出多少彩票.
    /// </summary>
    public int ReadGamePrintCaiPiaoNum()
    {
        return CaiPiaoNum;
    }

    /// <summary>
    /// 修改游戏一局可以出多少彩票.
    /// </summary>
    public void WriteGamePrintCaiPiaoNum(int num)
    {
        int value = num;
        CaiPiaoNum = value;
        PHandleJson.WriteToFileXml(FileName, "CaiPiaoNum", value.ToString());
    }

    /// <summary>
    /// 读取游戏是否可以出彩票.
    /// </summary>
    public bool ReadGameIsPrintCaiPiao()
    {
        return IsPrintCaiPiao;
    }

    /// <summary>
    /// 修改游戏是否可以出彩票.
    /// </summary>
    public void WriteGameIsPrintCaiPiao(bool isPrint)
    {
        int value = isPrint == true ? 1 : 0;
        IsPrintCaiPiao = isPrint;
        PHandleJson.WriteToFileXml(FileName, "IsPrintCaiPiao", value.ToString());
    }

    public int ReadGameAudioVolume()
	{
		return GameAudioVolume;
	}
	public void WriteGameAudioVolume(int value)
	{
		#if USE_HANDLE_JSON
		PHandleJson.WriteToFileXml(FileName, "GameAudioVolume", value.ToString());
		#else
		PlayerPrefs.SetInt("GameAudioVolume", value);
		PlayerPrefs.Save();
		#endif
		GameAudioVolume = value;
		AudioListener.volume = (float)value / 10f;
	}
	public string ReadStarCoinNumSet()
	{
		return m_pStarCoinNum;
	}
	public string ReadGameStarMode()
	{
		return m_pGameMode;
	}
	public string ReadInsertCoinNum()
	{
		return m_pInsertCoinNum;
	}
	public int ReadGameRecord()
	{
		return GameRecordVal;
	}
	public void WriteStarCoinNumSet(string value)
	{
		int coinStart = Convert.ToInt32(value);
		PlayerPrefs.SetInt("START_COIN", coinStart);
		m_pStarCoinNum = coinStart.ToString();
	}
	public void WriteGameStarMode(string value)
	{
		int gameModeSt = value == "oper" ? 0 : 1;
		PlayerPrefs.SetInt("GAME_MODE", gameModeSt);
		m_pGameMode = value;
	}
	public void WriteInsertCoinNum(string value)
	{
		m_pInsertCoinNum = value;
	}
	public void WriteGameRecord(int value)
	{
		PlayerPrefs.SetInt("GAME_RECORD", value);
		GameRecordVal = value;
	}
	public int ReadPlayerMinSpeedVal()
	{
		return PlayerMinSpeedVal;
	}
	public void WritePlayerMinSpeedVal(int value)
	{
		PlayerPrefs.SetInt("PlayerMinSpeedVal", value);
		PlayerMinSpeedVal = value;
	}
//	void OnGUI()
//	{
//		string strA = "StarCoinNum "+m_pStarCoinNum
//						+", GameMode "+m_pGameMode;
//		GUI.Box(new Rect(0f, 0f, Screen.width, 30f), strA);
//	}
}