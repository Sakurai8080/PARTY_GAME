using UnityEngine;
using DG.Tweening;

namespace TweenGroup
{
    /// <summary>
    /// UIのアニメーションコンポーネント
    /// </summary>
    public class GameSelectedUI : TweenBase
    {
        [Header("Variable")]
        [Tooltip("ループするスケールの値")]
        [SerializeField]
        private Vector3 _loopScaleAmount = new Vector3(0.8f, 0.8f, 0);

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

            TweenController._allTweenList.Add(_currentFadeTween);
            TweenController._allTweenList.Add(_currentScaleTween);
        }
    }
}
