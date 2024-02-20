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
    /// <summary>Cardを格納する</summary>
    public static Dictionary<Image, bool> _allBombUIdic = new Dictionary<Image, bool>();

    public static bool BombInChecker(Image image)
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
}
