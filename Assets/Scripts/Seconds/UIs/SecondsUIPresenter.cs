using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;
using TMPro;

public class SecondsUIPresenter : PresenterBase
{
    [SerializeField]
    CountUpButton _countUpButton;

    [SerializeField]
    NameSecondDisplay _nameSecoundDisplay;

    protected override void Start()
    {
        _currentHideUIs.OnClickObserver
                       .TakeUntilDestroy(this)
                       .Subscribe(_ =>
                       {
                           _nextActiveUIs.ToggleUIsVisibility();
                           _currentHideUIs.gameObject.SetActive(false);
                       });

        _countUpButton.InProgressObservable
                      .TakeUntilDestroy(this)
                      .Subscribe(value=>
                      {
                          if (value)
                          {
                              SecondsController.Instance.ToggleInProgress(value);
                              SecondsController.Instance.SecondsCountUpAsync().Forget();
                          }
                          else
                          {
                              _nameSecoundDisplay.ResultTMPActivator();
                              SecondsController.Instance.ToggleInProgress(value);
                          }
                      });
    }
}
