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
    public Dictionary<string, int> NameLifeDic => _nameLifeDic;
    public int GamePlayerAmount => _gamePlayerAmount;

    Dictionary<string, int> _nameLifeDic = new Dictionary<string, int>();

    /// <summary>参加人数</summary>
    private int _gamePlayerAmount = 0;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void Setup(List<string> name)
    {
        _gamePlayerAmount = name.Count();
        for (int i = 0; i < name.Count; i++)
        {
            _nameLifeDic.Add(name[i], 0);
        }
    }
}
