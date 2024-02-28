using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;

namespace TweenGroup
{
    /// <summary>
    /// UIのアニメーション用の基底クラス
    /// </summary>
    public abstract class TweenBase : MonoBehaviour
    {
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
            _amountText = GetComponentInChildren<TextMeshProUGUI>();
            _tweensButton.OnClickAsObservable()
                         .TakeUntilDestroy(this)
                         .Subscribe(_ =>
                         {
                             BombAnimationController._resetColor = _initialColor;
                             StartCoroutine(BombAnimationController.PauseTweens());
                             SelectedAnimation();
                         });

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

        protected void SelectedAnimation()
        {
            transform.DOScale(1, 0.25f)
                     .SetEase(Ease.OutQuint)
                     .SetDelay(0.1f)
                     .OnComplete(() =>
                     {
                         _amountText.SetText("");
                         bool inBomb = BombManager.BombInChecker(_targetImage);
                         BombAnimationController.TweenRemoveFromList(_currentScaleTween);
               　        float duration = 2f;
                    　　  transform.DOPunchRotation(new Vector3(180f, 270, -45), duration, 5, 1f);
                         BombAnimation((int)duration + 1, inBomb).Forget();
                     });
        }

        protected async UniTask BombAnimation(int delayTime, bool inBomb)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delayTime));
            if (!inBomb)
            {
                _targetImage.DOFade(0, 1).SetEase(Ease.InSine)
                                         .OnComplete(() =>
                                         {
                                             _targetImage.gameObject.SetActive(false);
                                         });
                return;
            }

            GameManager.Instance.SceneLoader("MainScene");
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
}