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
            TweenManager._allTweenList.Add(_targetImage);
            _targetImage.DOFade(0, 0);
            _text.DOFade(0, 0);
            PlayAnimation();
        }

        protected override void OnDisable()
        {
            ImageAlphaController(_targetImage, 1);
            KillTweens();
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
            _currentFadeTween = _targetImage.DOColor(_tweenData.LoopColor, _tweenData.FadeDuration)
                                            .SetEase(_tweenData.LoopEasing)
                                            .SetLoops(-1, _tweenData.LoopType);


            _currentScaleTween = transform.DOScale(0.3f, 1f)
                                          .SetEase(_tweenData.LoopEasing)
                                          .SetLoops(-1, _tweenData.LoopType);

        }
    }
}