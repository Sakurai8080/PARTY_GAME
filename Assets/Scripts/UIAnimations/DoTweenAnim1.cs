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
    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        PlayAnimation();
    }

    protected override void PlayAnimation()
    {
        _currentScaleTween= transform.DOScale(1, 1f)
                                     .SetEase(Ease.InSine)
                                     .SetDelay(0.5f)
                                     .OnComplete(async () =>
                                     {
                                         await AnimationDelay(1000);
                                         UiLoopAnimation();
                                     });
    }

    protected override void UiLoopAnimation()
    {
        _currentScaleTween = transform.DOScale(0.8f, 1)
                                      .SetLoops(-1, LoopType.Yoyo)
                                      .SetEase(Ease.Linear);

        _currentFadeTween = _targetImage.DOFade(0.5f, 1)
                                        .SetLoops(-1, LoopType.Yoyo);
    }
}
