using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class TweenBase : MonoBehaviour
{
    [Header("Variable")]
    [Tooltip("遅らせる時間")]
    [SerializeField]
    protected float _delayTime = 2f;

    [Tooltip("所要時間")]
    [SerializeField]
    protected float _requiredTime = 1f;

    [Tooltip("ターゲット値")]
    [SerializeField]
    protected float _targetAmout = 1f;

    [SerializeField]
    protected Image _targetImage = default;

    protected Tween _currentScaleTween = null;
    protected Tween _currentFadeTween = null;

    protected abstract void PlayAnimation();
    protected abstract void UiLoopAnimation();

    protected async UniTask AnimationDelay(double delayTime)
    {
        await UniTask.Delay(TimeSpan.FromMilliseconds(delayTime));
    }

    protected void ImageAlphaController(Image targetImage,float alphaAmount)
    {
        Color color = targetImage.color;
        color.a = alphaAmount;
        targetImage.color = color;
    }

    protected void KillTweens()
    {
        _currentFadeTween?.Kill();
        _currentScaleTween?.Kill();
    }
}
