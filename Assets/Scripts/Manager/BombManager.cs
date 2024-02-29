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
    public bool OnExplosion => _onExplosion;
    public bool IsChecking => _isChecking;

    /// <summary>Cardを格納する</summary>
    public static Dictionary<Image, bool> _allBombdic = new Dictionary<Image, bool>();

    private bool _onExplosion = false;
    private bool _isChecking = false; 

    public bool BombInChecker(Image image)
    {
        if (_allBombdic[image])
            _onExplosion = true;

        return _allBombdic[image];
    }

    public void BombRandomInstallation()
    {
        int cardCount = _allBombdic.Count();
        int inBombIndex = UnityEngine.Random.Range(0, cardCount);
        Image inBombImage = _allBombdic.Keys.ElementAt(inBombIndex);
        _allBombdic[inBombImage] = true;

        Debug.Log(inBombImage);
    }

    public void BombSet(List<Image> images)
    {
        images.ForEach(card =>
        {
            _allBombdic.Add(card, false);
        });
        BombRandomInstallation();
    }
}
