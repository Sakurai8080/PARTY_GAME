using UnityEngine;
using DG.Tweening;

namespace TweenGroup
{
    /// <summary>
    /// ボムカード用のアニメーションコンポーネント
    /// </summary>
    public class BombAnim : TweenBase
    {
        [Header("Variable")]
        [Tooltip("ループ時のバウンド回数")]
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
            _currentScaleTween = transform.DOShakeScale(_tweenData.ScaleDuration, 0.1f, _bounceCount)
                                          .SetLoops(-1, _tweenData.LoopType);

            TweenController._allTweenList.Add(_currentScaleTween);
        }
    }
}