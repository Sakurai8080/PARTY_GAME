using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UIPresenter : MonoBehaviour
{
    public IObservable<Unit> UIGroupObserver => _presenterSubject;

    Subject<Unit> _presenterSubject = new Subject<Unit>();

    [SerializeField]
    TweenUIsController _testUIActivator;

    [SerializeField]
    ActiveUIInput _activeUIInput;

    void Start()
    {
        _activeUIInput.OnClickObserver
                      .Subscribe(_ =>
                      {
                          _testUIActivator.ToggleUIsVisibility();
                      }).AddTo(this);
    }
}
