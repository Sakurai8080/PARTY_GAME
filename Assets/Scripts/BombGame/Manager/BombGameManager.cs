using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Cysharp.Threading.Tasks;

/// <summary>
/// Bombを管理するマネージャー
/// </summary>
public class BombGameManager : SingletonMonoBehaviour<BombGameManager>
{

    public bool OnExplosion => _onExplosion;

    private Dictionary<Image, BoxContents> _allBombdic = new Dictionary<Image, BoxContents>();
    private bool _onExplosion = false;

    /// <summary>
    /// カード選択時のハズレチェック
    /// </summary>
    /// <param name="image">選択したカード</param>
    /// <returns>ボムの有無</returns>
    public BoxContents BombInChecker(Image image)
    {
        if (_allBombdic[image] == BoxContents.Bomb)
            _onExplosion = true;

        return _allBombdic[image];
    }

    /// <summary>
    /// ランダムの値でボムのセット
    /// </summary>
    public void BombRandomInstallation()
    {
        int inCount = 2;
        List<Image> availableCards = _allBombdic.Keys.ToList();

        for (int i = 0; i < inCount; i++)
        {
            int randomIndex = Random.Range(0, availableCards.Count());
            Image inBombImage = availableCards[randomIndex];
            _allBombdic[inBombImage] = (BoxContents)i+1;
            Debug.Log($"ボムは<color=yellow>{inBombImage}</color>番目のカード");
            availableCards.RemoveAt(randomIndex);
        }
    }

    /// <summary>
    /// ボムをセットする前に一度全てカードのセット
    /// </summary>
    /// <param name="images">カードのイメージ</param>
    public void CardSet(List<Image>　images)
    {
        images.ForEach(card => _allBombdic.Add(card, BoxContents.None));
        BombRandomInstallation();
    }

    public void AfterEmptySelected()
    {
        BombUIsAnimationController.Instance.RestartTweens();
        BombUIsAnimationController.Instance.InteractableValidTask(true, 1).Forget();
        NameLifeManager.Instance.NameListOrderChange();
    }

    /// <summary>
    /// ボムを選択したあとの処理
    /// </summary>
    public void AfterExplosion()
    {
        string loseName = NameLifeManager.Instance.CurrentNameReciever();
        NameLifeManager.Instance.ReduceLife(loseName);
        Debug.Log(loseName);
        NameLifeManager.Instance.NameListOrderChange();
        GameManager.Instance.SceneLoader("GameSelect");
    }

    public void AfterAppleSelect()
    {
        string lifeUpName = NameLifeManager.Instance.CurrentNameReciever();
        NameLifeManager.Instance.IncreaseLife(lifeUpName);
        Debug.Log(lifeUpName);
        NameLifeManager.Instance.NameListOrderChange();
        BombUIsAnimationController.Instance.AfterImageValid(0,0.25f,false);
        BombUIsAnimationController.Instance.RestartTweens();
        BombUIsAnimationController.Instance.InteractableValidTask(true, 1).Forget();
    }
}

public enum BoxContents
{
    None,
    Bomb,
    Apple
}
