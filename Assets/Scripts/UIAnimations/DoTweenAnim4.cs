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

    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        ImageAlphaController(_targetImage, 1);
        PlayAnimation();
    }

    private void OnDisable()
    {
        KillTweens();
    }

    protected override void PlayAnimation()
    {
        _currentScaleTween = _targetImage.DOFade(0.5f, 1)
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

        _currentScaleTween = _targetImage.DOFade(1f, 1)
                                         .SetLoops(-1, LoopType.Yoyo);
    }
}
