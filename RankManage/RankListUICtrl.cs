using UnityEngine;

/// <summary>
/// 排行榜UI控制.
/// </summary>
public class RankListUICtrl : MonoBehaviour
{
    /// <summary>
    /// 头像列表.
    /// TouXiangImgArray[x] -> 0 猪猪侠, 1 波比, 2 超人强, 3 菲菲.
    /// </summary>
    public Texture[] TouXiangImgArray;
    /// <summary>
    /// 排名列表头像.
    /// </summary>
    public UITexture[] RankTouXiangArray;
    /// <summary>
    /// 显示排行榜UI.
    /// </summary>
    public void ShowRankListUI()
    {
        int indexVal = 0;
        for (int i = 0; i < RankTouXiangArray.Length; i++)
        {
            indexVal = (int)PlayerController.GetInstance().RankDtManage.RankDtList[i].RankType;
            RankTouXiangArray[i].mainTexture = TouXiangImgArray[indexVal];
        }
        gameObject.SetActive(true);
    }
}