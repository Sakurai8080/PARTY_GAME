using System;
using UniRx;
using UnityEngine;

public abstract class PresenterBase : MonoBehaviour
{
    public IObservable<Unit> MainUIActiveObserver => _mainUIActiveSubject;

    Subject<Unit> _mainUIActiveSubject = new Subject<Unit>();

   
    [Header("変数")]
    [SerializeField]
    protected UIsActiveController _nextActiveUIs;

    [SerializeField]
    protected ActiveUIInput _currentHideUIs;

    protected virtual void Start()
    {
        _currentHideUIs.OnClickObserver
                      .Subscribe(_ =>
                      {
                          _nextActiveUIs.ToggleUIsVisibility();
                          _currentHideUIs.gameObject.SetActive(false);
                      }).AddTo(this);
    }
}
