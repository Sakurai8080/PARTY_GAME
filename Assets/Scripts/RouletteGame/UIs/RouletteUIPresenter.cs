using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

/// <summary>
/// ルーレット関係のUIプレゼンター
/// </summary>
public class RouletteUIPresenter : PresenterBase
{
    [Tooltip("ルーレットのボタン")]
    [SerializeField]
    private RouletteButton _rouletteButton;

    [Tooltip("矢印のアニメーションコンポーネント")]
    [SerializeField]
    private ArrowImageAnim _arrowImageAnim;

    [Tooltip("ルーレットを描画するコンポーネント")]
    [SerializeField]
    private RouletteMaker _rouletteMaker;

    private RouletteStartButtonAnim _rouletteStartButtonAnim;

    protected override void Start()
    {
        _activeSwitchButton.OnClickObserver
                  .Subscribe(_ =>
                  {
                      ToggleUIsVisibility();
                      _hideUIGroup.gameObject.SetActive(false);
                  }).AddTo(this);

        _rouletteStartButtonAnim = _rouletteButton.GetComponent<RouletteStartButtonAnim>();

        _rouletteButton.RouletteButtonClickObserver.TakeUntilDestroy(this)
                                                   .Subscribe(clickCount =>
                                                   {
                                                       PresenterNotification(clickCount);
                                                       _rouletteStartButtonAnim.UILoopAnimation(clickCount);
                                                   });

        _rouletteMaker.RouletteMadeObserver.TakeUntilDestroy(this)
                                           .Subscribe(_ => _arrowImageAnim.ImagePlayAnimation());
                                           
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
