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
            
        }

        protected override void UiLoopAnimation()
        {
            //_currentScaleTween = transform.DOBlendableScaleBy(_loopScaleAmount, 1f)
            //                              .SetEase(Ease.InQuad)
            //                              .SetLoops(-1, LoopType.Yoyo);
        }
    }
}