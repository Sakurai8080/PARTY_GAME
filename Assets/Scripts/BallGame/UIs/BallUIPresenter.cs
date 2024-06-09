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
        base.Start();

        NaviTextAnimation naviTextAnimation = _explonationText.GetComponent<NaviTextAnimation>();
        BallGameManager.Instance.InGameReady
                             .TakeUntilDestroy(this)
                             .Subscribe(value =>
                             {
                                 if (value)
                                 {
                                     naviTextAnimation.StopAnimation();
                                     _currentOrderUIs.gameObject.SetActive(false);
                                     _explonationText.gameObject.SetActive(false);
                                     _fallButton.gameObject.SetActive(true);
                                     _fallButton.GetComponent<Image>().DOFade(1, 0.25f)
                                                                   .SetEase(Ease.InQuad);
                                 }
                             });

        _activeSwitchButton.OnClickObserver
                       .TakeUntilDestroy(this)
                       .Subscribe(_ =>
                       {
                           _backGround.SetActive(false);
                           BallController.Instance.Setup();
                       });

        _fallButton.FallButtonClickObserver
                   .TakeUntilDestroy(this)
                   .Subscribe(_ =>
                   {
                       CinemaChineController.Instance.ActivateCameraChange(CameraType.cam2);
                       CinemaChineController.Instance.DollySet(CameraType.cam2, CameraType.cam3);
                       BallController.Instance.RotateBallParent();
                   });

        FadeManager.Instance.NameAnimStartObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(_ =>
                            {
                                _explonationText.gameObject.SetActive(true);
                                naviTextAnimation.AnimationStart();
                            });
    
    }
}
