using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;

/// <summary>
/// ボタンゲームの全体を管理するマネージャー
/// </summary>
public class TBGameManager : SingletonMonoBehaviour<TBGameManager>
{
    public IObservable<int> TurnChangeObserver => _turnChangeSubject;

    [Header("変数")]
    [Tooltip("操作するボタンのリスト")]
    [SerializeField]
    private List<Button> _allButtonList = new List<Button>();

    private Dictionary<Button, bool> _allButtonDic = new Dictionary<Button, bool>();

    private Subject<int> _turnChangeSubject = new Subject<int>();

    private void Awake()
    {
        _allButtonList.ForEach(button => _allButtonDic.Add(button, false));
    }

    /// <summary>
    /// ランダムの値を返す処理
    /// </summary>
    /// <param name="maxAmount">最大のアクティブ数</param>
    /// <returns>アクティブにする数</returns>
    private int RandomAmountPass(int maxAmount)
    {
        int chosenNum = UnityEngine.Random.Range(1, maxAmount+1);
        return chosenNum;
    }

    /// <summary>
    /// 失敗となるボタンのセット
    /// </summary>
    /// <param name="button">失敗となるボタン</param>
    private void MissButtonSetter(Button button)
    {
        Debug.Log($"ハズレボタンは<color=blue>{button.ToString().Substring(6,1)}</color>");
        _allButtonDic[button] = true;
    }

    /// <summary>
    /// 全てのボタンを一度元に戻す処理
    /// </summary>
    private void buttonReconfigure()
    {
        _allButtonList.ForEach(button => button.gameObject.SetActive(true));
        _allButtonDic.Keys.ToList().ForEach(keys => _allButtonDic[keys] = false);
    }

    /// <summary>
    /// ボタンをランダムの値で非アクティブにする処理
    /// </summary>
    public void ButtonRandomHide()
    {
        _allButtonList.ForEach(button => button.gameObject.SetActive(false));
        int maxActiveAmount = _allButtonList.Count();
        int activeButtonAmount = RandomAmountPass(maxActiveAmount);
        int sqeezeButtonAmount = (activeButtonAmount <= 2) ? RandomAmountPass(maxActiveAmount) : activeButtonAmount;
        for (int i = 0; i < sqeezeButtonAmount; i++)
        {
            _allButtonList[i].gameObject.SetActive(true);
        }
        _turnChangeSubject.OnNext(sqeezeButtonAmount);
        int missButtonIndex = UnityEngine.Random.Range(0, sqeezeButtonAmount);
        if (sqeezeButtonAmount != 1)
        {
            MissButtonSetter(_allButtonList[missButtonIndex]);
        }
    }

    /// <summary>
    /// 選択したボタンが失敗ボタンではないか確認する処理
    /// </summary>
    /// <param name="selectedButton">選択したボタン</param>
    public void MissButtonChecker(Button selectedButton)
    {
        bool isMiss = _allButtonDic[selectedButton];
        if (isMiss)
        {
            string loseName = NameLifeManager.Instance.CurrentNameReciever();
            NameLifeManager.Instance.ReduceLife(loseName);
            NameLifeManager.Instance.NameListOrderChange();
            GameManager.Instance.SceneLoader("GameSelect");
            return;
        }
        else
        {
            NameLifeManager.Instance.NameListOrderChange();
            //Todo:PopUp機能で名前表示

            buttonReconfigure();
            ButtonRandomHide();
        }
    }
}