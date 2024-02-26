using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UniRx;

namespace TweenGroup
{
    /// <summary>
    /// UIのアニメーションコンポーネント
    /// </summary>
    public class GameSelectUIAnimation : MonoBehaviour
    {
        [SerializeField]
        private Image _targetImage = default;

        Tween _currentScaleTween;
        Tween _currentFadeTween;

        private void Start()
        {
            UiLoopAnimation();
        }

        private void UiLoopAnimation()
        {
            _currentScaleTween = transform.DOScale(0.9f,1f)
                                          .SetEase(Ease.InFlash)
                                          .SetLoops(-1, LoopType.Yoyo);

            _currentScaleTween = _targetImage.DOFade(0.8f, 1)
                                             .SetEase(Ease.InBounce)
                                             .SetLoops(-1, LoopType.Yoyo);

        }
    }
}
