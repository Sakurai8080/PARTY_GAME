#define DebugTest 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;

public class DiceUIPresenter : PresenterBase
{
    [Tooltip("説明画面の背景")]
    [SerializeField]
    private GameObject _backGround = default;

    [Tooltip("サイコロを振るボタン")]
    [SerializeField]
    private DiceRollButton _diceRollButton = default;

    [SerializeField]
    private AudioSource _se = default;

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
                           CinemaChineController.Instance.DollySet(InGameUIsActivator);
                       });
    
        _diceRollButton.IsRollObserver
                       .TakeUntilDestroy(this)
                       .Subscribe(_ =>
                       {
                           _inGameTMP.gameObject.SetActive(false);
                           DiceController.Instance.DiceGenerate();
                           CinemaChineController.Instance.DiceCheckMove(InGameUIsActivator);
#if DebugTest
                           Invoke("TestSE", 0.5f);
#endif
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

#if DebugTest
    //todo:テスト用。
    private void TestSE()
    {
        _se.Play();
    }
#endif
}
