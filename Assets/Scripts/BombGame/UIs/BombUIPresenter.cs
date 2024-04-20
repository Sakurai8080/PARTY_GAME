using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// BombGameのUI同士を仲介するプレゼンター
/// </summary>
public class BombUIPresenter : PresenterBase
{
    [Tooltip("カードのボタン")]
    [SerializeField]
    private List<Button> _bombButtonList = new List<Button>();

    List<BombSelectButton> _bombSelectButton = new List<BombSelectButton>();

    protected override void Start()
    {
        base.Start();
        _bombButtonList.ForEach(button => _bombSelectButton.Add(button.GetComponent<BombSelectButton>()));

        _activeSwitchButton.OnClickObserver
                           .TakeUntilDestroy(this)
                           .Subscribe(_ => Setup());

        _bombSelectButton.ForEach(button => button.OnClickObserver
                                                 .TakeUntilDestroy(this)
                                                 .Subscribe(bombAnim =>
                                                 {
                                                     BombUIsAnimationController.Instance.CardSelected(bombAnim);
                                                     bombAnim.SelectedAnimation();
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
