#define Bomb_Debug

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// Bombを管理するマネージャー
/// </summary>
public class BombGameManager : SingletonMonoBehaviour<BombGameManager>
{

    public bool OnExplosion => _onExplosion;

    private Dictionary<Image, bool> _allBombdic = new Dictionary<Image, bool>();
    private bool _onExplosion = false;


    /// <summary>
    /// カード選択時のハズレチェック
    /// </summary>
    /// <param name="image">選択したカード</param>
    /// <returns>ボムの有無</returns>
    public bool BombInChecker(Image image)
    {
        if (_allBombdic[image])
            _onExplosion = true;
        return _allBombdic[image];
    }

    /// <summary>
    /// ランダムの値でボムのセット
    /// </summary>
    public void BombRandomInstallation()
    {
        int cardCount = _allBombdic.Count();
        int inBombIndex = Random.Range(0, cardCount);
        Image inBombImage = _allBombdic.Keys.ElementAt(inBombIndex);
        _allBombdic[inBombImage] = true;
#if Bomb_Debug
        Debug.Log($"ボムは<color=yellow>{inBombImage}</color>番目のカード");
#endif 
    }

    /// <summary>
    /// ボムをセットする前に一度全てカードのセット
    /// </summary>
    /// <param name="images"></param>
    public void CardSet(List<Image> images)
    {
        images.ForEach(card => _allBombdic.Add(card, false));
        BombRandomInstallation();
    }

    /// <summary>
    /// ボムを選択したあとの処理
    /// </summary>
    public void AfterExplosion()
    {
        string loseName = NameLifeManager.Instance.CurrentNameReciever();
        NameLifeManager.Instance.ReduceLife(loseName);
#if Bomb_Debug
        Debug.Log(loseName);
#endif
        NameLifeManager.Instance.NameListOrderChange();
        GameManager.Instance.SceneLoader("GameSelect");
    }
}
