using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;

public class NameLifeManager : SingletonMonoBehaviour<NameLifeManager>
{
    public List<string> NameList => _nameList;
    public Dictionary<string, int> NameLifeDic => _nameLifeDic;
    public int GamePlayerAmount => _gamePlayerAmount;

    List<string> _nameList = new List<string>();
    Dictionary<string, int> _nameLifeDic = new Dictionary<string, int>();

    /// <summary>参加人数</summary>
    private int _gamePlayerAmount = 0;
    private int _currentOrder = 0;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void Setup(HashSet<string> names)
    {
        if (names.Count() >= 1)
        {
            _gamePlayerAmount = names.Count();
            names.ToList().ForEach(name => _nameLifeDic.Add(name, 3));
            _nameList.AddRange(names);
        }
    }

    public void ReduceLife(string loseName)
    {
        _nameLifeDic[loseName]--;
        //TODO:0になったらゲームマネージャーでゲームオーバーを呼び出す
        foreach (var item in _nameLifeDic.Keys)
        {
            Debug.Log($"{item} : {_nameLifeDic[item]}");
        }
    }

    public int NamefromLifePass(string checkName)
    {
        return _nameLifeDic[checkName];
    }

    public void NameListOrderChange()
    {
        _currentOrder++;
        if (_currentOrder >= _gamePlayerAmount)
        {
            _currentOrder = 0;
        }
    }

    public string CurrentNameReciever()
    {
        return _nameList[_currentOrder];
    }
}
