using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Linq;

/// <summary>
/// サイコロゲームを管理するマネージャー
/// </summary>
public class DiceGameManager : SingletonMonoBehaviour<DiceGameManager>
{
    private List<KeyValuePair<int, string>> _diceResultNameDic = new List<KeyValuePair<int, string>>();

    protected override void Awake(){}

    /// <summary>
    /// サイコロの結果を名前と紐づける処理
    /// </summary>
    /// <param name="currentResult">サイコロの和の結果</param>
    /// <returns></returns>
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

    /// <summary>
    /// 最終的に負けたプレイヤーの確認
    /// </summary>
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
