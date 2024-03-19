using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine.UI;

/// <summary>
/// ルーレットを回すためのUIアニメーション
/// </summary>
public class RouletteStartButtonAnim : TweenBase
{
    protected override void OnEnable()
    {
        PlayAnimation();
    }

    protected override void PlayAnimation()
    {
        _currentScaleTween = transform.DOScale(1, _tweenData.ScaleDuration)
                                      .SetEase(_tweenData.ScaleEasing);
    }

    public void UILoopAnimation(int clickCount)
    {
        if (clickCount > 1)
            return;
        else if(clickCount == 1)
        {
            _currentScaleTween.Kill();
            _currentScaleTween = transform.DOScale(1, 0.25f)
                                          .SetEase(Ease.InQuad);
        }
        if (clickCount == 0)
        {
            _currentScaleTween = transform.DOScale(1.15f, _tweenData.ScaleDuration)
                                          .SetLoops(-1, _tweenData.LoopType)
                                          .SetEase(_tweenData.LoopEasing);
        }
    }

    protected override void UiLoopAnimation()
    {
        throw new NotImplementedException();
    }
}