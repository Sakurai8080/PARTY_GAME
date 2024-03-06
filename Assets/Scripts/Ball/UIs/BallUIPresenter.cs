using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class BallUIPresenter : PresenterBase
{
    [SerializeField]
    BallController _ballController;

    protected override void Start()
    {
        _uiActiveInput.OnClickObserver
                      .Subscribe(_ =>
                      {
                          _mainUIsActivator.ToggleUIsVisibility();
                          _uiActiveInput.gameObject.SetActive(false);
                          _ballController.Setup();
                          _backGround.SetActive(false);
                      }).AddTo(this);
    }
}
