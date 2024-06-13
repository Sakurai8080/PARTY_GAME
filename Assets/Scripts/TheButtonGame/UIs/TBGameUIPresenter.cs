using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// ボタンゲームのUIプレゼンター
/// </summary>
public class TBGameUIPresenter : PresenterBase
{
    [Tooltip("選択を指示する説明用TMP")]
    [SerializeField]
    private TextMeshProUGUI _naviTMP = default;

    [Tooltip("画面を覆う画像")]
    [SerializeField]
    private Image _initCoverImage = default;

    [Tooltip("各セレクトボタン")]
    [SerializeField]
    private TBGameSelectBtn[] _button = default;

    protected override void Start()
    {
        base.Start();

        NaviTextAnimation naviTextAnimation = _naviTMP.GetComponent<NaviTextAnimation>();

        for (int i = 0; i < _button.Length; i++)
        {
            _button[i].SelectedObserver
                      .TakeUntilDestroy(this)
                      .Subscribe(button =>
                      {
                          naviTextAnimation.StopAnimation();
                          TBGameManager.Instance.MissButtonChecker(button);
                          _currentOrderUIs.gameObject.SetActive(false);
                          _naviTMP.gameObject.SetActive(false);
                          _initCoverImage.gameObject.SetActive(true);
                      });
        }

        _activeSwitchButton.OnClickObserver
                      .Subscribe(_ =>
                      {
                          FadeManager.Instance.OrderChangeFadeAnimation().Forget();
                          TBGameManager.Instance.ButtonRandomHide();
                          _initCoverImage.gameObject.SetActive(true);
                      }).AddTo(this);

        FadeManager.Instance.NameAnimStartObserver
                .TakeUntilDestroy(this)
                .Subscribe(_ =>
                {
                    if (_naviTMP.enabled)
                    _naviTMP.gameObject.SetActive(true);
                    naviTextAnimation.AnimationStart();
                });

        FadeManager.Instance.NameAnimFadeCompletedObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(_ =>_initCoverImage.gameObject.SetActive(false));
    }
}
