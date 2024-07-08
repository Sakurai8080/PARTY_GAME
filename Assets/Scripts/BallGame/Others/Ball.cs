using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UniRx;
using System;

/// <summary>
/// ボールの機能
/// </summary>
public class Ball : MonoBehaviour
{
    public IObservable<Ball> GoaledObserver => _goaledSubject;

    private Subject<Ball> _goaledSubject = new Subject<Ball>();

    private void OnTriggerEnter(Collider other)
    {
        _goaledSubject.OnNext(this);
    }
}
