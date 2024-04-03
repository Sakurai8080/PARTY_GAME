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
using System.Linq;

public class Dice : MonoBehaviour
{
    public int Result => _result;

    public IObservable<Unit> InActiveObservar => _inactiveSubject;

    [Header("変数")]
    [Tooltip("サイコロの各目の位置")]
    [SerializeField]
    private List<Transform> _dicePoints = default;

    private int _result = 0;
    Rigidbody _rd;

    private Subject<Unit> _inactiveSubject = new Subject<Unit>();
    private Subject<Unit> _diceStopSubject = new Subject<Unit>();

    private void Start()
    {
        Invoke("ReturnPool", 10);
        _rd = GetComponent<Rigidbody>();
        _diceStopSubject.Subscribe(_ => CheckResult());
        StartCoroutine(StopCheckCoroutine());
    }

    private void OnEnable()
    {
        Invoke("ReturnPool", 10);
    }

    private void OnDisable()
    {
        _inactiveSubject.OnNext(Unit.Default);
    }

    public void ReturnPool()
    {
        gameObject.SetActive(false);
    }

    public void CheckResult()
    {
        int resultPoint = _dicePoints.Select((point, index) => new { Index = index, YPos = point.position.y })
                                      .OrderByDescending(item => item.YPos)
                                      .First().Index + 1;

        Debug.Log(resultPoint);
        _result = resultPoint;
    }

    private IEnumerator StopCheckCoroutine()
    {
        WaitForSeconds waitTime = new WaitForSeconds(0.1f);
        while(true)
        {
            yield return waitTime;
            if (_rd.IsSleeping())
            {
                _diceStopSubject.OnNext(Unit.Default);
                break;
            }
        }
    }
}
