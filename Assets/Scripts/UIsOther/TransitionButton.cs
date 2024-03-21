using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;

public class TransitionButton : MonoBehaviour
{
 public IObservable<Unit> NextClickObserver => _nextClickSubject;

    [SerializeField]
    private Button _transitionButton = default;

    private Subject<Unit> _nextClickSubject = new Subject<Unit>();

    private void Start()
    {
        _transitionButton.OnClickAsObservable()
                         .TakeUntilDestroy(this)
                         .Subscribe(_ =>
                         {
                             _nextClickSubject.OnNext(Unit.Default);
                         });
    }
}
