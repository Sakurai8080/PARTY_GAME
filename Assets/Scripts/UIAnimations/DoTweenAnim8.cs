using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Linq;

public class DoTweenAnim8 : TweenBase
{
    protected override void OnEnable()
    {
        transform.localScale = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        PlayAnimation();
    }

    protected override void PlayAnimation()
    {
        _currentScaleTween = transform.DOScale(1, 1)
                                      .SetEase(Ease.InQuart)
                                      .SetDelay(0.5f)
                                      .OnComplete(async () =>
                                      {
                                          await AnimationDelay(1000);
                                           UiLoopAnimation();
                                      });
    }

    protected override void UiLoopAnimation()
    {
        _currentScaleTween = transform.DOBlendablePunchRotation(new Vector3(15, 1, 15), 1f,7)
                                      .SetEase(Ease.InOutElastic)
                                      .SetLoops(-1, LoopType.Yoyo);
            

        _currentFadeTween = _targetImage.DOColor(Color.yellow,0.5f)
                                         .SetEase(Ease.InOutQuint)
                                         .SetLoops(-1,LoopType.Yoyo);

    }
}
