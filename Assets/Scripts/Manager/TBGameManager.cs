#define DebugTBTest

using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;

public class TBGameManager : SingletonMonoBehaviour<TBGameManager>
{
    [SerializeField]
    private List<Button> _allButtonList = new List<Button>();

    [SerializeField]
    private TBGameBackGround _backGround = default;

    //TODo:SEMANAGERが出来たらけす。SETest用
    [SerializeField]
    private AudioSource _testSE;

    private Dictionary<Button, bool> _allButtonDic = new Dictionary<Button, bool>();
    private TBIngamePopup _popup;

    private void Awake()
    {
        _allButtonList.ForEach(button => _allButtonDic.Add(button, false));
    }

    private void Start()
    {
        _popup = GetComponent<TBIngamePopup>();
        _backGround = _backGround.GetComponent<TBGameBackGround>();
    }

    public void ButtonRandomHide()
    {
        _allButtonList.ForEach(button => button.gameObject.SetActive(false));
        int maxActiveAmount = _allButtonList.Count();
        int activeButtonAmount = RandomAmountPass(maxActiveAmount);
        int sqeezeButtonAmount = (activeButtonAmount == 1) ? RandomAmountPass(maxActiveAmount) : activeButtonAmount;
        for (int i = 0; i < sqeezeButtonAmount; i++)
        {
            _allButtonList[i].gameObject.SetActive(true);
        }
        PercentCheckAndPopup(sqeezeButtonAmount);
        int missButtonIndex = UnityEngine.Random.Range(0, sqeezeButtonAmount);
        if (sqeezeButtonAmount!=1)
        {
            MissButtonSetter(_allButtonList[missButtonIndex]);
        }
    }

    public int RandomAmountPass(int maxAmount)
    {
        return UnityEngine.Random.Range(1, maxAmount);
    }

    private void MissButtonSetter(Button button)
    {
        Debug.Log($"ハズレボタンは{button}");
        _allButtonDic[button] = true;
    }

    public void buttonReconfigure()
    {
        _allButtonList.ForEach(button => button.gameObject.SetActive(true));
        _allButtonDic.Keys.ToList().ForEach(key => _allButtonDic[key] = false);
        ButtonRandomHide();
    }

    public void PercentCheckAndPopup(int activeAmount)
    {
        _backGround.BackGroundChange(activeAmount);
        if (activeAmount == 1)
        {
#if DebugTBTest
            _testSE.Play();
#endif
            _popup.PercentPopup(100);
            return;
        }
        int missPercent = 100 / activeAmount;
        int persistenceRate = 100 - missPercent;
        _popup.PercentPopup(persistenceRate);
    }

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
        }
    }
}