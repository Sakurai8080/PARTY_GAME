using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class TheButtonAnimation : MonoBehaviour
{
    [Header("変数")]
    [Tooltip("Tweenのスクリタブルオブジェクト")]
    [SerializeField]
    protected TweenData _tweenData;

    private Tween _currentScaleTween;

    private void OnEnable()
    {
        AnimationStart();
    }

    private void OnDisable()
    {
        StopAnimation();
    }

    /// <summary>
    /// 各ボタンのアニメーションスタート処理 
    /// </summary>
    private void AnimationStart()
    {
        Dispose();
        UiLoopAnimation();
    }

    /// <summary>
    //  アニメーションを止める処理
    /// </summary>
    public void StopAnimation()
    {
        Dispose();
        _currentScaleTween = transform.DOScale(Vector3.one, 0)
                                      .SetEase(Ease.Linear)
                                      .SetLink(gameObject);
    }

    /// <summary>
    /// ループさせるアニメーション
    /// </summary>
    private void UiLoopAnimation()
    {
        _currentScaleTween = transform.DOScale(_tweenData.ScaleAmount, _tweenData.ScaleDuration)
                                      .SetEase(_tweenData.LoopEasing)
                                      .SetLoops(-1, _tweenData.LoopType)
                                      .SetLink(gameObject);
    }

    public void Dispose()
    {
        _currentScaleTween?.Kill();
        _currentScaleTween = null;
    }
}
