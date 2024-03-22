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
    public IObservable<int> NextClickObserver => _nextClickSubject;

    [SerializeField]
    private Button _transitionButton = default;

    private Subject<int> _nextClickSubject = new Subject<int>();

    private void Start()
    {
        int clickCount = 0;
        _transitionButton.OnClickAsObservable()
                         .TakeUntilDestroy(this)
                         .Subscribe(_ =>
                         {
                             clickCount++;
                             _nextClickSubject.OnNext(clickCount);
                         });
    }
}
