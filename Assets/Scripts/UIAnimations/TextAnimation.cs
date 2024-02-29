using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;
using TMPro;

/// <summary>
/// テキストを動かすコンポーネント
/// </summary>
public class TextAnimation : MonoBehaviour
{
    [Header("Variable")]
    [Tooltip("Tweenのスクリタブルオブジェクト")]
    [SerializeField]
    TweenData _tweenData = default;

    [Tooltip("動かすテキスト")]
    [SerializeField]
    TextMeshProUGUI _moveText = default;

    [Tooltip("弾ませるカウント")]
    [SerializeField]
    int _bounceCount = 2;

    private Tween _currentScaleTween;

    void Start()
    {
        UiLoopAnimation();
    }

    private void UiLoopAnimation()
    {
        _currentScaleTween = transform.DOShakeScale(_tweenData.ScaleDuration, 0.1f, _bounceCount)
                                      .SetEase(_tweenData.LoopEasing)
                                      .SetLoops(-1, _tweenData.LoopType);

        AllBombAnimationController._allTweenList.Add(_currentScaleTween);
    }
}
