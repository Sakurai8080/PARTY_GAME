using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Linq;

public class DoTweenAnim2 : TweenBase
{
    protected override void PlayAnimation()
    {
        _currentScaleTween = transform.DOScale(1, _tweenData.ScaleDuration)
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
                                      .SetEase(_tweenData.LoopEasing)
                                      .SetLoops(-1,_tweenData.LoopType);

        _currentFadeTween = _targetImage.DOFade(0.8f, _tweenData.FadeDuration)
                                        .SetEase(_tweenData.LoopEasing)
                                        .SetLoops(-1, _tweenData.LoopType);
                                        
    }
}
