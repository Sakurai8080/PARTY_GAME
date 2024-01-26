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
    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        PlayAnimation();
    }

    protected override void PlayAnimation()
    {
        _currentScaleTween = transform.DOScale(1, 1)
                                      .SetEase(Ease.Flash)
                                      .SetDelay(0.5f)
                                      .OnComplete(async () =>
                                      {
                                          await AnimationDelay(1000);
                                          UiLoopAnimation();
                                      });
    }

    protected override void UiLoopAnimation()
    {
        _currentScaleTween = transform.DOScale(0.1f, 1)
                                      .SetEase(Ease.InFlash)
                                      .SetLoops(-1, LoopType.Yoyo);

        _currentFadeTween = _targetImage.DOFade(0.8f, 1)
                                        .SetEase(Ease.InOutCirc)
                                        .SetLoops(-1, LoopType.Yoyo);
                                        
    }
}
