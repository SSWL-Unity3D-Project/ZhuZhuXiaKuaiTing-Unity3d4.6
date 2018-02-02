using UnityEngine;

/// <summary>
/// npc随机root动画控制.
/// </summary>
public class NpcRandRootAni : MonoBehaviour
{
    /// <summary>
    /// 指定npc必须做某一个root动画,当RootIndex属于[1, 255].
    /// 否则启动随机root动画逻辑.
    /// </summary>
    public int RootIndex = -1;
    [Range(1, 255)]
    public int MaxRootIndex = 3;
    public Animator NpcAni;
    void Start()
    {
        if (NpcAni != null)
        {
            int rootIndexVal = (Random.Range(0, 1000) % MaxRootIndex) + 1;
            string rootAni = "IsRoot" + rootIndexVal;
            if (RootIndex >= 1 && RootIndex <= 255)
            {
                rootAni = "IsRoot" + RootIndex;
            }
            NpcAni.SetTrigger(rootAni);
        }
    }
}