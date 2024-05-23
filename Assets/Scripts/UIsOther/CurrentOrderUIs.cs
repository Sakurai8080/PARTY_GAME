using UnityEngine;
using TMPro;

/// <summary>
/// 現在の順番を管理するコンポーネント
/// </summary>
public class CurrentOrderUIs : MonoBehaviour
{
    [Header("変数")]
    [Tooltip("現在の番を表示するTMP")]
    [SerializeField]
    private TextMeshProUGUI _currentName = default;

    public void CurrentNameActivator()
    {
        string recieveName = NameLifeManager.Instance.CurrentNameReciever();
        _currentName.text =  recieveName.Length > 5 ? recieveName.Substring(0, 5) : recieveName;
        
    }
}
