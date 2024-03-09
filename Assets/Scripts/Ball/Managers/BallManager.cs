using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine.UI;
using Cinemachine;

//ボールと名前の紐づけ
public class BallManager : SingletonMonoBehaviour<BallManager>
{

    public IObservable<bool> InGameObserver => _inGame;

    private ReactiveProperty<bool> _inGame = new ReactiveProperty<bool>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameStateSwitcher(bool onValid)
    {
        _inGame.Value = onValid;
    }
}
