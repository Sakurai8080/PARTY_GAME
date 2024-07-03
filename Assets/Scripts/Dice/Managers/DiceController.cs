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
/// サイコロの操作
/// </summary>
public class DiceController : SingletonMonoBehaviour<DiceController>
{
    public IObservable<int> CaliculatedObserver => _calculatedSubject;

    [Header("変数")]
    [Tooltip("サイコロ")]
    [SerializeField]
    private List<Dice> _diceList = default;

    [Tooltip("サイコロの親オブジェクト")]
    [SerializeField]
    private Transform _diceParent = default;

    private Queue<Dice> _diceQueue = new Queue<Dice>();
    private List<int> _resultDice = new List<int>();
    private const int GENERATE_MULT = 2;

    private Subject<int> _calculatedSubject = new Subject<int>();

    protected override void Awake(){}

    /// <summary>
    /// サイコロの生成(プール機能)
    /// </summary>
    public void DiceGenerate()
    {
        if (_diceQueue.Count() > 0)
        {
            while (_diceQueue.Count() > 0)
            {
                Dice reuseDice = _diceQueue.Dequeue();
                reuseDice.gameObject.SetActive(true);
                OnRollDice(reuseDice);
            }
        }
        else if(_diceQueue.Count() <= 0)
        {
            for (int i = 0; i < GENERATE_MULT; i++)
            {
                foreach (var dice in _diceList)
                {
                    Dice madeDice = Instantiate(dice, _diceParent);
                    madeDice.CheckedObserver.Subscribe(value => DiceResultChecker(value));
                    OnRollDice(madeDice);
                    madeDice.InActiveObservar.Subscribe(_ => _diceQueue.Enqueue(madeDice));
                }
            }
        }
    }

    /// <summary>
    /// サイコロの結果確認
    /// </summary>
    /// <param name="diceResult">一つ一つのサイコロの結果</param>
    private void DiceResultChecker(int diceResult)
    {
        _resultDice.Add(diceResult);
        if (_resultDice.Count() >= _diceList.Count * GENERATE_MULT)
        {
            int finalResult = DiceCalc.DiceSum(_resultDice.ToArray());
            Debug.Log($"サイコロの結果 : {finalResult}");
            _resultDice.Clear();
            ResultPass(finalResult);
            _calculatedSubject.OnNext(finalResult);
        }
    }

    /// <summary>
    /// 結果をマネージャーに渡す
    /// </summary>
    /// <param name="res">合計の結果</param>
    private void ResultPass(int res)
    {
        DiceGameManager.Instance.ResultReciever(res);
    }

    /// <summary>
    /// サイコロを振る処理
    /// </summary>
    /// <param name="dice">振るサイコロ</param>
    private void OnRollDice(Dice dice)
    {
        float randomRotateAmount = UnityEngine.Random.Range(-360, 360);
        dice.transform.DORotate(new Vector3(randomRotateAmount, randomRotateAmount,randomRotateAmount), 0.7f,RotateMode.FastBeyond360);
    }
}
