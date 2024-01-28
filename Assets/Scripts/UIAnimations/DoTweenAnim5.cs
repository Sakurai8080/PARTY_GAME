using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Linq;

public class DoTweenAnim5 : TweenBase
{
    protected override void PlayAnimation()
    {
        _currentScaleTween = transform.DOScale(1, 1f)
                                      .SetEase(Ease.InOutBounce)
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
                                      .SetLoops(-1, LoopType.Yoyo)
                                      .SetEase(Ease.InBounce);
    }
}
