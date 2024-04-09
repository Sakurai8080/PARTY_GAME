using DG.Tweening;
using UniRx;

/// <summary>
/// 全イメージに使えるアニメーションコンポーネント
/// </summary>
public class ImageAnimtion : TweenBase
{
    protected override void OnEnable()
    {
        UiLoopAnimation();
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


        _currentFadeTween = _targetImage.DOFade(0.9f, _tweenData.FadeDuration)
                                         .SetEase(_tweenData.LoopEasing)
                                         .SetLoops(-1, _tweenData.LoopType)
                                         .SetLink(gameObject);
                                         
    }
}
