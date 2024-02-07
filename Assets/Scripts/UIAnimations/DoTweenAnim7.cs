using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace TweenGroup
{
    public class DoTweenAnim7 : TweenBase
    {

        [SerializeField]
        private int _bounceCount = 4;

        protected override void PlayAnimation()
        {
            _currentScaleTween = transform.DOScale(1f, _tweenData.ScaleDuration)
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
            _currentScaleTween = transform.DOShakeScale(_tweenData.ScaleDuration, 0.3f, _bounceCount)
                                          .SetLoops(-1, _tweenData.LoopType);
        }
    }
}