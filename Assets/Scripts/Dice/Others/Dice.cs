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

/// <summary>
/// サイコロの機能
/// </summary>
public class Dice : MonoBehaviour
{
    public int Result => _result;

    public IObservable<Unit> InActiveObservar => _inactiveSubject;
    public IObservable<Unit> DiceStopObserver => _diceStopSubject;
    public IObservable<int> CheckedObserver => _checkedSubject;


    [Header("変数")]
    [Tooltip("サイコロの各目の位置")]
    [SerializeField]
    private List<Transform> _dicePoints = default;

    private int _result = 0;
    private Rigidbody _rd;
    private Vector3 _initPosition;
    private Coroutine _currentCoroutine;
    private bool _onSE = false;

    private Subject<Unit> _inactiveSubject = new Subject<Unit>();
    private Subject<Unit> _diceStopSubject = new Subject<Unit>();
    private Subject<int> _checkedSubject = new Subject<int>();

    private void Start()
    {
        _initPosition = transform.position;
        _rd = GetComponent<Rigidbody>();
        _diceStopSubject.TakeUntilDestroy(this).Subscribe(_ => CheckResult());
        FadeManager.Instance.NameAnimCompletedObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(async _ =>
                            {
                                await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
                                ReturnPool();
                            });
    }

    private void OnEnable()
    {
        _currentCoroutine = StartCoroutine(StopCheckCoroutine());
    }

    private void OnDisable()
    {
        transform.position = _initPosition;
        if (_currentCoroutine != null)
            _currentCoroutine = null;
        _inactiveSubject.OnNext(Unit.Default);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Bowl") && !_onSE)
        {
            _onSE = true;
            AudioManager.Instance.PlaySE(SEType.Dice);
        }
    }

    /// <summary>
    /// プールに戻す
    /// </summary>
    public void ReturnPool()
    {
        gameObject.SetActive(false);
        _onSE = false;
    }

    /// <summary>
    /// サイコロが止まった際の自身の目の確認
    /// </summary>
    public void CheckResult()
    {
        int resultPoint = _dicePoints.Select((point, index) => new { Index = index, YPos = point.position.y })
                                      .OrderByDescending(item => item.YPos)
                                      .First().Index + 1;

        Debug.Log(resultPoint);
        _result = resultPoint;
        _checkedSubject.OnNext(_result);
    }

    /// <summary>
    /// サイコロが止まったかどうかの確認
    /// </summary>
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
