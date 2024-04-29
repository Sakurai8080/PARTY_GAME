using DG.Tweening;
using UnityEngine;

public class ArrowImageAnim : TweenBase
{
    protected override void OnEnable()
    {
        transform.localScale = Vector3.zero;
    }

    public void ImagePlayAnimation()
    {
        _currentScaleTween = transform.DOScale(_tweenData.ScaleAmount, _tweenData.ScaleDuration)
                                      .SetEase(_tweenData.LoopEasing)
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
