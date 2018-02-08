using UnityEngine;
using System.Collections;

public class LogoAnimation : MonoBehaviour
{
    void Awake()
    {
        gameObject.SetActive(false);
    }

    public void OpenLogoAnimation()
    {
        Debug.Log("OpenLogoAnimation...");
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 动画事件回调.
    /// </summary>
    public void OnAnimationTrigger(int index)
    {
        Debug.Log("OnAnimationTrigger -> index " + index);
        gameObject.SetActive(false);
        PlayerControllerForMoiew.GetInstance().ReplayStartCartoon();
    }
}