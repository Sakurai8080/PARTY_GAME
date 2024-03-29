using System.Collections;
using System.Collections.Generic;
using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;
using System.Threading;

public class Dice : MonoBehaviour
{
    public IObservable<Unit> InActiveObservar => _inactiveSubject;

    private Subject<Unit> _inactiveSubject = new Subject<Unit>();

    private void Start()
    {
        Invoke("ReturnPool", 5);   
    }

    private void OnEnable()
    {
        Invoke("ReturnPool", 5);
    }

    private void OnDisable()
    {
        _inactiveSubject.OnNext(Unit.Default);
    }

    public void ReturnPool()
    {
        gameObject.SetActive(false);
    }
}
