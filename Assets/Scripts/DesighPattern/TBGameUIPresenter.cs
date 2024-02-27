using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine.UI;

public class TBGameUIPresenter : MonoBehaviour
{
    public IObservable<Unit> MainUIActiveObserver => _mainUIActiveSubject;

    Subject<Unit> _mainUIActiveSubject = new Subject<Unit>();

    [SerializeField]
    UIsActiveController _tbButtonUIActivator;

    [SerializeField]
    ActiveUIInput _uiActiveInput;

    void Start()
    {
        _uiActiveInput.OnClickObserver
                      .Subscribe(_ =>
                      {
                          _tbButtonUIActivator.ToggleUIsVisibility();
                          _uiActiveInput.gameObject.SetActive(false);
                      }).AddTo(this);
    }
}
