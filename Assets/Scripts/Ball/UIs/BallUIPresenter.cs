using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class BallUIPresenter : PresenterBase
{
    [SerializeField]
    GameObject _ballParent;


    protected override void Start()
    {
        _uiActiveInput.OnClickObserver
                      .Subscribe(_ =>
                      {
                          _mainUIsActivator.ToggleUIsVisibility();
                          _uiActiveInput.gameObject.SetActive(false);
                          _ballParent.SetActive(true);
                          _backGround.SetActive(false);
                      }).AddTo(this);
    }
}
