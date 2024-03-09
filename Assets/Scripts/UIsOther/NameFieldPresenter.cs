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

public class NameFieldPresenter : PresenterBase
{
    protected override void Start()
    {
        _currentHideUIs.OnClickObserver
                      .Subscribe(_ =>
                      {
                          _nextActiveUIs.ToggleUIsVisibility();
                          _currentHideUIs.gameObject.SetActive(false);
                      }).AddTo(this);
    }
}
