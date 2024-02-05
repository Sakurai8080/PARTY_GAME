using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using UniRx;
using System;
using System.Linq;

public class DoTweenAnim1 : TweenBase
{
    protected override void PlayAnimation()
    {
        _currentScaleTween= transform.DOScale(1, _tweenData.ScaleDuration)
                                     .SetEase(_tweenData.ScaleEasing)
                                     .SetDelay(_tweenData.AnimationDelayTime)
                                     .OnComplete(async () =>
                                     {
                                         await AnimationDelay(1000);
                                         UiLoopAnimation();
                                     });
    }

    protected override void UiLoopAnimation()
    {
        _currentScaleTween = transform.DOScale(0.8f, _tweenData.ScaleDuration)
                                      .SetLoops(-1, _tweenData.LoopType)
                                      .SetEase(_tweenData.LoopEasing);

        _currentFadeTween = _targetImage.DOFade(0.5f, 1)
                                        .SetLoops(-1, _tweenData.LoopType);
    }
}
