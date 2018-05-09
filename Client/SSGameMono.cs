using UnityEngine;

public class SSGameMono : MonoBehaviour
{
    /// <summary>
    /// 产生预制.
    /// </summary>
    public Object Instantiate(GameObject prefab, Transform parent)
    {
        GameObject obj = (GameObject)Instantiate(prefab);
        if (parent != null)
        {
            obj.transform.SetParent(parent);
            obj.transform.localScale = prefab.transform.localScale;
            obj.transform.localPosition = prefab.transform.localPosition;
        }
        return obj;
    }
}