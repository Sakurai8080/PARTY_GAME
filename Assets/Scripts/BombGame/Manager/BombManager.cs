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
    public static bool OnExplosion => _onExplosion;

    /// <summary>Cardを格納する</summary>
    private static Dictionary<Image, bool> _allBombdic = new Dictionary<Image, bool>();

    private static bool _onExplosion = false;

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
            _allBombdic.Add(card, false);
        });
        BombRandomInstallation();
    }

    public static void AfterExplosion()
    {
        string loseName = NameLifeManager.Instance.CurrentNameReciever();
        NameLifeManager.Instance.ReduceLife(loseName);
        Debug.Log(loseName);
        NameLifeManager.Instance.NameListOrderChange();
        GameManager.Instance.SceneLoader("GameSelect");
    }
}
