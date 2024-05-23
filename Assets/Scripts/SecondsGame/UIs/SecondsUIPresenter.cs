using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;

/// <summary>
/// 10秒ゲームのUIプレゼンター
/// </summary>
public class SecondsUIPresenter : PresenterBase
{
    [Tooltip("カウント操作するボタン")]
    [SerializeField]
    CountUpButton _countUpButton;

    [Tooltip("秒数表示用コンポーネント")]
    [SerializeField]
    SecondsDisplay _seconsDisplay;

    protected override void Start()
    {
        _activeSwitchButton.OnClickObserver
                      .Subscribe(_ =>
                      {
                          FadeManager.Instance.OrderChangeFadeAnimation().Forget();
                          ToggleUIsVisibility();
                          _hideUIGroup.gameObject.SetActive(false);
                      }).AddTo(this);

        _countUpButton.InProgressObservable
                      .TakeUntilDestroy(this)
                      .Subscribe(value=>
                      {
                          if (value)
                          {
                              _currentOrderUIs.gameObject.SetActive(false);
                              SecondsController.Instance.ToggleInProgress(value);
                              SecondsController.Instance.SecondsCountUpAsync().Forget();
                          }
                          else
                          {
                              _seconsDisplay.ResultTMPActivator();
                              SecondsController.Instance.ToggleInProgress(value);
                          }
                      });

        FadeManager.Instance.NameAnimCompletedObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(_ =>
                            {
                                _currentOrderUIs.gameObject.SetActive(true);
                                _currentOrderUIs.CurrentNameActivator();
                            });
    }
}
