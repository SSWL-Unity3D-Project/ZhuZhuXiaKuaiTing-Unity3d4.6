using UnityEngine;

public class SSPrintCardCtrl : MonoBehaviour
{
    /// <summary>
    /// 彩票不足.
    /// </summary>
    public GameObject CardEmptyObj;
    /// <summary>
    /// JiFenSpriteArray[0] -> 数据最低位.
    /// </summary>
    public UISprite[] m_CardNumSpArray;
    /// <summary>
    /// 初始化.
    /// </summary>
    public void Init(int cardNum)
    {
        CardEmptyObj.SetActive(false);
        ShowGameCardInfo(cardNum);
    }

    /// <summary>
    /// 显示彩票数量信息.
    /// </summary>
    public void ShowGameCardInfo(int cardNum)
    {
        int valTmp = 0;
        string valStr = cardNum.ToString();
        for (int i = 0; i < 2; i++)
        {
            if (valStr.Length > i)
            {
                m_CardNumSpArray[i].enabled = true;
                valTmp = cardNum % 10;
                m_CardNumSpArray[i].spriteName = valTmp.ToString();
                cardNum = (int)(cardNum / 10f);
            }
            else
            {
				m_CardNumSpArray[i].enabled = true;
				m_CardNumSpArray[i].spriteName = "0";
            }
        }
    }

    /// <summary>
    /// 显示彩票不足UI.
    /// </summary>
    public void ShowCardEmpty()
    {
        if (!CardEmptyObj.activeInHierarchy)
        {
            CardEmptyObj.SetActive(true);
        }
    }

    public void HiddenCardPrintUI()
    {
        gameObject.SetActive(false);
    }
}