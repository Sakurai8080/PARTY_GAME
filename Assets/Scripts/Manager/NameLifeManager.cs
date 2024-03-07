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
    public int GamePlayerAmount => _gamePlayerAmount;

    Dictionary<string, int> _lifeNameDic = new Dictionary<string, int>();
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
            _lifeNameDic.Add(name[i], 0);
        }
    }
}
