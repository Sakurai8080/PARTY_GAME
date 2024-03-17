using UnityEngine;
using DG.Tweening;

namespace TweenGroup
{
    /// <summary>
    /// UIのアニメーションコンポーネント
    /// </summary>
    public class UIAnimtion : TweenBase
    {
        protected override void OnEnable()
        {
            UiLoopAnimation();
        }

        protected override void PlayAnimation()
        {
            throw new System.NotImplementedException();
        }

        protected override void UiLoopAnimation()
        {
            _currentScaleTween = transform.DOScale(_tweenData.ScaleAmount, _tweenData.ScaleDuration)
                                          .SetEase(_tweenData.LoopEasing)
                                          .SetLoops(-1, _tweenData.LoopType);


            _currentFadeTween = _targetImage.DOFade(0.9f,_tweenData.FadeDuration)
                                             .SetEase(_tweenData.LoopEasing)
                                             .SetLoops(-1, _tweenData.LoopType);
        }
    }
}