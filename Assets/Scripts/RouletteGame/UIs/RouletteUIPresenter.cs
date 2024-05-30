using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using TMPro;
using DG.Tweening;

/// <summary>
/// ルーレット関係のUIプレゼンター
/// </summary>
public class RouletteUIPresenter : PresenterBase
{
    [Tooltip("ルーレットのボタン")]
    [SerializeField]
    private RouletteButton _rouletteButton;

    [Tooltip("ルーレットを描画するコンポーネント")]
    [SerializeField]
    private RouletteMaker _rouletteMaker;

    [Tooltip("カウントアップを説明するTMPのアニメ")]
    [SerializeField]
    private NaviTextAnimation _naviTextAnimation;

    private const string _stopNaviText = "ボタンでルーレットストップ。";

    private RouletteStartButtonAnim _rouletteStartButtonAnim;
    private TextMeshProUGUI _naviTMP;

    protected override void Start()
    {
        _activeSwitchButton.OnClickObserver
                  .Subscribe(_ =>
                  {
                      ToggleUIsVisibility();
                      _hideUIGroup.gameObject.SetActive(false);
                      _naviTextAnimation.AnimationStart();
                  }).AddTo(this);

        _rouletteStartButtonAnim = _rouletteButton.GetComponent<RouletteStartButtonAnim>();
        _naviTMP = _naviTextAnimation.GetComponent<TextMeshProUGUI>();

        _rouletteButton.RouletteButtonClickObserver.TakeUntilDestroy(this)
                                                   .Subscribe(clickCount =>
                                                   {
                                                       if (clickCount == 1)
                                                           _naviTMP.text = _stopNaviText;
                                                       if (clickCount == 2)
                                                           _naviTMP.DOFade(0, 0.25f)
                                                                   .SetEase(Ease.Linear);

                                                       PresenterNotification(clickCount);
                                                       _rouletteStartButtonAnim.StopAnimation(clickCount);
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
