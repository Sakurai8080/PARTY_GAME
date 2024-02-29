using UnityEngine;
using DG.Tweening;

namespace TweenGroup
{
    /// <summary>
    /// UIのアニメーションコンポーネント
    /// </summary>
    public class DoTweenAnim9 : TweenBase
    {
        protected override void OnEnable()
        {
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
            _currentScaleTween = transform.DORotate(new Vector3(0, 0, 540), 1f, RotateMode.FastBeyond360)
                                          .SetEase(_tweenData.LoopEasing)
                                          .SetLoops(-1, _tweenData.LoopType);

            _currentFadeTween = _targetImage.DOFade(0, 1)
                                            .SetEase(_tweenData.LoopEasing)
                                            .SetLoops(-1, LoopType.Yoyo);

            AllBombAnimationController._allTweenList.Add(_currentFadeTween);
            AllBombAnimationController._allTweenList.Add(_currentScaleTween);
        }
    }
}