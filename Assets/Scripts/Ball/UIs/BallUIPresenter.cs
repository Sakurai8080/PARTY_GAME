using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class BallUIPresenter : PresenterBase
{
    [SerializeField]
    BallController _ballController;

    [SerializeField]
    CinemaChineController _cinemachineController;

    [SerializeField]
    BallFallButton _fallButton;

    protected override void Start()
    {
        _initUIButton.OnClickObserver
                      .Subscribe(_ =>
                      {
                          _mainUIsActivator.ToggleUIsVisibility();
                          _initUIButton.gameObject.SetActive(false);
                          _ballController.Setup();
                          _backGround.SetActive(false);
                      }).AddTo(this);
        
        _fallButton.FallButtonClickObserver
                   .TakeUntilDestroy(this)
                   .Subscribe(_ =>
                   {
                  //     _cinemachineController.;
                       _ballController.RotateBallParent();
                   });
    }
}
