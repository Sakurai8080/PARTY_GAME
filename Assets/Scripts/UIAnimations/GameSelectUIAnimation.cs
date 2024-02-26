using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// UIのアニメーションコンポーネント
/// </summary>
public class GameSelectUIAnimation : MonoBehaviour
{
    [SerializeField]
    private Image _targetImage = default;

    [SerializeField]
    private GameType _gameType = default;

    Tween _currentScaleTween;
    Tween _currentFadeTween;

    private void Start()
    {
        bool isSelected = GameManager.Instance.SelectedChecher(_gameType);
        if (isSelected)
            UiDisabledSet();
        else
            UiLoopAnimation();
    }

    private void UiLoopAnimation()
    {
        _currentScaleTween = transform.DOScale(0.9f, 1f)
                                      .SetEase(Ease.InFlash)
                                      .SetLoops(-1, LoopType.Yoyo);

        _currentFadeTween = _targetImage.DOFade(0.8f, 1)
                                         .SetEase(Ease.InBounce)
                                         .SetLoops(-1, LoopType.Yoyo);

    }

    private void UiDisabledSet()
    {
        _currentScaleTween = transform.DOScale(1, 0);
        _currentFadeTween = _targetImage.DOFade(0.2f, 0);
    }

    public void CurrentTweendKill()
    {
        _currentFadeTween?.Kill();
        _currentScaleTween?.Kill();
    }
}
