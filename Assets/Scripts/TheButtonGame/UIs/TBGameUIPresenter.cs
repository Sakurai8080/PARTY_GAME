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
        for (int i = 0; i < _button.Length; i++)
        {
            _button[i].SelectedObsever
                      .TakeUntilDestroy(this)
                      .Subscribe(button =>
                      {
                          TBGameManager.Instance.MissButtonChecker(button);
                          _currentOrderUIs.gameObject.SetActive(false);
                          _naviTMP.gameObject.SetActive(false);
                      });
        }

        _activeSwitchButton.OnClickObserver
                      .Subscribe(_ =>
                      {
                          TBGameManager.Instance.ButtonRandomHide();
                          FadeManager.Instance.OrderChangeFadeAnimation().Forget();
                      }).AddTo(this);

        FadeManager.Instance.NameAnimCompletedObserver
                .TakeUntilDestroy(this)
                .Subscribe(_ => _naviTMP.gameObject.SetActive(true));
    }
}
