using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;

public class BombSelectButton : MonoBehaviour
{
    [SerializeField]
    Button _bombButton = default;

    [SerializeField]
    TextMeshProUGUI _buttonNum = default;

    [Tooltip("カラーをコントロールするUI")]
    [SerializeField]
    protected Image _bombTargetImage = default;

    private BombAnim _bombAnim;

    private void Start()
    {
        _bombAnim = GetComponent<BombAnim>();

        _bombButton.OnClickAsObservable()
               .TakeUntilDestroy(this)
               .Subscribe(_ =>
               {
                   BombAnimationController._resetColor = _bombAnim.InitialColor;
                   StartCoroutine(BombAnimationController.PauseTweens());
                   SelectedAnimation();
               });
    }

    protected void SelectedAnimation()
    {
        transform.DOScale(1, 0.25f)
                 .SetEase(Ease.OutQuint)
                 .SetDelay(0.1f)
                 .OnComplete(() =>
                 {
                     _buttonNum.SetText("");
                     bool inBomb = BombManager.BombInChecker(_bombTargetImage);
                     BombAnimationController.TweenRemoveFromList(_bombAnim.CurrentScaleTween);
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
            _bombTargetImage.DOFade(0, 1).SetEase(Ease.InSine)
                                     .OnComplete(() =>
                                     {
                                         _bombTargetImage.gameObject.SetActive(false);
                                     });
            return;
        }

        GameManager.Instance.SceneLoader("MainScene");
    }
}
