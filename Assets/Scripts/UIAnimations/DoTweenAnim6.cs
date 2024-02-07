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
    public class DoTweenAnim6 : TweenBase
    {
        [SerializeField]
        private Vector3 _loopScaleAmount = new Vector3(0.1f, 0.1f, 0);

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
            _currentScaleTween = transform.DOBlendableScaleBy(_loopScaleAmount,_tweenData.ScaleDuration)
                                          .SetEase(_tweenData.LoopEasing)
                                          .SetLoops(-1, _tweenData.LoopType);
        }
    }
}
