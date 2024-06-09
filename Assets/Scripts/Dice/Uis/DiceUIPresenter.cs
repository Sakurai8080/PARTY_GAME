using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;
using DG.Tweening;

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

    [Tooltip("次の動作を指示するTMP")]
    [SerializeField]
    private TextMeshProUGUI _naviTMP = default;

    [Tooltip("サイコロの結果を表示するTMP")]
    [SerializeField]
    private DiceResultTMP _diceResultTMP = default;

    protected override void Start()
    {
        base.Start();

        NaviTextAnimation naviTextAnimation = _naviTMP.GetComponent<NaviTextAnimation>();

        _activeSwitchButton.OnClickObserver
                       .TakeUntilDestroy(this)
                       .Subscribe(_ =>
                       {
                           _backGround.SetActive(false);
                           CinemaChineController.Instance.DollySet(CameraType.cam1, CameraType.cam2);
                       });

        _diceRollButton.IsRollObserver
                       .TakeUntilDestroy(this)
                       .Subscribe(_ =>
                       {
                           _currentOrderUIs.gameObject.SetActive(false);
                           naviTextAnimation.StopAnimation();
                           DiceController.Instance.DiceGenerate();
                           CinemaChineController.Instance.DiceCheckMove();
                           _naviTMP.DOFade(0, 0.25f);
                           _diceRollButton.gameObject.SetActive(false);
                       });

        DiceController.Instance.CaliculatedObserver
                               .TakeUntilDestroy(this)
                               .Subscribe(result => _diceResultTMP.FadeTMP(result));

        FadeManager.Instance.NameAnimStartObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(_ =>
                            {
                                _diceRollButton.gameObject.SetActive(true);
                                _naviTMP.DOFade(1, 0.25f);
                                naviTextAnimation.AnimationStart();
                                _diceRollButton.GetComponent<Button>().interactable = true;
                            });
    }
}