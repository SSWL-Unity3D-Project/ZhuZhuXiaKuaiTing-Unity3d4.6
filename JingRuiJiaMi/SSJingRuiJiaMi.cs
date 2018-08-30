using UnityEngine;
using System.Collections;

public class SSJingRuiJiaMi : MonoBehaviour
{
	static SSJingRuiJiaMi _Instance;
	public static SSJingRuiJiaMi GetInstance()
	{
		return _Instance;
	}

	/// <summary>
	/// 加密检测中UI.
	/// </summary>
	public GameObject m_JiaMiJianCeUI;
	GameObject m_JiaMiJianCeObj;
	void Start()
	{
		_Instance = this;

        XKGameVersionCtrl gameVersionCom = gameObject.AddComponent<XKGameVersionCtrl>();
        if (gameVersionCom != null)
        {
            gameVersionCom.Init();
        }
        
        if (pcvr.IsOpenJingRuiJiaMi)
		{
			CreatJiaMiJianCeUI();
			StartJingRuiJiaMi();
		}
		else
		{
			LoadGame();
		}
	}

	/// <summary>
	/// 完整的精锐加密校验,只需要一次成功就行,后面校验时会认为是成功的.
	/// </summary>
	void StartJingRuiJiaMi()
	{
		StartCoroutine(DelayJingRuiJiaMiJiaoYan());
	}
	
	IEnumerator DelayJingRuiJiaMiJiaoYan()
	{
		Debug.Log("Start JingRui JiaMi test...");
		yield return new WaitForSeconds(5f);
		GameRoot.StartInitialization();
		StandbyProcess sp = new StandbyProcess();
		sp.Initialization();
	}

	/// <summary>
	/// 创建加密检测UI.
	/// </summary>
	void CreatJiaMiJianCeUI()
	{
		if (m_JiaMiJianCeUI != null)
		{
			m_JiaMiJianCeObj = (GameObject)Instantiate(m_JiaMiJianCeUI);
		}
		//m_LastJiaoYanTime = Time.time;
	}

	/// <summary>
	/// 删除加密检测UI.
	/// </summary>
	public void RemoveJiaMiJianCeUI()
	{
		if (m_JiaMiJianCeObj != null)
		{
			Destroy(m_JiaMiJianCeObj);
		}
	}

	/// <summary>
	/// 加载游戏场景.
	/// </summary>
	public void LoadGame()
	{
		Application.LoadLevel(1);
	}
}