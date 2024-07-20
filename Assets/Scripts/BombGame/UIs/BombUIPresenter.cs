using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using DG.Tweening;

/// <summary>
/// BombGameのUI同士を仲介するプレゼンター
/// </summary>
public class BombUIPresenter : PresenterBase
{
    [Tooltip("選択を指示する説明用TMP")]
    [SerializeField]
    private TextMeshProUGUI _naviTMP = default;

    [Tooltip("カードのボタン")]
    [SerializeField]
    private List<Button> _bombButtonList = new List<Button>();

    private List<BombSelectButton> _bombSelectButton = new List<BombSelectButton>();


    protected override void Start()
    {

        _bombSelectButton?.Clear();

        _activeSwitchButton.OnClickObserver
                    .Subscribe(_ =>
                    {
                        ToggleUIsVisibility();
                        _hideUIGroup.gameObject.SetActive(false);
                    }).AddTo(this);

        NaviTextAnimation naviTextAnimation = _naviTMP.GetComponent<NaviTextAnimation>();
        FadeManager.Instance.NameAnimStartObserver
                    .TakeUntilDestroy(this)
                    .Subscribe(_ =>
                    {
                        _naviTMP.DOFade(1, 0.25f);
                        naviTextAnimation.AnimationStart();
                        _currentOrderUIs.CurrentNameGroupFade(NameFadeType.In);
                        _currentOrderUIs.CurrentNameActivator();
                    });

        _bombButtonList.ForEach(button => _bombSelectButton.Add(button.GetComponent<BombSelectButton>()));

        _activeSwitchButton.OnClickObserver
                           .TakeUntilDestroy(this)
                           .Subscribe(_ => Setup());

        _bombSelectButton.ForEach(button => button.OnClickObserver
                                                 .TakeUntilDestroy(this)
                                                 .Subscribe(bombAnim =>
                                                 {
                                                     naviTextAnimation.StopAnimation();
                                                     BombUIsAnimationController.Instance.CardSelected(bombAnim);
                                                     bombAnim.SelectedAnimation();
                                                     _naviTMP.DOFade(0, 0.25f);
                                                     _currentOrderUIs.CurrentNameGroupFade(NameFadeType.Out);
                                                 }));
    }

    /// <summary>
    /// 初期セットアップ
    /// </summary>
    private void Setup()
    {
        List<Image> imageList = _bombButtonList.Select(button => button.image).ToList();
        BombGameManager.Instance.CardSet(imageList);
        BombUIsAnimationController.Instance.InitSet(_bombButtonList);
    }
}
