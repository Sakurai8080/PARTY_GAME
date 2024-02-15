using UnityEngine;
using DG.Tweening;

namespace TweenGroup
{
    /// <summary>
    /// UIのアニメーションコンポーネント
    /// </summary>
    public class DoTweenAnim8 : TweenBase
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
            _currentScaleTween = transform.DOBlendablePunchRotation(new Vector3(15, 1, 15), _tweenData.ScaleDuration, 7)
                                          .SetEase(_tweenData.LoopEasing)
                                          .SetLoops(-1, _tweenData.LoopType);


            _currentFadeTween = _targetImage.DOColor(_tweenData.LoopColor, 1f)
                                             .SetEase(_tweenData.LoopEasing)
                                             .SetLoops(-1, _tweenData.LoopType);

        }
    }
}