using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;

/// <summary>
/// カードのTweenアニメーションのコントローラー
/// </summary>
public static class TweenController
{
    public static List<Image> _allTweensList = new List<Image>();

    public static void AllTweenStop(Color resetColor)
    {
        Killweens();
        ResetTweens(resetColor);
    }

    public static void Killweens()
    {
        foreach (var image in _allTweensList)
        {
            image.transform.DOKill();
            image.DOKill();
        }
    }

    public static void ResetTweens(Color resetColor)
    {
        foreach (var image in _allTweensList)
        {
            image.transform.DOScale(1, 0.25f)
                               .SetEase(Ease.InBack);
            image.transform.DORotate(Vector3.zero, 0.25f)
                               .SetEase(Ease.InBack);
            image.DOFade(1, 0.25f);
            image.DOColor(resetColor, 0.25f);
        }
    }

    public static void TweenListClear()
    {
         _allTweensList.Clear();
    }
}
