using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// ゲームセレクトUIのアニメーション
/// </summary>
public class GameSelectUIAnimation : MonoBehaviour, IDisposable
{
    [Header("変数")]
    [Tooltip("動かすイメージ")]
    [SerializeField]
    private Image _targetImage = default;

    [Tooltip("ゲームの種類")]
    [SerializeField]
    private GameType _gameType = default;

    Tween _currentScaleTween;
    Tween _currentFadeTween;

    private void Start()
    {
        AnimationSetup();
    }

    private void OnDisable()
    {
        Dispose();
    }

    /// <summary>
    /// 選択済みでなければアニメーション開始
    /// </summary>
    private void AnimationSetup()
    {
        bool isSelected = GameManager.Instance.SelectedChecker(_gameType);
        if (!isSelected)
            UiLoopAnimation();
    }

    /// <summary>
    /// アニメーションのループ
    /// </summary>
    private void UiLoopAnimation()
    {
        _currentScaleTween = transform.DOScale(0.9f, 1f)
                                      .SetEase(Ease.InFlash)
                                      .SetLoops(-1, LoopType.Yoyo)
                                      .SetLink(gameObject);

        _currentFadeTween = _targetImage.DOFade(0.8f, 1)
                                         .SetEase(Ease.InBounce)
                                         .SetLoops(-1, LoopType.Yoyo)
                                         .SetLink(gameObject);
    }

    public void Dispose()
    {
        _currentFadeTween?.Kill();
        _currentFadeTween = null;
        _currentScaleTween?.Kill();
        _currentScaleTween = null;
    }
}
