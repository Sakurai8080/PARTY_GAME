using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class DiceUIPresenter : PresenterBase
{
    [Tooltip("説明画面の背景")]
    [SerializeField]
    private GameObject _backGround = default;

    [Tooltip("サイコロを振るボタン")]
    [SerializeField]
    private DiceRollButton _diceRollButton = default;

    protected override void Start()
    {
        base.Start();
        _activeSwitchButton.OnClickObserver
                       .TakeUntilDestroy(this)
                       .Subscribe(_ =>
                       {
                           _backGround.SetActive(false);
                       });

        _diceRollButton.IsRollObserver
                       .TakeUntilDestroy(this)
                       .Subscribe(_ =>
                       {
                           DiceController.Instance.DiceGenerate();
                       });
    }
}
