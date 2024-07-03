using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;
using Cysharp.Threading.Tasks;


/// <summary>
/// ボタンゲームの全体を管理するマネージャー
/// </summary>
public class TBGameManager : SingletonMonoBehaviour<TBGameManager>
{
    public IObservable<int> TurnChangeObserver => _turnChangeSubject;
    public int CurrentActiveAmount => _squeezeButtonAmount;

    private Action _loseFadeCompletedCallBack;

    [Header("変数")]
    [Tooltip("操作するボタンのリスト")]
    [SerializeField]
    private List<Button> _allButtonList = new List<Button>();

    private Dictionary<Button, bool> _allButtonDic = new Dictionary<Button, bool>();

    private Subject<int> _turnChangeSubject = new Subject<int>();

    private int _squeezeButtonAmount = 0;

    protected override void Awake()
    {
        _allButtonList.ForEach(button => _allButtonDic.Add(button, false));
    }

    private void Start()
    {
        FadeManager.Instance.NameAnimFadeCompletedObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(_=> ButtonRandomHide());
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
        Debug.Log($"ハズレボタンは<color=yellow>{button.ToString().Substring(6,1)}</color>");
        _allButtonDic[button] = true;
    }

    /// <summary>
    /// 全てのボタンを一度元に戻す処理
    /// </summary>
    private void buttonReconfigure()
    {
        _allButtonList.ForEach(button => button.gameObject.SetActive(false));
        _allButtonDic.Keys.ToList().ForEach(keys => _allButtonDic[keys] = false);
    }

    /// <summary>
    /// ボタンをランダムの値で非アクティブにする処理
    /// </summary>
    private void ButtonRandomHide()
    {
        _allButtonList.ForEach(button => button.gameObject.SetActive(false));
        int maxActiveAmount = _allButtonList.Count();
        int activeButtonAmount = RandomAmountPass(maxActiveAmount);
        _squeezeButtonAmount = (activeButtonAmount <= 1) ? RandomAmountPass(maxActiveAmount) : activeButtonAmount;
        for (int i = 0; i < _squeezeButtonAmount; i++)
        {
            _allButtonList[i].gameObject.SetActive(true);
        }
        _turnChangeSubject.OnNext(_squeezeButtonAmount);
        int missButtonIndex = UnityEngine.Random.Range(0, _squeezeButtonAmount);
        if (_squeezeButtonAmount != 1)
        {
            MissButtonSetter(_allButtonList[missButtonIndex]);
        }
    }

    private void NextOrderActivator()
    {
        FadeManager.Instance.OrderChangeFadeAnimation().Forget();
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
            string loseName = NameLifeManager.Instance.CurrentNameReceiver();
            NameLifeManager.Instance.ReduceLife(loseName);
            string sceneName = NameLifeManager.Instance.NameLifeDic.Values.Contains(0)? "Result" : "GameSelect"; 
            NameLifeManager.Instance.NameListOrderChange();
            _loseFadeCompletedCallBack = () => GameManager.Instance.SceneLoader(sceneName);
            FadeManager.Instance.LoseNameFade(loseName, _loseFadeCompletedCallBack).Forget();
            return;
        }
        else
        {
            NameLifeManager.Instance.NameListOrderChange();
            NextOrderActivator();
            buttonReconfigure();
        }
    }
}