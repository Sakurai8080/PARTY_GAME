using UnityEngine;
using UniRx;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// ボールゲームのプレゼンター
/// </summary>
public class BallUIPresenter : PresenterBase
{
    [Tooltip("ボールを落とすボタン")]
    [SerializeField]
    private BallFallButton _fallButton = default;

    [Tooltip("説明文")]
    [SerializeField]
    private TextMeshProUGUI _explonationText = default;

    [Tooltip("初期画面の背景")]
    [SerializeField]
    private GameObject _backGround = default;

    protected override void Start()
    {
        //全てボールが選択されたときの通知
        BallGameManager.Instance.InGameReady
                             .TakeUntilDestroy(this)
                             .Subscribe(value =>
                             {
                                 if (value)
                                 {
                                     _explonationText.gameObject.SetActive(false);
                                     _fallButton.gameObject.SetActive(true);
                                     _fallButton.GetComponent<Image>().DOFade(1, 0.25f)
                                                                   .SetEase(Ease.InQuad);
                                 }
                             });

        //ゲーム説明画面のボタン押下購読
        _currentHideUIs.OnClickObserver
                       .TakeUntilDestroy(this)
                       .Subscribe(_ =>
                       {
                           _nextActiveUIs.ToggleUIsVisibility();
                           _currentHideUIs.gameObject.SetActive(false);
                           _backGround.SetActive(false);
                           BallController.Instance.Setup();
                       });

        //ボールを落とすボタンの押下検知
        _fallButton.FallButtonClickObserver
                   .TakeUntilDestroy(this)
                   .Subscribe(_ =>
                   {
                       CinemaChineController.Instance.ActivateCameraChange(CameraType.cam2);
                       CinemaChineController.Instance.DollySet();
                       BallController.Instance.RotateBallParent();
                   });
    }
}
