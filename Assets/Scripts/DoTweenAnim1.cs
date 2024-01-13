using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using UniRx;
using System;

public class DoTweenAnim1 : TweenBase
{
    private Tween _currentTween = null;

    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        PlayAnimation();
    }

    private void PlayAnimation()
    {
        _currentTween = transform.DOScale(1, 1f)
                                 .SetEase(Ease.InSine)
                                 .SetDelay(0.5f)
                                 .OnComplete(async () =>
                                 {
                                     await AnimationDelay(1000);
                                     UiLoopAnimation();
                                 });
    }


    private async UniTask AnimationDelay(double delayTime)
    {
        await UniTask.Delay(TimeSpan.FromMilliseconds(delayTime));
    }

    protected override void UiLoopAnimation()
    {
        _currentTween = transform.DOScale(0.8f, 1)
                                 .SetLoops(-1, LoopType.Yoyo)
                                 .SetEase(Ease.Linear);

        _targetImage.DOFade(0.5f, 1)
                    .SetLoops(-1, LoopType.Yoyo);
    }
}
