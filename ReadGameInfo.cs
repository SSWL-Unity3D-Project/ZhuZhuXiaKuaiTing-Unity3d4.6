using UnityEngine;
using System;

public class ReadGameInfo : MonoBehaviour 
{
	static private ReadGameInfo Instance = null;
    public enum GameMode
    {
        Oper, //运营模式.
        Free, //免费模式.
    }

	private HandleJson handleJsonObj;
	string m_pStarCoinNum = "";
	string m_pGameMode = "";
	string m_pInsertCoinNum = "0";
    /// <summary>
    /// 游戏语言信息.
    /// GameLanguageVal == 0 -> 中文.
    /// GameLanguageVal == 1 -> 英文.
    /// </summary>
    int GameLanguageVal;
    /// <summary>
    /// 游戏最高记录.
    /// </summary>
    int GameRecordVal;
    /// <summary>
    /// 游戏难度.
    /// </summary>
    int mGrade = 1;
    /// <summary>
    /// 游戏音量(0-10).
    /// </summary>
    int GameAudioVolume;
    /// <summary>
    /// 是否出彩票.
    /// </summary>
    bool IsPrintCaiPiao = false;
    /// <summary>
    /// 彩票数量(1局游戏可以出多少彩票) CaiPiaoNum = [1, 10].
    /// </summary>
    int CaiPiaoNum = 1;
    /// <summary>
    /// 出票率.
    /// </summary>
    int ChuPiaoLv = 100;
    HandleJson mHandleJson;
	string mFileName = "SSGameConfig.xml";
	static public ReadGameInfo GetInstance()
	{
		if (Instance == null)
        {
			GameObject obj = new GameObject("_ReadGameInfo");
			DontDestroyOnLoad(obj);
			Instance = obj.AddComponent<ReadGameInfo>();
			Instance.InitGameInfo();
		}
		return Instance;
	}

	void InitGameInfo()
    {
        mHandleJson = HandleJson.GetInstance();
        m_pInsertCoinNum = "0";
		
        //游戏音量信息.
		string readInfo = mHandleJson.ReadFromFileXml(mFileName, "GameAudioVolume");
		if (readInfo == null || readInfo == "") {
			readInfo = "7";
			mHandleJson.WriteToFileXml(mFileName, "GameAudioVolume", readInfo);
		}

		int value = Convert.ToInt32(readInfo);
		if (value < 0 || value > 10) {
			value = 7;
			mHandleJson.WriteToFileXml(mFileName, "GameAudioVolume", value.ToString());
        }
        GameAudioVolume = value;

        //游戏出彩票信息.
        readInfo = mHandleJson.ReadFromFileXml(mFileName, "IsPrintCaiPiao");
        if (readInfo == null || readInfo == "")
        {
            readInfo = "0";
            mHandleJson.WriteToFileXml(mFileName, "IsPrintCaiPiao", readInfo);
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

        readInfo = mHandleJson.ReadFromFileXml(mFileName, "CaiPiaoNum");
        if (readInfo == null || readInfo == "")
        {
            readInfo = "30";
            mHandleJson.WriteToFileXml(mFileName, "CaiPiaoNum", readInfo);
        }

        value = Convert.ToInt32(readInfo);
        if (value < 0)
        {
            value = 30;
            mHandleJson.WriteToFileXml(mFileName, "CaiPiaoNum", value.ToString());
        }
        CaiPiaoNum = value;

        //出票率.
        readInfo = mHandleJson.ReadFromFileXml(mFileName, "ChuPiaoLv");
        if (readInfo == null || readInfo == "")
        {
            readInfo = "100";
            mHandleJson.WriteToFileXml(mFileName, "ChuPiaoLv", readInfo);
        }

        value = Convert.ToInt32(readInfo);
        if (value < 0)
        {
            value = 100;
            mHandleJson.WriteToFileXml(mFileName, "ChuPiaoLv", value.ToString());
        }
        ChuPiaoLv = value;

        //游戏难度.
        readInfo = mHandleJson.ReadFromFileXml(mFileName, "Grade");
        if (readInfo == null || readInfo == "")
        {
            readInfo = "2";
            mHandleJson.WriteToFileXml(mFileName, "Grade", readInfo);
        }

        value = Convert.ToInt32(readInfo);
        if (value < 1 || value > 3)
        {
            value = 2;
            mHandleJson.WriteToFileXml(mFileName, "Grade", value.ToString());
        }
        mGrade = value;
        
        //游戏运营模式.
        readInfo = mHandleJson.ReadFromFileXml(mFileName, "GAME_MODE");
        if (readInfo == null || readInfo == "")
        {
            readInfo = "0";
            mHandleJson.WriteToFileXml(mFileName, "GAME_MODE", readInfo);
        }

        value = Convert.ToInt32(readInfo);
        if (value != 0 && value != 1)
        {
            value = 0; //0->运营模式, 1->免费模式.
            mHandleJson.WriteToFileXml(mFileName, "GAME_MODE", value.ToString());
        }
        m_pGameMode = value == 0 ? GameMode.Oper.ToString() : GameMode.Free.ToString();

        //游戏启动币数.
        readInfo = mHandleJson.ReadFromFileXml(mFileName, "START_COIN");
        if (readInfo == null || readInfo == "")
        {
            readInfo = "2";
            mHandleJson.WriteToFileXml(mFileName, "START_COIN", readInfo);
        }

        value = Convert.ToInt32(readInfo);
        if (value < 0 || value > 10)
        {
            value = 2;
            mHandleJson.WriteToFileXml(mFileName, "START_COIN", value.ToString());
        }
        m_pStarCoinNum = value.ToString();
        
        //游戏最高记录.
        readInfo = mHandleJson.ReadFromFileXml(mFileName, "GAME_RECORD");
        if (readInfo == null || readInfo == "")
        {
            readInfo = "180";
            mHandleJson.WriteToFileXml(mFileName, "GAME_RECORD", readInfo);
        }

        value = Convert.ToInt32(readInfo);
        if (value < 0)
        {
            value = 180;
            mHandleJson.WriteToFileXml(mFileName, "GAME_RECORD", value.ToString());
        }
        GameRecordVal = value;

        //游戏语言信息.
        readInfo = mHandleJson.ReadFromFileXml(mFileName, "GAME_LANGUAGE");
        if (readInfo == null || readInfo == "")
        {
            readInfo = "0";
            mHandleJson.WriteToFileXml(mFileName, "GAME_LANGUAGE", readInfo);
        }

        value = Convert.ToInt32(readInfo);
        if (value < 0 || value > 1)
        {
            value = 0;
            mHandleJson.WriteToFileXml(mFileName, "GAME_LANGUAGE", value.ToString());
        }
        GameLanguageVal = value;
    }

    public void FactoryReset()
	{
		WriteStarCoinNumSet("2");
		WriteGameStarMode(GameMode.Oper.ToString());
		WriteInsertCoinNum("0");
		WriteGameRecord(180);
		WriteGameAudioVolume(7);

        WriteGameIsPrintCaiPiao(false);
        WriteGamePrintCaiPiaoNum(30);
        WriteChuPiaoLv(100);

        WriteGrade(2);
        WriteGameLanguage((int)GameTextType.Chinese);
    }

    /// <summary>
    /// 读取游戏难度.
    /// </summary>
    public int ReadGrade()
    {
        return mGrade;
    }

    /// <summary>
    /// 修改游戏难度.
    /// </summary>
    public void WriteGrade(int val)
    {
        mGrade = val;
        mHandleJson.WriteToFileXml(mFileName, "Grade", val.ToString());
    }

    /// <summary>
    /// 读取游戏一币可以出多少彩票.
    /// </summary>
    public int ReadGamePrintCaiPiaoNum()
    {
        return CaiPiaoNum;
    }

    /// <summary>
    /// 修改游戏一币可以出多少彩票.
    /// </summary>
    public void WriteGamePrintCaiPiaoNum(int val)
    {
        CaiPiaoNum = val;
        mHandleJson.WriteToFileXml(mFileName, "CaiPiaoNum", val.ToString());
    }

    /// <summary>
    /// 读取出票率.
    /// </summary>
    public int ReadChuPiaoLv()
    {
        return ChuPiaoLv;
    }

    /// <summary>
    /// 修改出票率.
    /// </summary>
    public void WriteChuPiaoLv(int val)
    {
        ChuPiaoLv = val;
        mHandleJson.WriteToFileXml(mFileName, "ChuPiaoLv", val.ToString());
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
        mHandleJson.WriteToFileXml(mFileName, "IsPrintCaiPiao", value.ToString());
    }

    public int ReadGameAudioVolume()
	{
		return GameAudioVolume;
	}
	public void WriteGameAudioVolume(int value)
	{
		mHandleJson.WriteToFileXml(mFileName, "GameAudioVolume", value.ToString());
		GameAudioVolume = value;
		AudioListener.volume = value / 10f;
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
    public int ReadGameLanguage()
    {
        return GameLanguageVal;
    }
    public void WriteStarCoinNumSet(string value)
	{
        mHandleJson.WriteToFileXml(mFileName, "START_COIN", value);
        m_pStarCoinNum = value;
	}
	public void WriteGameStarMode(string value)
	{
        mHandleJson.WriteToFileXml(mFileName, "GAME_MODE", value == GameMode.Oper.ToString() ? "0" : "1");
        m_pGameMode = value;
	}
	public void WriteInsertCoinNum(string value)
	{
		m_pInsertCoinNum = value;
	}
	public void WriteGameRecord(int value)
	{
        mHandleJson.WriteToFileXml(mFileName, "GAME_RECORD", value.ToString());
        GameRecordVal = value;
    }
    public void WriteGameLanguage(int value)
    {
        mHandleJson.WriteToFileXml(mFileName, "GAME_LANGUAGE", value.ToString());
        GameLanguageVal = value;
    }
}