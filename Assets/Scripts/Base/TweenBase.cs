using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public abstract class TweenBase : MonoBehaviour
{


    [SerializeField]
    protected TweenData _tweenData;

    [SerializeField]
    protected Image _targetImage = default;

    [SerializeField]
    protected Button _tweensButton = default;

    /// <summary>現在起動しているScaleに関わるTween操作用</summary>
    protected Tween _currentScaleTween = null;
    /// <summary>現在起動しているFadeに関わるTween操作用</summary>
    protected Tween _currentFadeTween = null;
    protected Color _initialColor;


    protected abstract void PlayAnimation();
    protected abstract void UiLoopAnimation();

    protected virtual void OnEnable()
    {
        transform.localScale = Vector3.zero;
        PlayAnimation();

        TweenManager._allTweenList.Add(_targetImage);
    }

    protected virtual void OnDisable()
    {
        ImageAlphaController(_targetImage, 1);
        KillTweens();
    }

    protected virtual void Start()
    {
        _tweensButton.OnClickAsObservable()
                     .TakeUntilDestroy(this)
                     .Subscribe(_ =>
                     {
                         TweenManager.AllTweenStop(_initialColor);
                         ImageAlphaController(_targetImage, 1);
                         KillTweens();
                         SelectedAnimation();
                         RotationReset();
                         ColorReset();
                     });

        _initialColor = _targetImage.color;             
    }

    //WARNING:引数はミリ秒
    protected async UniTask AnimationDelay(double delayTime)
    {
        await UniTask.Delay(TimeSpan.FromMilliseconds(delayTime));
    }

    protected void ImageAlphaController(Image targetImage,float alphaAmount)
    {
        Color color = targetImage.color;
        color.a = alphaAmount;
        targetImage.color = _initialColor;
    }

    protected void KillTweens()
    {
        _currentFadeTween?.Kill();
        _currentScaleTween?.Kill();
        _currentFadeTween = null;
        _currentScaleTween = null;
    }

    protected void SelectedAnimation()
    {
        _currentScaleTween = transform.DOScale(1, 0.25f)
                                      .SetEase(Ease.OutQuint)
                                      .SetDelay(0.1f)
                                      .OnComplete(() => transform.DOPunchRotation(new Vector3(180f, 270, -45), 2f, 5, 1f));
    }

    protected void RotationReset()
    {
        if (transform.localEulerAngles != Vector3.zero)
        {
            transform.DOLocalRotate(Vector3.zero, 0.3f)
                     .SetEase(Ease.InOutQuint);
        }        
    }

    protected void ColorReset()
    {
        if (_initialColor != null)
        {
            if (_initialColor.a != 1)
                _initialColor.a = 1;
            _targetImage.color = _initialColor;
        }
    }
}
