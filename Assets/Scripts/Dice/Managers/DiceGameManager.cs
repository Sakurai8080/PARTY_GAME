using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Linq;
using UniRx;

/// <summary>
/// サイコロゲームを管理するマネージャー
/// </summary>
public class DiceGameManager : SingletonMonoBehaviour<DiceGameManager>
{

    [Tooltip("サイコロの結果を表示するコンポーネント")]
    [SerializeField]
    private DiceResultTMP _diceResultTMP = default;

    private List<KeyValuePair<int, string>> _diceResultNameDic = new List<KeyValuePair<int, string>>();
    private List<string> _loseNameList = new();

    private Action _loseFadeCompletedCallBack;

    protected override void Awake(){}

    private void Start()
    {
        CinemaChineController.Instance.CameraReturnObserver
                                      .TakeUntilDestroy(this)
                                      .Subscribe(returnCount =>
                                      {
                                          if (returnCount < NameLifeManager.Instance.GamePlayerAmount)
                                          {
                                              FadeManager.Instance.OrderChangeFadeAnimation().Forget();
                                          }
                                      });

        _diceResultTMP.ResultInActiveObserver
                      .TakeUntilDestroy(this)
                      .Subscribe(_ => SceneLoadAfterFade());
    }

    /// <summary>
    /// サイコロの結果を名前と紐づける処理
    /// </summary>
    /// <param name="currentResult">サイコロの和の結果</param>
    /// <returns></returns>
    public void ResultReciever(int currentResult)
    {
        string currentName = NameLifeManager.Instance.CurrentNameReceiver();
        _diceResultNameDic.Add(new KeyValuePair<int, string>(currentResult, currentName));
        Debug.Log($"{currentName}:{currentResult}");
        NameLifeManager.Instance.NameListOrderChange();
    }

    /// <summary>
    /// ゲーム終了後のシーン選択と負けた人を表示する機能
    /// </summary>
    /// <param name="loseName">負けた名前</param>
    private void SceneLoadAfterFade()
    {
        if (_diceResultNameDic.Count() >= NameLifeManager.Instance.GamePlayerAmount)
        {
            LoseCheck();
            string sceneName = NameLifeManager.Instance.NameLifeDic.Values.Contains(0) ? "Result" : "GameSelect";
            _loseFadeCompletedCallBack = () => GameManager.Instance.SceneLoader(sceneName);
            FadeManager.Instance.LoseNameFade(_loseNameList, _loseFadeCompletedCallBack).Forget();
        }
    }

    /// <summary>
    /// 最終的に負けたプレイヤーの確認
    /// </summary>
    private void LoseCheck()
    {
        _diceResultNameDic.Sort((x,y)=> x.Key.CompareTo(y.Key));

        int minkey = _diceResultNameDic[0].Key;
        _loseNameList = _diceResultNameDic.Where(entry => entry.Key == minkey)
                                                   .Select(entry => entry.Value)
                                                   .ToList();
        _loseNameList.ForEach(name => Debug.Log(name));
        _loseNameList.ForEach(name => NameLifeManager.Instance.ReduceLife(name));
    }
}
