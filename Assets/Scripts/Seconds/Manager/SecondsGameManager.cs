using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;
using TMPro;
using System.Threading;

public class SecondsGameManager : SingletonMonoBehaviour<SecondsGameManager>
{
    Dictionary<TimeSpan, string> _timeNameDic = new Dictionary<TimeSpan, string>();

    TimeSpan _correctTime = TimeSpan.FromSeconds(10);

    public void TimeNameAdd(TimeSpan time, string name , Action callBack = null)
    {
        _timeNameDic.Add(time, name);
        Debug.Log($"{time},{name}");
        if (_timeNameDic.Count() >= NameLifeManager.Instance.GamePlayerAmount)
        {
            ResultCheck();
            GameManager.Instance.SceneLoader("GameSelect");
        }
        else
        {
            callBack?.Invoke();
        }
    }

    private void ResultCheck()
    {
        TimeSpan currentMaxGapTime = TimeSpan.MinValue;
        TimeSpan loseTime = TimeSpan.MinValue;
        foreach (var item in _timeNameDic.Keys)
        {
            TimeSpan gap = _correctTime - item;

            gap = (gap < TimeSpan.Zero) ? TimeSpan.FromTicks(-gap.Ticks):gap;

            Debug.Log($"<color=red>{gap}</color>");
            if (currentMaxGapTime < gap)
            {
                currentMaxGapTime = gap;
                loseTime = item;
            }
        }
        string loseName = _timeNameDic[loseTime];

        NameLifeManager.Instance.ReduceLife(loseName);
    }
}
