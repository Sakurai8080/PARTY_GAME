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

        protected override void PlayAnimation()
        {
            _currentScaleTween = transform.DOScale(1f, 1f)
                                          .SetEase(Ease.OutExpo)
                                          .SetDelay(0.5f)
                                          .OnComplete(async () =>
                                          {
                                              await AnimationDelay(1000);
                                              UiLoopAnimation();
                                          });
        }

        protected override void UiLoopAnimation()
        {
            _currentScaleTween = transform.DOShakeScale(1f, 0.3f, 4)
                                          .SetLoops(-1, LoopType.Yoyo);
        }
    }
}