using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;
using TMPro;


/// <summary>
/// スイッチボタンのアニメーション機能
/// </summary>
public class ToggleButtonAnimation : MonoBehaviour
{
    private void OnEnable()
    {
        UiLoopAnimation();
    }

    private void UiLoopAnimation()
    {
        transform.DOScale(0.8f, 1)
                 .SetEase(Ease.InFlash)
                 .SetLoops(-1, LoopType.Yoyo)
                 .SetUpdate(true);
    }

}