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
using TMPro;

/// <summary>
/// ボールゲームの管理
/// </summary>
public class BallGameManager : SingletonMonoBehaviour<BallGameManager>
{
    public IObservable<bool> InGameObserver => _inGame;
    public IReadOnlyReactiveProperty<bool> InGameReady => _inGameReady;

    private ReactiveProperty<bool> _inGame = new ReactiveProperty<bool>();
    private ReactiveProperty<bool> _inGameReady = new ReactiveProperty<bool>();
    private int _chooseBallCount = 0;

    protected override void Awake()
    {
    }

    /// <summary>
    /// ボタンを全て選択されたら通知
    /// </summary>
    public void ChooseBall()
    {
        _chooseBallCount++;
        if (_chooseBallCount == NameLifeManager.Instance.GamePlayerAmount)
            _inGameReady.Value = true;
    }

    /// <summary>
    /// ボールゲームのインゲーム確認
    /// </summary>
    /// <param name="onValid">ゲーム中か</param>
    public void GameStateSwitcher(bool onValid)
    {
        _inGame.Value = onValid;
    }
}
