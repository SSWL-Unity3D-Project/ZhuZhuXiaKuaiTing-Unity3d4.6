using UnityEngine;

/// <summary>
/// 闪光灯跟随音乐节奏变化控制.
/// </summary>
public class SSLedByAudioCtrl : MonoBehaviour
{
	private float[] SpectrumData;
	private float MaxValue = 0.1f;
	private int ValueIndex = 0;
	private float JianGeTotalTime = 0.2f;
	private float JianGeTimeCur = 0.2f;
	private AudioSource BeiJingAudio = null;
    /// <summary>
    /// 是否改变Led的状态.
    /// </summary>
    bool IsChangeLedState = false;
    /// <summary>
    /// 闪光灯控制命令.
    /// </summary>
    public pcvr.ShanGuangDengCmd[] LedCmd = new pcvr.ShanGuangDengCmd[7];
    
    void Start()
	{
		SpectrumData = new float[64];	//8192
		JianGeTimeCur = 0.2f;
		BeiJingAudio = GetComponent<AudioSource> ();
    }

	void Update()
    {
        if (BeiJingAudio == null)
        {
            return;
        }

        if (!IsChangeLedState)
        {
            return;
        }

        if (JianGeTimeCur <= 0)
		{
			JianGeTimeCur = JianGeTotalTime;
			
			if (BeiJingAudio && BeiJingAudio.isPlaying)
			{
				BeiJingAudio.GetSpectrumData(SpectrumData, 0, FFTWindow.BlackmanHarris);
				
				if (MaxValue < SpectrumData[0])
				{
					MaxValue = SpectrumData[0];
				}

				if (SpectrumData[0] >= MaxValue * 0.75f)
				{
					ValueIndex = 1;
				}
				else if (SpectrumData[0] >= MaxValue * 0.55f)
				{
					ValueIndex = 4;
				}
				else if (SpectrumData[0] >= MaxValue * 0.3f)
				{
					ValueIndex = 5;
				}
				else if (SpectrumData[0] >= MaxValue * 0.2)
				{
					ValueIndex = 3;
				}
				else if (SpectrumData[0] >= MaxValue * 0.1f)
				{
					ValueIndex = 2;
				}
				else if (SpectrumData[0] >= MaxValue * 0.04f)
				{
					ValueIndex = 6;
				}
				else if (SpectrumData[0] >= 0f)
				{
					ValueIndex = 7;
				}
				else
				{
					ValueIndex = 0;
				}
			}
			else
			{
				ValueIndex = 0;
			}
		}
		else
		{
			JianGeTimeCur -= Time.deltaTime;
		}
		
        if (ValueIndex == 0)
        {
            pcvr.GetInstance().CloseShanGuangDeng();
        }
        else
        {
            pcvr.GetInstance().ChangeShanGuangDeng(LedCmd[ValueIndex - 1]);
        }
	}

    /// <summary>
    /// 开启改变Led状态开关.
    /// </summary>
    public void OpenChangeLedState()
    {
        IsChangeLedState = true;
    }

    /// <summary>
    /// 关闭改变Led状态开关.
    /// </summary>
    public void CloseChangeLedState()
    {
        ValueIndex = 0;
        IsChangeLedState = false;
        pcvr.GetInstance().CloseShanGuangDeng();
    }

    //void OnGUI()
    //{
    //    string info = "ValueIndex:  " + ValueIndex;
    //    GUI.Box(new Rect(10f, 50f, Screen.width - 20f, 30f), info);
    //}
}