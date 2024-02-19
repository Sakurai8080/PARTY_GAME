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
/// Bombを管理するマネージャー
/// </summary>
public static class BombManager
{
    /// <summary>Bombを格納する辞書</summary>
    public static Dictionary<Image, bool> _allBombUIdic = new Dictionary<Image, bool>();

    public static void AllTweenStop(Color resetColor)
    {
        Killweens();
        ResetTweens(resetColor);
    }

    public static bool BombInChecher(Image image)
    {
        return _allBombUIdic[image];
    }

    public static void BombRandomInstallation()
    {
        int cardCount = _allBombUIdic.Count();
        int inBombIndex = UnityEngine.Random.Range(0, cardCount);
        Image inBombImage = _allBombUIdic.Keys.ElementAt(inBombIndex);
        _allBombUIdic[inBombImage] = true;

        Debug.Log(inBombImage);
    }

    public static void Killweens()
    {
        foreach (var image in _allBombUIdic)
        {
            image.Key.transform.DOKill();
            image.Key.DOKill();
        }
    }

    public static void ResetTweens(Color resetColor)
    {
        foreach (var image in _allBombUIdic)
        {
            image.Key.transform.DOScale(1, 0.25f)
                               .SetEase(Ease.InBack);
            image.Key.transform.DORotate(Vector3.zero, 0.25f)
                               .SetEase(Ease.InBack);
            image.Key.DOFade(1, 0.25f);
            image.Key.DOColor(resetColor, 0.25f);
        }
    }

    public static void TweenListClear()
    {
        _allBombUIdic.Clear();
    }
}
