using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public static class TweenManager
{
    public static List<Image> _allTweenList = new List<Image>();

    public static void AllTweenStop(Color ResetColor)
    {
        foreach (var item in _allTweenList)
        {
            item.transform.DOKill();
            item.DOKill();

            item.transform.DOScale(1, 0.25f)
                          .SetEase(Ease.InBack);

            item.transform.DORotate(Vector3.zero, 0.25f)
                          .SetEase(Ease.InBack);

            item.DOFade(1, 0.25f);
            item.DOColor(ResetColor, 0.25f);
        }
        _allTweenList.Clear();
    }
}
