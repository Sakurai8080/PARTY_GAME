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
/// UI全体を管理するマネージャー
/// </summary>
public static class TweenManager
{
    /// <summary>全アニメーションを格納するリスト</summary>
    public static List<Image> _allTweenList = new List<Image>();

    public static void AllTweenStop(Color resetColor)
    {
        Killweens();
        ResetTweens(resetColor);
    }

    public static void Killweens()
    {
        _allTweenList.ForEach(tween =>
        {
            tween.transform.DOKill();
            tween.DOKill();
        });
    }

    public static void ResetTweens(Color resetColor)
    {
        _allTweenList.ForEach(tween =>
        {
            tween.transform.DOScale(1, 0.25f)
                          .SetEase(Ease.InBack);
            tween.transform.DORotate(Vector3.zero, 0.25f)
                          .SetEase(Ease.InBack);
            tween.DOFade(1, 0.25f);
            tween.DOColor(resetColor, 0.25f);
        });
    }

    private static void TweenListClear()
    {
        _allTweenList.Clear();
    }
}
