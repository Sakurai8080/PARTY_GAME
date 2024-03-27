using System;
using DG.Tweening;

/// <summary>
/// ルーレットを回すためのUIアニメーション
/// </summary>
public class RouletteStartButtonAnim : TweenBase
{
    protected override void OnEnable()
    {
        PlayAnimation();
    }

    /// <summary>
    /// 初期アニメーション
    /// </summary>
    protected override void PlayAnimation()
    {
        _currentScaleTween = transform.DOScale(1, _tweenData.ScaleDuration)
                                      .SetEase(_tweenData.ScaleEasing);
    }

    /// <summary>
    /// ループさせるアニメーション
    /// </summary>
    /// <param name="clickCount"></param>
    public void UILoopAnimation(int clickCount)
    {
        if (clickCount > 1)
            return;
        else if(clickCount == 1)
        {
            _currentScaleTween.Kill();
            _currentScaleTween = transform.DOScale(1, 0.25f)
                                          .SetEase(Ease.InQuad);
        }
        if (clickCount == 0)
        {
            _currentScaleTween = transform.DOScale(1.15f, _tweenData.ScaleDuration)
                                          .SetLoops(-1, _tweenData.LoopType)
                                          .SetEase(_tweenData.LoopEasing);
        }
    }

    protected override void UiLoopAnimation()
    {
        throw new NotImplementedException();
    }
}