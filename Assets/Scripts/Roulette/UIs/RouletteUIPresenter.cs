using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine.UI;


public class RouletteUIPresenter : PresenterBase
{
    [SerializeField]
    RouletteButton _rouletteButton;

    [SerializeField]
    RouletteController _rouletteController;

    [SerializeField]
    GameObject _rouletteMaker;

    protected override void Start()
    {
        _currentHideUIs.OnClickObserver
                      .Subscribe(_ =>
                      {
                          _nextActiveUIs.ToggleUIsVisibility();
                          _currentHideUIs.gameObject.SetActive(false);
                          _rouletteMaker.SetActive(true);
                       }).AddTo(this);

        _rouletteButton.RouletteButtonClickObserver.TakeUntilDestroy(this)
                                                   .Subscribe(clickCount =>
                                                   {
                                                       PresenterNotification(clickCount);
                                                   });
    }

    private void PresenterNotification(int count)
    {
        _rouletteController.RouletteRotate(count);
    }
}
