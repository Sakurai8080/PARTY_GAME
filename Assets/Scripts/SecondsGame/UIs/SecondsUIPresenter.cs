using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using TMPro;

/// <summary>
/// 10秒ゲームのUIプレゼンター
/// </summary>
public class SecondsUIPresenter : PresenterBase
{
    [Tooltip("カウント操作するボタン")]
    [SerializeField]
    private CountUpButton _countUpButton;

    [Tooltip("秒数表示用コンポーネント")]
    [SerializeField]
    private SecondsDisplay _seconsDisplay;

    [Tooltip("カウントアップを説明するTMP")]
    [SerializeField]
    private TextMeshProUGUI _naviTMP;

    [Tooltip("カウントを切り替えるボタンアニメ")]
    [SerializeField]
    private SecoundsToggleUIAnim _secoundsToggleUIAnim;

    [Tooltip("カウントアップを説明するTMPのアニメ")]
    [SerializeField]
    private NaviTextAnimation _naviTextAnimation;

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
                              _secoundsToggleUIAnim.AnimationStart();
                              _currentOrderUIs.gameObject.SetActive(false);
                              _naviTextAnimation.StopAnimation();
                              _naviTMP.gameObject.SetActive(false);
                              SecondsController.Instance.ToggleInProgress(value);
                              SecondsController.Instance.SecondsCountUpAsync().Forget();
                              _countUpButton.TextToggle(value);
                          }
                          else
                          {
                              _secoundsToggleUIAnim.StopAnimation();
                              _seconsDisplay.ResultTMPActivator();
                              SecondsController.Instance.ToggleInProgress(value);
                          }
                      });

        FadeManager.Instance.NameAnimCompletedObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(_ =>
                            {
                                _naviTMP.gameObject.SetActive(true);
                                _naviTextAnimation.AnimationStart();
                                _countUpButton.TextToggle(false);
                                _currentOrderUIs.gameObject.SetActive(true);
                                _currentOrderUIs.CurrentNameActivator();
                            });
    }
}
