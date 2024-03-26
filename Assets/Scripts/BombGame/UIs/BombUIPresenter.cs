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

    protected override void Start()
    {
        base.Start();
        _currentHideUIs.OnClickObserver
                      .Subscribe(_ =>
                      {
                          Setup();
                      }).AddTo(this);
    }

    private void Setup()
    {
        BombGameManager.Instance.CardSet(_bombCardImageList);
        BombUIsAnimationController.Instance.InitSet(_bombButtonList);
    }
}
