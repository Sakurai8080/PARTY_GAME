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

public class TextAnimation : MonoBehaviour
{

    [SerializeField]
    TweenData _tweenData = default;

    [SerializeField]
    TextMeshProUGUI _moveText = default;

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
                                      .SetLoops(-1, _tweenData.LoopType);

        TweenController._allTweenList.Add(_currentScaleTween);
    }
}
