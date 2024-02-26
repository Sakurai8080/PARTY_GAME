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
public class BombManager : SingletonMonoBehaviour<BombManager>
{
    /// <summary>Cardを格納する</summary>
    public static Dictionary<Image, bool> _allBombdic = new Dictionary<Image, bool>();

    public static bool _onExplosion = false;

    public static bool BombInChecker(Image image)
    {
        if (_allBombdic[image])
            _onExplosion = true;

        return _allBombdic[image];
    }

    public static void BombRandomInstallation()
    {
        int cardCount = _allBombdic.Count();
        int inBombIndex = UnityEngine.Random.Range(0, cardCount);
        Image inBombImage = _allBombdic.Keys.ElementAt(inBombIndex);
        _allBombdic[inBombImage] = true;

        Debug.Log(inBombImage);
    }

    public static void BombSet(List<Image> images)
    {
        images.ForEach(card =>
        {
            BombManager._allBombdic.Add(card, false);
        });
        BombManager.BombRandomInstallation();
    }
}
