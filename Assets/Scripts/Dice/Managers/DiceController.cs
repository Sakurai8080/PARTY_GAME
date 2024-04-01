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
    [Header("変数")]
    [Tooltip("サイコロ")]
    [SerializeField]
    private List<Dice> _diceList = default;

    [Tooltip("サイコロの親オブジェクト")]
    [SerializeField]
    private Transform _diceParent = default;

    private Queue<Dice> _diceQueue = new Queue<Dice>();
    private Dictionary<Dice, Vector3> _diceInitPosDic = new Dictionary<Dice, Vector3>();

    public void DiceGenerate()
    {
        if (_diceQueue.Count() > 0)
        {
            while (_diceQueue.Count() > 0)
            {
                Dice reuseDice = _diceQueue.Dequeue();
                reuseDice.transform.position = _diceInitPosDic[reuseDice];
                reuseDice.gameObject.SetActive(true);
                OnRollDice(reuseDice);
            }
        }
        else if(_diceQueue.Count() <= 0)
        {
            for (int i = 0; i < 2; i++)
            {
                foreach (var dice in _diceList)
                {
                    Dice madeDice = Instantiate(dice, _diceParent);
                    _diceInitPosDic.Add(madeDice, madeDice.transform.localPosition);
                    OnRollDice(madeDice);
                    madeDice.InActiveObservar.Subscribe(_ => _diceQueue.Enqueue(madeDice));
                }
            }
        }
    }

    private void OnRollDice(Dice dice)
    {
        float randomRotateAmount = UnityEngine.Random.Range(-1000, 1000);
        Rigidbody diceRD = dice.GetComponent<Rigidbody>();
        dice.transform.DORotate(new Vector3(randomRotateAmount, randomRotateAmount), 0,RotateMode.FastBeyond360);
        diceRD.useGravity =true;
    }
}
