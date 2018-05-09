using UnityEngine;

public class SSGameDataCtrl : MonoBehaviour
{
    [System.Serializable]
    public class UIData
    {
        /// <summary>
        /// 游戏时长.
        /// </summary>
        public float m_pGameTime = 300.0f;
        /// <summary>
        /// 路径总长.
        /// </summary>
        public float Distance = 6400;
    }
    public UIData m_UIData;

    static SSGameDataCtrl _Instance = null;
    public static SSGameDataCtrl GetInstance()
    {
        return _Instance;
    }

    void Awake()
    {
        _Instance = this;
    }
}