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

    private Dictionary<Button, bool> _allButtonDic = new Dictionary<Button, bool>();
    private TBIngamePopup _popup;

    private void Awake()
    {
        _allButtonList.ForEach(button => _allButtonDic.Add(button, false));
    }

    private void Start()
    {
        _popup = GetComponent<TBIngamePopup>();
    }

    public void ButtonRandomHide()
    {
        _allButtonList.ForEach(button => button.gameObject.SetActive(false));
        int maxActiveAmount = _allButtonList.Count();
        int activeButtonAmount = UnityEngine.Random.Range(2,maxActiveAmount+1);
        for (int i = 0; i < activeButtonAmount; i++)
        {
            _allButtonList[i].gameObject.SetActive(true);
        }
        PercentCheckAndPopup(activeButtonAmount);
        int missButtonIndex = UnityEngine.Random.Range(0,activeButtonAmount);
        MissButtonSetter(_allButtonList[missButtonIndex]);
    }

    private void MissButtonSetter(Button button)
    {
        Debug.Log(button);
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
        Debug.Log(activeAmount);
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