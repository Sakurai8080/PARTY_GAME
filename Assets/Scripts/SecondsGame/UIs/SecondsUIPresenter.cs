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
        _currentHideUIs.OnClickObserver
                       .TakeUntilDestroy(this)
                       .Subscribe(_ =>
                       {
                           _nextActiveUIs.ToggleUIsVisibility();
                           _currentHideUIs.gameObject.SetActive(false);
                       });

        _countUpButton.InProgressObservable
                      .TakeUntilDestroy(this)
                      .Subscribe(value=>
                      {
                          if (value)
                          {
                              SecondsController.Instance.ToggleInProgress(value);
                              SecondsController.Instance.SecondsCountUpAsync().Forget();
                          }
                          else
                          {
                              _seconsDisplay.ResultTMPActivator();
                              SecondsController.Instance.ToggleInProgress(value);
                          }
                      });
    }
}
