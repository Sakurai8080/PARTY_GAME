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
    private void OnEnable()
    {
        ImageAlphaController(_targetImage, 0);
        PlayAnimation();
        _initialColor = _targetImage.color;
    }

    protected override void PlayAnimation()
    {
        _currentFadeTween = _targetImage.DOFade(1, 1.5f)
                                        .SetEase(Ease.InBack)
                                        .OnComplete(async () =>
                                        {
                                            await AnimationDelay(2000);
                                            UiLoopAnimation();
                                        });
    }

    protected override void UiLoopAnimation()
    {
        _currentFadeTween = _targetImage.DOColor(Color.cyan, 1)
                                        .SetLoops(-1, LoopType.Yoyo);

    }
}
