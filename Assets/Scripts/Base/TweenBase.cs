using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
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

    [Header("変数")]
    [Tooltip("Tweenのスクリタブルオブジェクト")]
    [SerializeField]
    protected TweenData _tweenData;

    [Tooltip("カラーをコントロールするUI")]
    [SerializeField]
    protected Image _targetImage = default;

    [Tooltip("スケールをコントロールするUI")]
    [SerializeField]
    protected Button _tweensButton = default;

    protected Tween _currentScaleTween = null;
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
        //TweenUIsController.Instance.KillTweens(_currentFadeTween);
        //TweenUIsController.Instance.KillTweens(_currentScaleTween);
    }

    protected virtual void Start()
    {
        _initialColor = _targetImage.color;
    }

    /// <summary>
    /// アニメーションを遅らせる処理
    /// </summary>
    /// <param name="delayTime">遅らせる時間</param>
    //WARNING:引数はミリ秒
    protected async UniTask AnimationDelay(double delayTime)
    {
        await UniTask.Delay(TimeSpan.FromMilliseconds(delayTime));
    }

    /// <summary>
    /// アルファ値とカラーの変更
    /// </summary>
    /// <param name="targetImage">変更する画像</param>
    /// <param name="alphaAmount">アルファ値</param>
    protected void ImageAlphaController(Image targetImage, float alphaAmount)
    {
        Color color = targetImage.color;
        color.a = alphaAmount;
        targetImage.color = _initialColor;
    }

    /// <summary>
    /// 回転を初期値に戻す処理
    /// </summary>
    protected void RotationReset()
    {
        if (transform.localEulerAngles != Vector3.zero)
        {
            transform.DOLocalRotate(Vector3.zero, 0.3f)
                     .SetEase(Ease.InOutQuint);
        }
    }

    /// <summary>
    /// カラーを初期値に戻す処理
    /// </summary>
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
