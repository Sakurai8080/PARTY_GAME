using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;
using TMPro;

namespace TweenGroup
{
    /// <summary>
    /// スイッチボタンのアニメーション機能
    /// </summary>
    public class ToggleButtonAnimation : TweenBase
    {

        protected override void OnEnable()
        {
            UiLoopAnimation();
        }

        protected override void PlayAnimation()
        {
            throw new NotImplementedException();
        }

        protected override void UiLoopAnimation()
        {
            _currentFadeTween = _targetImage.transform.DOScale(0.8f, 1)
                                                      .SetEase(Ease.InFlash)
                                                      .SetLoops(-1, LoopType.Yoyo)
                                                      .SetUpdate(true);
        }

        private new void Start()
        {
            //Do nothing.
        }
    }
}