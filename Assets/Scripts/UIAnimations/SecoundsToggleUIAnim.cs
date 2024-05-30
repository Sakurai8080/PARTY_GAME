using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UniRx;

public class SecoundsToggleUIAnim : TweenBase
{
    protected override void OnEnable() {}

    protected override void Start()
    {
        base.Start();
    }

    public void AnimationStart()
    {
        UiLoopAnimation();
    }

    /// <summary>
    //  アニメーションを止める処理
    /// </summary>
    public void StopAnimation()
    {
        Dispose();
        _currentScaleTween = transform.DOScale(Vector3.one, 0.1f)
                                      .SetEase(Ease.Linear)
                                      .SetLink(gameObject);
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

    protected override void PlayAnimation()
    {
        throw new System.NotImplementedException();
    }
}
