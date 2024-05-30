using DG.Tweening;
using UnityEngine;

/// <summary>
/// ルーレットの矢印コンポーネント
/// </summary>
public class ArrowImageAnim : TweenBase
{
    protected override void OnEnable()
    {
        transform.localScale = Vector3.zero;
    }

    /// <summary>
    /// アニメーションの再生
    /// </summary>
    public void ImagePlayAnimation()
    {
        _currentScaleTween = transform.DOScale(_tweenData.ScaleAmount, _tweenData.ScaleDuration)
                                      .SetEase(_tweenData.ScaleEasing)
                                      .SetDelay(_tweenData.AnimationDelayTime)
                                      .SetLink(gameObject);
    }

    protected override void PlayAnimation()
    {
        throw new System.NotImplementedException();
    }

    protected override void UiLoopAnimation()
    {
        throw new System.NotImplementedException();
    }
}
