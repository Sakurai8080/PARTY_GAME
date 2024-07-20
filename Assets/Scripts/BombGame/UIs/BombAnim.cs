using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System;
using TMPro;


/// <summary>
/// ボムカード用のアニメーションコンポーネント
/// </summary>
public class BombAnim : TweenBase
{
    [Tooltip("ループ時のバウンド回数")]
    [SerializeField]
    private int _bounceCount = 4;

    [Tooltip("カードの番号")]
    [SerializeField]
    TextMeshProUGUI _buttonNum = default;

    private Action _selectedCallBack;
    private BoxContents _boxContent;

    /// <summary>
    /// ゲーム開始時のカードのアニメーション
    /// </summary>
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

    /// <summary>
    /// ゲーム開始後ループするアニメーション
    /// </summary>
    protected override void UiLoopAnimation()
    {
        _currentScaleTween = transform.DOShakeScale(_tweenData.ScaleDuration, 0.1f, _bounceCount)
                                      .SetLoops(-1, _tweenData.LoopType);

        BombUIsAnimationController.Instance.TweenAddList(_currentScaleTween);
    }

    /// <summary>
    /// カードが選択されたときのアニメーション
    /// </summary>
    public void SelectedAnimation()
    {
        transform.DOScale(1, 0.25f)
                 .SetEase(Ease.OutQuint)
                 .SetDelay(0.1f)
                 .OnComplete(() => BombCheckAndAnimation());
    }

    /// <summary>
    /// カード選択後のボム有無チェック
    /// </summary>
    private void BombCheckAndAnimation()
    {
        _buttonNum.SetText("");
        _boxContent = BombGameManager.Instance.BombInChecker(_targetImage);
        BombUIsAnimationController.Instance.TweenRemoveFromList(CurrentScaleTween);
        float duration = 2f;
        transform.DOPunchRotation(new Vector3(180f, 270, -45), duration, 5, 1f);
        switch (_boxContent)
        {
            case BoxContents.None:
                _selectedCallBack = BombGameManager.Instance.AfterEmptySelected;
                NonBombAnimation((int)duration + 1, _selectedCallBack).Forget();
                break;

            case BoxContents.Bomb:
                _selectedCallBack = BombGameManager.Instance.AfterExplosion;
                InBoxAnimation((int)duration + 1, _selectedCallBack).Forget();
                break;

            case BoxContents.Apple:
                _selectedCallBack = BombGameManager.Instance.AfterAppleSelect;
                InBoxAnimation((int)duration + 1, _selectedCallBack).Forget();
                break;

            default:
                Debug.LogError($"BoxContents :{_boxContent} が見つかりません");
                break;
        }
    }

    /// <summary>
    /// カードにボムが入っていなかったときのアニメーション
    /// </summary>
    /// <param name="delayTime">遅らせる時間</param>
    private async UniTask NonBombAnimation(int delayTime, Action callback = null)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(delayTime));
        _targetImage.DOFade(0, 1).SetEase(Ease.InSine)
                                 .OnComplete(() => _targetImage.gameObject.SetActive(false));
        callback?.Invoke();
    }

    /// <summary>
    /// ボックスに何か入っていたときのアニメーション
    /// </summary>
    /// <param name="delayTime">遅らせる時間</param>
    private async UniTask InBoxAnimation(int delayTime, Action callback = null)
    {
        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delayTime));
            _bounceCount = 10;
            _currentScaleTween = transform.DOShakeScale(2, 0.5f, _bounceCount);
            delayTime = 4;
            await UniTask.Delay(TimeSpan.FromSeconds(delayTime));
            ImageChange();
            GetComponent<Button>().image.DOFade(0, 0.5f)
                                  .OnComplete(()=> _targetImage.gameObject.SetActive(false));
            await UniTask.Delay(TimeSpan.FromSeconds(delayTime));
            callback?.Invoke();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }

    private void ImageChange()
    {
        BombUIsAnimationController.Instance.AfterImageSet(_boxContent,_targetImage.rectTransform);
    }
}