#define DebugTest 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

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

    protected override void Start()
    {
        base.Start();
        _activeSwitchButton.OnClickObserver
                       .TakeUntilDestroy(this)
                       .Subscribe(_ =>
                           _backGround.SetActive(false));

        _diceRollButton.IsRollObserver
                       .TakeUntilDestroy(this)
                       .Subscribe(_ =>
                       {
                           DiceController.Instance.DiceGenerate();
#if DebugTest
                           Invoke("TestSE", 0.5f);
#endif
                       });
    }

#if DebugTest
    //todo:テスト用。
    private void TestSE()
    {
        _se.Play();
    }
#endif
}
