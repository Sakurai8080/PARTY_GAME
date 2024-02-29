using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System;
using TMPro;

/// <summary>
/// ボムカード用のアニメーションコンポーネント
/// </summary>
public class BombAnim : TweenBase
{
    [Header("Variable")]
    [Tooltip("ループ時のバウンド回数")]
    [SerializeField]
    private int _bounceCount = 4;

    [SerializeField]
    TextMeshProUGUI _buttonNum = default;

    protected override void PlayAnimation()
    {
        _currentScaleTween = transform.DOScale(1f, _tweenData.ScaleDuration)
                                      .SetEase(_tweenData.ScaleEasing)
                                      .SetDelay(_tweenData.AnimationDelayTime)
                                      .OnComplete(async () =>
                                      {
                                          await AnimationDelay(1000);
                                          UiLoopAnimation();
                                      });
    }

    protected override void UiLoopAnimation()
    {
        _currentScaleTween = transform.DOShakeScale(_tweenData.ScaleDuration, 0.1f, _bounceCount)
                                      .SetLoops(-1, _tweenData.LoopType);

        BombAnimationController._allTweenList.Add(_currentScaleTween);
    }

    public void SelectedAnimation()
    {
        transform.DOScale(1, 0.25f)
                 .SetEase(Ease.OutQuint)
                 .SetDelay(0.1f)
                 .OnComplete(() =>
                 {
                     _buttonNum.SetText("");
                     bool inBomb = BombManager.Instance.BombInChecker(_targetImage);
                     BombAnimationController.TweenRemoveFromList(CurrentScaleTween);
                     float duration = 2f;
                     transform.DOPunchRotation(new Vector3(180f, 270, -45), duration, 5, 1f);
                     BombAnimation((int)duration + 1, inBomb).Forget();
                 });
    }

    private async UniTask BombAnimation(int delayTime, bool inBomb)
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
}