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
    public Dictionary<Button, bool> TestDic => _allButtonDic;

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

    public bool MissButtonChecker(Button selectedButton)
    {
        return _allButtonDic[selectedButton];
    }

    public void Test(Button button)
    {
        Debug.Log(_allButtonDic[button]);
    }
}
