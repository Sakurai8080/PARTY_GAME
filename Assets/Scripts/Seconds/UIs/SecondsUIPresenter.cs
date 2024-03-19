using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

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
                      .Subscribe(async value=>
                      {
                          if (value)
                          {
                              SecondsController.Instance.ToggleInProgress(value);
                              await SecondsController.Instance.SecondsCountUpAsync();
                          }
                          else
                          {
                              _nameSecoundDisplay.ResultTMPActivator();
                              SecondsController.Instance.ToggleInProgress(value);
                          }
                      });
    }
}
