using UnityEngine;
using System.Collections;
using System;
using System.Diagnostics;

public class HardWareTest : MonoBehaviour 
{
	public UILabel TouBiLabel;
	public UILabel AnJianLabel;
	public UILabel FangXiangLabel;
	public static bool IsTestHardWare;
	static HardWareTest Instance;
    public static HardWareTest GetInstance()
    {
        return Instance;
    }

    void Start ()
	{
		Instance = this;
		JiaMiJiaoYanCtrlObj.SetActive(IsJiaMiTest);
		IsTestHardWare = true;
		AnJianLabel.text = "";
		InputEventCtrl.GetInstance().ClickSetEnterBtEvent += ClickSetEnterBtEvent;
		InputEventCtrl.GetInstance().ClickSetMoveBtEvent += ClickSetMoveBtEvent;
		InputEventCtrl.GetInstance().ClickCloseDongGanBtEvent += ClickCloseDongGanBtEvent;
		//pcvr.GetInstance();
	}
	public UILabel BeiYongYouMenLabel;
    
    public void CheckReadComMsg(byte[] buffer)
    {
        UpdatePcvrInfo(buffer);
    }

    void UpdatePcvrInfo(byte[] buffer)
	{
		TouBiLabel.text = GlobalData.CoinCur.ToString();
        if (pcvr.bIsHardWare)
        {
            FangXiangLabel.text = buffer[30].ToString("X2");
        }
	}

    void ClickSetEnterBtEvent(InputEventCtrl.ButtonState val)
	{
		if (val == InputEventCtrl.ButtonState.DOWN) {
			AnJianLabel.text = "SetEnter Down 按键3（开始1）";
		}
		else {
			AnJianLabel.text = "SetEnter Up 按键3（开始1）";
		}
	}
	void ClickSetMoveBtEvent(InputEventCtrl.ButtonState val)
	{
		if (val == InputEventCtrl.ButtonState.DOWN) {
			AnJianLabel.text = "SetMove Down 按键4（开始2）";
		}
		else {
			AnJianLabel.text = "SetMove Up 按键4（开始2）";
		}
	}
	void ClickCloseDongGanBtEvent(InputEventCtrl.ButtonState val)
	{
		if (val == InputEventCtrl.ButtonState.DOWN) {
			AnJianLabel.text = "DongGanBt Down 按键5（开始3）";
		}
		else {
			AnJianLabel.text = "DongGanBt Up 按键5（开始3）";
		}
	}
	public void OnClickSubCoinBt()
	{
        pcvr.GetInstance().mPcvrTXManage.SubPlayerCoin(1, pcvrTXManage.PlayerCoinEnum.player01);
	}
	public void OnClickCloseAppBt()
	{
		Application.Quit();
	}
	
	public bool IsJiaMiTest = false;
	public GameObject JiaMiJiaoYanCtrlObj;
	public void OnClickRestartAppBt()
	{
		Application.Quit();
		RunCmd("start ComTest.exe");
	}
	void RunCmd(string command)
	{
		//實例一個Process類，啟動一個獨立進程    
		Process p = new Process();    //Process類有一個StartInfo屬性，這個是ProcessStartInfo類，    
		//包括了一些屬性和方法，下面我們用到了他的幾個屬性：   
		p.StartInfo.FileName = "cmd.exe";           //設定程序名   
		p.StartInfo.Arguments = "/c " + command;    //設定程式執行參數   
		p.StartInfo.UseShellExecute = false;        //關閉Shell的使用    p.StartInfo.RedirectStandardInput = true;   //重定向標準輸入    p.StartInfo.RedirectStandardOutput = true;  //重定向標準輸出   
		p.StartInfo.RedirectStandardError = true;   //重定向錯誤輸出    
		p.StartInfo.CreateNoWindow = true;          //設置不顯示窗口    
		p.Start();   //啟動
		
		//p.WaitForInputIdle();
		//MoveWindow(p.MainWindowHandle, 1000, 10, 300, 200, true);
		
		//p.StandardInput.WriteLine(command); //也可以用這種方式輸入要執行的命令    
		//p.StandardInput.WriteLine("exit");        //不過要記得加上Exit要不然下一行程式執行的時候會當機    return p.StandardOutput.ReadToEnd();        //從輸出流取得命令執行結果
	}

	public UILabel StartLightLabel;
	int LightStart = 1;
	public void OnClickStartLightBt()
	{
		LightStart++;
		//Debug.Log("**************LightStart "+LightStart);
		switch (LightStart) {
		case 1:
			StartLightLabel.text = "开始灯亮";
			//pcvr.StartBtLight = StartLightState.Liang;
			break;

		case 2:
			StartLightLabel.text = "开始灯闪";
			//pcvr.StartBtLight = StartLightState.Shan;
			break;

		case 3:
			StartLightLabel.text = "开始灯灭";
			//pcvr.StartBtLight = StartLightState.Mie;
			LightStart = 1;
			break;
		}
	}

	public UILabel JiaMiJYLabel;
	public UILabel JiaMiJYMsg;
	public static bool IsOpenJiaMiJiaoYan;
	void CloseJiaMiJiaoYanFailed()
	{
		if (!IsInvoking("JiaMiJiaoYanFailed")) {
			return;
		}
		CancelInvoke("JiaMiJiaoYanFailed");
	}

	public void OnClickJiaMiJiaoYanBt()
	{
		if (JiaMiJYLabel.text != "开启校验" && !pcvr.IsJiaoYanHid) {
			UnityEngine.Debug.Log("OnClickJiaMiJiaoYanBt...");
			OpenJiaMiJiaoYan();
			JiaMiJYLabel.text = "开启校验";
			SetJiaMiJYMsg("校验中...", JiaMiJiaoYanEnum.Null);
		}
	}
	
	public static void OpenJiaMiJiaoYan()
	{
		if (IsOpenJiaMiJiaoYan) {
			return;
		}
		IsOpenJiaMiJiaoYan = true;
		//Instance.DelayCloseJiaMiJiaoYan();

		pcvr.GetInstance().mPcvrTXManage.StartJiaoYanIO();
	}
	
	public void DelayCloseJiaMiJiaoYan()
	{
		CloseJiaMiJiaoYanFailed();
		Invoke("JiaMiJiaoYanFailed", 5f);
	}
	
	public void JiaMiJiaoYanFailed()
	{
		SetJiaMiJYMsg("", JiaMiJiaoYanEnum.Failed);
	}

	public void JiaMiJiaoYanSucceed()
	{
		SetJiaMiJYMsg("", JiaMiJiaoYanEnum.Succeed);
	}
	
	public static void CloseJiaMiJiaoYan()
	{
		if (!IsOpenJiaMiJiaoYan) {
			return;
		}
		IsOpenJiaMiJiaoYan = false;
	}
	
	void ResetJiaMiJYLabelInfo()
	{
		CloseJiaMiJiaoYan();
		JiaMiJYLabel.text = "加密校验";
	}
	
	public void SetJiaMiJYMsg(string msg, JiaMiJiaoYanEnum key)
	{
		switch (key) {
		case JiaMiJiaoYanEnum.Succeed:
			CloseJiaMiJiaoYanFailed();
			JiaMiJYMsg.text = "校验成功";
			ResetJiaMiJYLabelInfo();
			ScreenLog.Log("校验成功");
			break;
			
		case JiaMiJiaoYanEnum.Failed:
			CloseJiaMiJiaoYanFailed();
			JiaMiJYMsg.text = "校验失败";
			ResetJiaMiJYLabelInfo();
			ScreenLog.Log("校验失败");
			break;
			
		default:
			JiaMiJYMsg.text = msg;
			ScreenLog.Log(msg);
			break;
		}
    }

    public enum JiaMiJiaoYanEnum
    {
        Null,
        Succeed,
        Failed,
    }
}