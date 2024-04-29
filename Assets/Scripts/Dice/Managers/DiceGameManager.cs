using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Linq;

public class DiceGameManager : SingletonMonoBehaviour<DiceGameManager>
{
    private List<KeyValuePair<int, string>> _diceResultNameDic = new List<KeyValuePair<int, string>>();

    protected override void Awake()
    {
    }

    public async UniTask ResultReciever(int currentResult)
    {
        string currentName = NameLifeManager.Instance.CurrentNameReciever();
        _diceResultNameDic.Add(new KeyValuePair<int, string>(currentResult, currentName));
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
        _diceResultNameDic.Sort((x,y)=> x.Key.CompareTo(y.Key));

        int minkey = _diceResultNameDic[0].Key;
        List<string> loseNames = _diceResultNameDic.Where(entry => entry.Key == minkey)
                                                   .Select(entry => entry.Value)
                                                   .ToList();
        loseNames.ForEach(name => Debug.Log(name));
        loseNames.ForEach(name => NameLifeManager.Instance.ReduceLife(name));
    }
}
