using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PresenterBase : MonoBehaviour
{
    public IObservable<Unit> MainUIActiveObserver => _mainUIActiveSubject;

    Subject<Unit> _mainUIActiveSubject = new Subject<Unit>();

    [SerializeField]
    protected UIsActiveController _mainUIsActivator;

    [SerializeField]
    protected ActiveUIInput _uiActiveInput;

    [SerializeField]
    protected GameObject _backGround;

    protected virtual void Start()
    {
        _uiActiveInput.OnClickObserver
                      .Subscribe(_ =>
                      {
                          _mainUIsActivator.ToggleUIsVisibility();
                          _uiActiveInput.gameObject.SetActive(false);
                      }).AddTo(this);
    }
}
