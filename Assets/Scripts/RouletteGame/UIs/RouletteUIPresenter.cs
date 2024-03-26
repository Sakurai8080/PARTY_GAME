using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

/// <summary>
/// ルーレット関係のUIプレゼンター
/// </summary>
public class RouletteUIPresenter : PresenterBase
{
    [Header("変数")]
    [Tooltip("ルーレットのボタン")]
    [SerializeField]
    private RouletteButton _rouletteButton;

    private RouletteStartButtonAnim _rouletteStartButtonAnim;

    protected override void Start()
    {
        _rouletteStartButtonAnim = _rouletteButton.GetComponent<RouletteStartButtonAnim>();
        _currentHideUIs.OnClickObserver
                       .Subscribe(_ =>
                       {
                           _nextActiveUIs.ToggleUIsVisibility();
                           _currentHideUIs.gameObject.SetActive(false);
                       }).AddTo(this);

        _rouletteButton.RouletteButtonClickObserver.TakeUntilDestroy(this)
                                                   .Subscribe(clickCount =>
                                                   {
                                                       PresenterNotification(clickCount);
                                                       _rouletteStartButtonAnim.UILoopAnimation(clickCount);
                                                   });
    }

    /// <summary>
    /// 回転処理にクリック回数を渡す
    /// </summary>
    /// <param name="count">クリック回数</param>
    private void PresenterNotification(int count)
    {
        RouletteController.Instance.RouletteRotate(count);
    }
}
