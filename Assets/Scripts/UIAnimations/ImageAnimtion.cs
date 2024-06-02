using DG.Tweening;
using UniRx;
using UnityEngine;

/// <summary>
/// 全イメージに使えるアニメーションコンポーネント
/// </summary>
public class ImageAnimtion : TweenBase
{
    protected override void OnEnable()
    {
        Dispose();
        UiLoopAnimation();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        transform.DOScale(Vector3.one, 0);
    }

    protected override void PlayAnimation()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// ループアニメーション
    /// </summary>
    protected override void UiLoopAnimation()
    {
        _currentScaleTween = transform.DOScale(_tweenData.ScaleAmount, _tweenData.ScaleDuration)
                                      .SetEase(_tweenData.LoopEasing)
                                      .SetLoops(-1, _tweenData.LoopType)
                                      .SetLink(gameObject);
    }
}
