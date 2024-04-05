using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Linq;

public class DiceGameManager : SingletonMonoBehaviour<DiceGameManager>
{
    private SortedList<int,string> _diceResultNameDic = new SortedList<int, string>();

    public async UniTask ResultReciever(int currentResult)
    {
        string currentName = NameLifeManager.Instance.CurrentNameReciever();
        _diceResultNameDic.Add(currentResult, currentName);
        Debug.Log($"{currentName}:{currentResult}");
        NameLifeManager.Instance.NameListOrderChange();
        if (_diceResultNameDic.Count() >= NameLifeManager.Instance.GamePlayerAmount)
        {
            loseCheck();
            await UniTask.Delay(TimeSpan.FromSeconds(4));
            GameManager.Instance.SceneLoader("GameSelect");
        }
    }

    private void loseCheck()
    {
        int minNum = _diceResultNameDic.Keys[0];
        string loseName = _diceResultNameDic[minNum];
        NameLifeManager.Instance.ReduceLife(loseName);
    }
}
