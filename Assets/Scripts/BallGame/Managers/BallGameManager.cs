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

    private Action _loseFadeCompletedCallBack;

    protected override void Awake(){}

    private void Start()
    {
        BallController.Instance.AllBallInstancedObserver
                               .TakeUntilDestroy(this)
                               .Subscribe(_ => FadeManager.Instance.OrderChangeFadeAnimation().Forget());
    }

    /// <summary>
    /// ボタンが全て選択されたら通知
    /// </summary>
    public void ChooseBall()
    {
        _chooseBallCount++;
        if (_chooseBallCount == NameLifeManager.Instance.GamePlayerAmount)
        {
            _inGameReady.Value = true;
            return;
        }
        FadeManager.Instance.OrderChangeFadeAnimation().Forget();
    }

    /// <summary>
    /// ボールゲームのインゲーム確認
    /// </summary>
    /// <param name="onValid">ゲーム中か</param>
    public void GameStateSwitcher(bool onValid)
    {
        _inGame.Value = onValid;
    }

    /// <summary>
    /// ゲーム終了後のシーン選択と負けた人を表示する機能
    /// </summary>
    /// <param name="loseName">負けた名前</param>
    public void SceneLoadAfterFade(string loseName)
    {
        string sceneName = NameLifeManager.Instance.NameLifeDic.Values.Contains(0)? "Result" : "GameSelect"; 
        _loseFadeCompletedCallBack = ()=> GameManager.Instance.SceneLoader(sceneName);
        FadeManager.Instance.LoseNameFade(loseName, _loseFadeCompletedCallBack).Forget();
    }
}
