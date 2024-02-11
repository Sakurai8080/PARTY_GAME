using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class DoTweenAnim9 : TweenBase
{

    protected override void OnEnable()
    {
        TweenManager._allTweenList.Add(_targetImage);
        transform.localScale = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        PlayAnimation();
    }

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
        _currentScaleTween = transform.DORotate(new Vector3(0,0,540),1f,RotateMode.FastBeyond360)
                                      .SetEase(_tweenData.LoopEasing)
                                      .SetLoops(-1, _tweenData.LoopType);

        _currentFadeTween = _targetImage.DOFade(0, 1)
                                        .SetEase(_tweenData.LoopEasing)
                                        .SetLoops(-1, LoopType.Yoyo);
    }
}
