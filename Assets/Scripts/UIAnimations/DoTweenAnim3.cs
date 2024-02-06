using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Linq;

public class DoTweenAnim3 : TweenBase
{
    protected override void OnEnable()
    {
        ImageAlphaController(_targetImage, 0);
        _initialColor = _targetImage.color;
        PlayAnimation();
    }

    protected override void PlayAnimation()
    {
        _currentFadeTween = _targetImage.DOFade(1, _tweenData.FadeDuration)
                                        .SetEase(_tweenData.FadeEasing)
                                        .SetDelay(_tweenData.AnimationDelayTime)
                                        .OnComplete(async () =>
                                        {
                                            await AnimationDelay(1000);
                                            UiLoopAnimation();
                                        });
    }

    protected override void UiLoopAnimation()
    {
        _currentFadeTween = _targetImage.DOColor(_tweenData.LoopColor, _tweenData.FadeDuration)
                                        .SetEase(_tweenData.LoopEasing)
                                        .SetLoops(-1, _tweenData.LoopType);


        _currentScaleTween = transform.DOScale(0,1f)
                                      .SetEase(_tweenData.LoopEasing)
                                      .SetLoops(-1,_tweenData.LoopType);

    }
}
