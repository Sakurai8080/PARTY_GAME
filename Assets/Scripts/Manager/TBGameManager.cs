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

    private void Awake()
    {
        _allButtonList.ForEach(button => _allButtonDic.Add(button, false));
    }

    public void ButtonRandomHide()
    {
        int maxHideAmount = _allButtonList.Count() -2;
        int hideButtonAmount = UnityEngine.Random.Range(0,maxHideAmount);
        for (int i = 0; i < hideButtonAmount; i++)
        {
            _allButtonList[i].gameObject.SetActive(false);
        }
        int missButtonIndex = UnityEngine.Random.Range(hideButtonAmount,_allButtonList.Count()-1);
        Debug.Log($"false{missButtonIndex}");
        MissButtonSetter(_allButtonList[missButtonIndex]);
    }

    private void MissButtonSetter(Button button)
    {
        _allButtonDic[button] = true;
    }

    public void resetButtonValue()
    {
        _allButtonDic.Keys.ToList().ForEach(key => _allButtonDic[key] = false);
    }

    public void Test(Button button)
    {
        Debug.Log(_allButtonDic[button]);
    }
}
