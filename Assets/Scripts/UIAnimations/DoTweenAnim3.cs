using UnityEngine;
using DG.Tweening;
using TMPro;

namespace TweenGroup
{
    /// <summary>
    /// UIのアニメーションコンポーネント
    /// </summary>
    public class DoTweenAnim3 : TweenBase
    {
        TextMeshProUGUI _text;

        private void Awake()
        {
            _text = GetComponentInChildren<TextMeshProUGUI>();
        }

        protected override void OnEnable()
        {
            _targetImage.DOFade(0, 0);
            _text.DOFade(0, 0);
            PlayAnimation();
        }

        protected override void OnDisable()
        {
            ImageAlphaController(_targetImage, 1);
            BombAnimationController.KillTweens(_currentScaleTween);
            BombAnimationController.KillTweens(_currentFadeTween);
            transform.localScale = Vector3.one;
        }

        protected override void PlayAnimation()
        {
            _text.DOFade(1, _tweenData.FadeDuration)
                .SetEase(Ease.InExpo)
                .SetDelay(_tweenData.AnimationDelayTime);

            _currentFadeTween = _targetImage.DOFade(1, _tweenData.FadeDuration)
                                            .SetEase(_tweenData.FadeEasing)
                                            .SetDelay(_tweenData.AnimationDelayTime)
                                            .OnComplete(async () =>
                                            {
                                                _initialColor = _targetImage.color;
                                                await AnimationDelay(1000);
                                                UiLoopAnimation();
                                            });

        }

        protected override void UiLoopAnimation()
        {
            _currentScaleTween = transform.DOScale(0.7f, 1f)
                                          .SetEase(_tweenData.LoopEasing)
                                          .SetLoops(-1, _tweenData.LoopType);

            BombAnimationController._allTweenList.Add(_currentFadeTween);
            BombAnimationController._allTweenList.Add(_currentScaleTween);

        }
    }
}