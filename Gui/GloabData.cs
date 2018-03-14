
public class GlobalData
{
    /// <summary>
    /// 游戏中英文界面控制.
    /// </summary>
	public GameTextType GameTextMode = GameTextType.Chinese;
    /// <summary>
    /// 游戏当前币值信息.
    /// </summary>
	public int CoinCur;
    /// <summary>
    /// 当前游戏要出的彩票数据.
    /// </summary>
    public int CaiPiaoCur;
	private static  GlobalData _Instance;
	public static GlobalData GetInstance()
	{
		if (_Instance == null) {
			_Instance = new GlobalData();
			bool isChineseGame = false;
			if (!isChineseGame) {
                _Instance.GameTextMode = GameTextType.English;
			}
			else {
                _Instance.GameTextMode = GameTextType.Chinese;
			}
		}
		return _Instance;
	}

	public static GameTextType GetGameTextMode()
	{
		if (_Instance == null) {
			GetInstance();
		}
		return _Instance.GameTextMode;
	}
}

public enum GameTextType
{
	Chinese,
	English,
}