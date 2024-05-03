#define DebugTest 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;

/// <summary>
/// サイコロゲームのプレゼンター
/// </summary>
public class DiceUIPresenter : PresenterBase
{
    [Tooltip("説明画面の背景")]
    [SerializeField]
    private GameObject _backGround = default;

    [Tooltip("サイコロを振るボタン")]
    [SerializeField]
    private DiceRollButton _diceRollButton = default;

    [SerializeField]
    private TextMeshProUGUI _inGameTMP = default;

    [SerializeField]
    private DiceResultTMP _diceResultTMP = default;

    protected override void Start()
    {
        base.Start();
        _activeSwitchButton.OnClickObserver
                       .TakeUntilDestroy(this)
                       .Subscribe(_ =>
                       {
                           _backGround.SetActive(false);
                           CinemaChineController.Instance.DollySet(CameraType.cam1, CameraType.cam2, InGameUIsActivator);
                       });

        _diceRollButton.IsRollObserver
                       .TakeUntilDestroy(this)
                       .Subscribe(_ =>
                       {
                           _inGameTMP.gameObject.SetActive(false);
                           DiceController.Instance.DiceGenerate();
                           CinemaChineController.Instance.DiceCheckMove(InGameUIsActivator);
                       });

        DiceController.Instance.CaliculatedObserver
                               .TakeUntilDestroy(this)
                               .Subscribe(result =>
                               {
                                   _diceResultTMP.FadeTMP(result);
                               });

    }

    private void InGameUIsActivator()
    {
        _diceRollButton.gameObject.SetActive(true);
        _inGameTMP.gameObject.SetActive(true);
        _diceRollButton.GetComponent<Button>().interactable = true;
    }
}