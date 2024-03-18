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

public class CountUpButton : MonoBehaviour
{
    public IObservable<bool> InProgressObservable => _inProgressSubject;
    private Subject<bool> _inProgressSubject = new Subject<bool>();

    [SerializeField]
    Button _countUpButton = default;

    private bool _inProgress = false;

    private void Start()
    {
        _countUpButton.OnClickAsObservable()
                      .TakeUntilDestroy(this)
                      .Subscribe(_ =>
                      {
                          _inProgress = !_inProgress;
                          _inProgressSubject.OnNext(_inProgress);
                      });
    }
}
