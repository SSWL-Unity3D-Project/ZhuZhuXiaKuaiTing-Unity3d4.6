using UnityEngine;

public class XKGameTextCtrl : MonoBehaviour
{
    /// <summary>
    /// UI贴图变换.
    /// </summary>
	public Texture TextureCH;
	public Texture TextureEN;
	public bool IsFixTexture;
	public Vector2 TextureVecCh;
	public Vector2 TextureVecEn;
    /// <summary>
    /// UI动画精灵.
    /// </summary>
    public UISpriteAnimation UISpAniCom;
	public string ChSpAni; //中文前缀.
	public string EnSpAni; //英文前缀.
    /// <summary>
    /// UI图集.
    /// </summary>
    public UISprite UISpCom;
	public string ChSpName; //中文图名称.
	public string EnSpName; //英文图名称.
    /// <summary>
    /// 材质变换.
    /// </summary>
	public MeshRenderer MeshRenderCom;
	public Material Material_Ch;
	public Material Material_En;
    /// <summary>
    /// 声音变换.
    /// </summary>
    public AudioSource m_AudioSource;
    public AudioClip Audio_Ch;
    public AudioClip Audio_En;
    GameTextType GameTextVal = GameTextType.Null;
	// Use this for initialization
	void Start()
	{
        if (GameTextVal != GameTextType.Null)
        {
            return;
        }

		GameTextVal = GlobalData.GetInstance().GetGameTextMode();
		//GameTextVal = GameTextType.English; //test.
		//Debug.Log("GameTextVal ================== "+GameTextVal);
		CheckGameUITexture();
		CheckUISpAniCom();
		CheckGameUISpCom();
		CheckMeshRenderCom();
        CheckGameAudioClip();
        Destroy(this);
    }

    void CheckGameAudioClip()
    {
        if (m_AudioSource != null)
        {
            switch (GameTextVal)
            {
                case GameTextType.Chinese:
                    {
                        m_AudioSource.clip = Audio_Ch;
                        break;
                    }
                case GameTextType.English:
                    {
                        m_AudioSource.clip = Audio_En;
                        break;
                    }
            }
        }
    }

    void CheckGameUITexture()
	{
		if (TextureCH != null && TextureEN != null) {
			//改变UITexture的图片.
			UITexture uiTextureCom = GetComponent<UITexture>();
			switch (GameTextVal) {
			case GameTextType.Chinese:
				if (uiTextureCom != null) {
					uiTextureCom.mainTexture = TextureCH;
					if (IsFixTexture) {
						uiTextureCom.width = (int)TextureVecCh.x;
						uiTextureCom.height = (int)TextureVecCh.y;
					}
				}
				break;
				
			case GameTextType.English:
				if (uiTextureCom != null) {
					uiTextureCom.mainTexture = TextureEN;
					if (IsFixTexture) {
						uiTextureCom.width = (int)TextureVecEn.x;
						uiTextureCom.height = (int)TextureVecEn.y;
					}
				}
				break;
			}
		}
	}
	
	void CheckUISpAniCom()
	{
		if (UISpAniCom == null) {
			return;
		}
		
		switch (GameTextVal) {
		case GameTextType.Chinese:
			UISpAniCom.namePrefix = ChSpAni;
			break;
			
		case GameTextType.English:
			UISpAniCom.namePrefix = EnSpAni;
			break;
		}
	}
	
	void CheckGameUISpCom()
	{
		if (UISpCom == null) {
			return;
		}

		switch (GameTextVal) {
		case GameTextType.Chinese:
			UISpCom.spriteName = ChSpName;
			break;
			
		case GameTextType.English:
			UISpCom.spriteName = EnSpName;
			break;
		}
	}
	
	void CheckMeshRenderCom()
	{
		if (MeshRenderCom == null) {
			return;
		}
		
		switch (GameTextVal) {
		case GameTextType.Chinese:
			MeshRenderCom.material = Material_Ch;
			break;
			
		case GameTextType.English:
			MeshRenderCom.material = Material_En;
			break;
		}
	}
}