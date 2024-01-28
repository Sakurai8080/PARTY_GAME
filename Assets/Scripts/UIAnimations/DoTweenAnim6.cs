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
        private Vector3 _loopScaleAmount = new Vector3(0.1f, 0.1f, 0);

        private void OnEnable()
        {
            transform.localScale = Vector3.zero;
            PlayAnimation();
        }

        protected override void PlayAnimation()
        {
            _currentScaleTween = transform.DOScale(1f, 1f)
                                          .SetEase(Ease.InExpo)
                                          .SetDelay(0.5f)
                                          .OnComplete(async () =>
                                          {
                                              await AnimationDelay(1000);
                                              UiLoopAnimation();
                                          });
        }

        protected override void UiLoopAnimation()
        {
            _currentScaleTween = transform.DOBlendableScaleBy(_loopScaleAmount,1f)
                                          .SetEase(Ease.InQuad)
                                          .SetLoops(-1, LoopType.Yoyo);
        }
    }
}
