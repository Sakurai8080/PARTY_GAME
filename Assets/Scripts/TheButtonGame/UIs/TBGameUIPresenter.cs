using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine.UI;

public class TBGameUIPresenter : PresenterBase
{
    protected override void Start()
    {
        base.Start();
        _currentHideUIs.OnClickObserver
                      .Subscribe(_ =>
                      {
                          TBGameManager.Instance.ButtonRandomHide();
                      }).AddTo(this);
    }
}
