using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲーム全体を管理するマネージャー
/// </summary>
public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public Dictionary<GameType, bool> GameTypeDic => _gameTypeDic;

    [Header("変数")]
    [Tooltip("ミニゲームの種類")]
    [SerializeField]
    private List<GameType> _gameTypeList = new List<GameType>();

    private Dictionary<GameType, bool> _gameTypeDic = new Dictionary<GameType, bool>();

    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < _gameTypeList.Count(); i++)
            _gameTypeDic.Add((GameType)i, false);
    }

    public void GameReset()
    {
        int gameTypeCount = System.Enum.GetValues(typeof(GameType)).Length;

        for (int i = 0; i < gameTypeCount; i++)
        {
            _gameTypeDic[(GameType)i] = false;
        }
    }

    /// <summary>
    /// シーンのロード
    /// </summary>
    /// <param name="sceneName">ロードするシーン</param>
    public void SceneLoader(string sceneName, BGMType bgm = BGMType.None)
    {
        SceneManager.LoadScene(sceneName);
        if (bgm != BGMType.None)
        {
            AudioManager.Instance.PlayBGM(bgm);
        }
        else if (sceneName == "Result")
        {
            AudioManager.Instance.PlayBGM(BGMType.Result);
        }   
    }

    /// <summary>
    /// 選択済のゲーム
    /// </summary>
    /// <param name="gameType"></param>
    public void GameSelected(GameType gameType)
    {
        _gameTypeDic[gameType] = true;
    }

    /// <summary>
    /// 既に選択されたゲームのチェック
    /// </summary>
    /// <param name="gameType">ゲームの種類</param>
    /// <returns>選択済か</returns>
    public bool SelectedChecker(GameType gameType)
    {
        return _gameTypeDic[gameType];
    }
}

public enum GameType
{
    BombGame,
    Roulette,
    TheButton,
    Dice,
    Ball,
    Secound
}