using DG.Tweening;
using UnityEngine;
using TMPro;

/// <summary>
/// テキストを動かすコンポーネント
/// </summary>
public class TitleAnimation : MonoBehaviour
{
    [Header("変数")]
    [Tooltip("Tweenのスクリタブルオブジェクト")]
    [SerializeField]
    protected TweenData _tweenData;

    [Tooltip("弾ませるカウント")]
    [SerializeField]
    int _bounceCount = 2;

    Tween _currentScaleTween;

    private void Start()
    {
        UiLoopAnimation();
    }

    private void OnEnable()
    {
        UiLoopAnimation();
    }

    private void OnDisable()
    {
        Dispose();
    }

    /// <summary>
    /// ループさせるアニメーション
    /// </summary>
    private void UiLoopAnimation()
    {
        _currentScaleTween = transform.DOShakeScale(_tweenData.ScaleDuration, 0.1f, _bounceCount)
                                      .SetEase(_tweenData.LoopEasing)
                                      .SetLoops(-1, _tweenData.LoopType);
    }

    public void Dispose()
    {
        _currentScaleTween?.Kill();
        _currentScaleTween = null;
    }
}
