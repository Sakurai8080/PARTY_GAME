using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TMPro;

/// <summary>
/// 終わった秒数の表示コンポーネント
/// </summary>
public class SecondsDisplay : MonoBehaviour
{
    [Header("変数")]
    [Tooltip("メインに表示する秒数TMP")]
    [SerializeField]
    private TextMeshProUGUI _currentSecondTMP = default;

    [Tooltip("各個人の表示TMP")]
    [SerializeField]
    private List<TextMeshProUGUI> _SecoundTMPList = default;

    int _currentOrder = 0;

    /// <summary>
    /// 各個人の結果を表示する処理
    /// </summary>
    public void ResultTMPActivator()
    {
        _SecoundTMPList[_currentOrder].text = _currentSecondTMP.text;
        _SecoundTMPList[_currentOrder].DOFade(1, 0.25f);
        _currentOrder++;
    }

    /// <summary>
    /// 最終結果で負けた秒数の色変化
    /// </summary>
    /// <param name="order">負けた秒数の要素数</param>
    public void FinalResultTextChange(params int[] order)
    {
        for (int i = 0; i < order.Length; i++)
        {
            _SecoundTMPList[order[i]].DOColor(Color.red, 3)
                                     .SetEase(Ease.OutQuad);
        }
    }
}
