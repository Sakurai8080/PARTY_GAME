using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;


public class PeopleAmountButton : MonoBehaviour
{

    public IObservable<int> PeopleButtonClickObserver => _peopleButtonClickSubject;

    [SerializeField]
    private Button _peopleAmountButton = default;

    [SerializeField]
    private int _peopleAmount = default;

    private Subject<int> _peopleButtonClickSubject = new Subject<int>();

    private void Start()
    {
        _peopleAmountButton.OnClickAsObservable()
                           .TakeUntilDestroy(this)
                           .Subscribe(_ =>
                           {
                               _peopleButtonClickSubject.OnNext(_peopleAmount);
                           });
    }
}
