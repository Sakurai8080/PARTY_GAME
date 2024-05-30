using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using TMPro;

/// <summary>
/// ボタンゲームのUIプレゼンター
/// </summary>
public class TBGameUIPresenter : PresenterBase
{
    [Tooltip("選択を指示する説明用TMP")]
    [SerializeField]
    private TextMeshProUGUI _naviTMP = default;

    [Tooltip("各セレクトボタン")]
    [SerializeField]
    private TBGameSelectBtn[] _button = default;

    protected override void Start()
    {
        base.Start();

        NaviTextAnimation naviTextAnimation = _naviTMP.GetComponent<NaviTextAnimation>();

        for (int i = 0; i < _button.Length; i++)
        {
            _button[i].SelectedObsever
                      .TakeUntilDestroy(this)
                      .Subscribe(button =>
                      {
                          naviTextAnimation.StopAnimation();
                          TBGameManager.Instance.MissButtonChecker(button);
                          _currentOrderUIs.gameObject.SetActive(false);
                          _naviTMP.gameObject.SetActive(false);
                      });
        }

        _activeSwitchButton.OnClickObserver
                      .Subscribe(_ =>
                      {
                          FadeManager.Instance.OrderChangeFadeAnimation().Forget();
                          TBGameManager.Instance.ButtonRandomHide();
                      }).AddTo(this);

        FadeManager.Instance.NameAnimCompletedObserver
                .TakeUntilDestroy(this)
                .Subscribe(_ =>
                {
                    if (_naviTMP.enabled)
                    _naviTMP.gameObject.SetActive(true);
                    naviTextAnimation.AnimationStart();
                });
    }
}
