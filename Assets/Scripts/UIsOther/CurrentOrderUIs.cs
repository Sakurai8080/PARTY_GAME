using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

/// <summary>
/// 現在の順番を管理するコンポーネント
/// </summary>
public class CurrentOrderUIs : MonoBehaviour
{
    [Header("変数")]
    [Tooltip("現在の番を表示するTMP")]
    [SerializeField]
    private TextMeshProUGUI _currentName = default;

    [Tooltip("現在の名前表示の背景")]
    [SerializeField]
    private Image _currentNameBack = default;

    public void CurrentNameActivator()
    {
        string recieveName = NameLifeManager.Instance.CurrentNameReceiver();
        _currentName.text =  recieveName.Length > 5 ? recieveName.Substring(0, 5) : recieveName;
    }

    public void CurrentNameGroupFade(NameFadeType type)
    {
        float endValue = type == NameFadeType.In ? 1f : 0;

        _currentName.DOFade(endValue, 0.25f)
                    .SetEase(Ease.Linear);
        _currentNameBack.DOFade(endValue, 0.25f)
                        .SetEase(Ease.Linear);
    }
}

public enum NameFadeType
{
    In,
    Out
}