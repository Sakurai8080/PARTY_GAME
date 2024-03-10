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
public class BallGameManager : SingletonMonoBehaviour<BallGameManager>
{
    public IReadOnlyReactiveProperty<bool> InGame => _inGame;

    public IObservable<bool> InGameObserver => _inGame;

    private ReactiveProperty<bool> _inGame = new ReactiveProperty<bool>();

    public void GameStateSwitcher(bool onValid)
    {
        _inGame.Value = onValid;
    }
}
