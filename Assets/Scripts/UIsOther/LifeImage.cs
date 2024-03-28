using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ライフを表示するためのコンポーネント
/// </summary>
public class LifeImage : MonoBehaviour
{
    [Header("変数")]
    [Tooltip("ライフ用の画像")]
    [SerializeField]
    private Image[] LifeImages = default;

    string _connectionName = default;
    int _currentLife = 0;

    private void Start()
    {
        LifeReciever();
        for (int i = 0; i < _currentLife; i++)
            LifeImages[i].gameObject.SetActive(true);
    }

    /// <summary>
    /// 確認のための名前受取 
    /// </summary>
    /// <param name="name"></param>
    public void NameReciever(string name)
    {
        _connectionName = name;
    }

    /// <summary>
    /// 表示するためのライフ確認
    /// </summary>
    private void LifeReciever()
    {
        _currentLife = NameLifeManager.Instance.NamefromLifePass(_connectionName);
    }
}
