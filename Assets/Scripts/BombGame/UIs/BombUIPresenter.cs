using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

/// <summary>
/// BombGameのUI同士を仲介するプレゼンター
/// </summary>
public class BombUIPresenter : PresenterBase
{
    [Tooltip("カードの画像")]
    [SerializeField]
    private List<Image> _bombCardImageList = new List<Image>();

    [Tooltip("カードのボタン")]
    [SerializeField]
    private List<Button> _bombButtonList = new List<Button>();

    [Tooltip("セレクトボタン")]
    [SerializeField]
    private List<BombSelectButton> _bombSelectButton;

    protected override void Start()
    {
        base.Start();
        _activeSwitchButton.OnClickObserver
                           .Subscribe(_ =>
                           {
                               Setup();
                           }).AddTo(this);

        _bombSelectButton.ForEach(button => button.OnClickObserver
                                                 .TakeUntilDestroy(this)
                                                 .Subscribe(bombAnim =>
                                                 {
                                                     BombUIsAnimationController.Instance.CardSelected(bombAnim);
                                                     bombAnim.SelectedAnimation();
                                                 }));
    }

    private void Setup()
    {
        BombGameManager.Instance.CardSet(_bombCardImageList);
        BombUIsAnimationController.Instance.InitSet(_bombButtonList);
    }
}
