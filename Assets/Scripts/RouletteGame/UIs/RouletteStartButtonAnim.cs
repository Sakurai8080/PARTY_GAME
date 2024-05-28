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
                                      .SetEase(_tweenData.ScaleEasing)
                                      .OnComplete(()=> UILoopAnimation());
    }

    private void UILoopAnimation()
    {
        _currentScaleTween = transform.DOScale(0.8f, _tweenData.ScaleDuration)
                                         .SetLoops(-1, _tweenData.LoopType)
                                         .SetEase(_tweenData.LoopEasing);
    }

    /// <summary>
    /// ループさせるアニメーション
    /// </summary>
    /// <param name="clickCount">クリック回数</param>
    public void StopAnimation(int clickCount)
    {
        if (clickCount > 1)
            return;
        else if(clickCount == 1)
        {
            _currentScaleTween.Kill();
            _currentScaleTween = transform.DOScale(1, 0.25f)
                                          .SetEase(Ease.InQuad);
        }
    }

    protected override void UiLoopAnimation()
    {
        throw new NotImplementedException();
    }
}