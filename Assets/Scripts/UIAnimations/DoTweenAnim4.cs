using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using UniRx;
using System;
using System.Linq;

public class DoTweenAnim4 : TweenBase
{
    protected override void PlayAnimation()
    {
        _currentFadeTween = _targetImage.DOFade(0.1f, 1)
                                         .SetEase(Ease.OutFlash);

        _currentScaleTween = transform.DOScale(1, 1f)
                                      .SetEase(Ease.OutQuint)
                                      .SetDelay(0.5f)
                                      .OnComplete(async () =>
                                      {
                                          await AnimationDelay(1000);
                                          UiLoopAnimation();
                                      });
    }

    protected override void UiLoopAnimation()
    {
        _currentScaleTween = transform.DOScale(0.3f, 1)
                                      .SetLoops(-1, LoopType.Yoyo)
                                      .SetEase(Ease.OutCubic);

        _currentFadeTween = _targetImage.DOFade(1f, 1)
                                         .SetLoops(-1, LoopType.Yoyo);
    }
}
