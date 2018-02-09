using UnityEngine;
using System.Collections;

public class LogoAnimation : MonoBehaviour
{
	public GameObject BeiJingObj;
    void Awake()
    {
        gameObject.SetActive(false);
		BeiJingObj.SetActive(false);
    }

    public void OpenLogoAnimation()
    {
        Debug.Log("OpenLogoAnimation...");
		gameObject.SetActive(true);
		BeiJingObj.SetActive(true);
    }

    /// <summary>
    /// 动画事件回调.
    /// </summary>
    public void OnAnimationTrigger(int index)
    {
        Debug.Log("OnAnimationTrigger -> index " + index);
		gameObject.SetActive(false);
		BeiJingObj.SetActive(false);
        PlayerControllerForMoiew.GetInstance().ReplayStartCartoon();
    }
}