using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;


/// <summary>
/// UIのアニメーション用の基底クラス
/// </summary>
public abstract class TweenBase : MonoBehaviour
{
    public Image TargetImage => _targetImage;
    public Color InitialColor => _initialColor;
    public Tween CurrentFadeTween => _currentFadeTween;
    public Tween CurrentScaleTween => _currentScaleTween;

    [Header("Variable")]
    [Tooltip("Tweenのスクリタブルオブジェクト")]
    [SerializeField]
    protected TweenData _tweenData;

    [Tooltip("カラーをコントロールするUI")]
    [SerializeField]
    protected Image _targetImage = default;

    [Tooltip("スケールをコントロールするUI")]
    [SerializeField]
    protected Button _tweensButton = default;

    /// <summary>現在起動しているScaleに関わるTween操作用</summary>
    protected Tween _currentScaleTween = null;
    /// <summary>現在起動しているFadeに関わるTween操作用</summary>
    protected Tween _currentFadeTween = null;
    protected Color _initialColor;
    protected TextMeshProUGUI _amountText;

    protected abstract void PlayAnimation();
    protected abstract void UiLoopAnimation();

    protected virtual void OnEnable()
    {
        transform.localScale = Vector3.zero;
        PlayAnimation();
    }

    protected virtual void OnDisable()
    {
        ImageAlphaController(_targetImage, 1);
        BombAnimationController.KillTweens(_currentFadeTween);
        BombAnimationController.KillTweens(_currentScaleTween);
    }

    protected virtual void Start()
    {
        _initialColor = _targetImage.color;
    }

    //WARNING:引数はミリ秒
    protected async UniTask AnimationDelay(double delayTime)
    {
        await UniTask.Delay(TimeSpan.FromMilliseconds(delayTime));
    }

    protected void ImageAlphaController(Image targetImage, float alphaAmount)
    {
        Color color = targetImage.color;
        color.a = alphaAmount;
        targetImage.color = _initialColor;
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
