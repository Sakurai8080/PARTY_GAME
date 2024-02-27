using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public Dictionary<GameType, bool> GameTypeDic => _gameTypeDic;

    [SerializeField]
    List<GameType> _gameTypeList = new List<GameType>();

    private Dictionary<GameType, bool> _gameTypeDic = new Dictionary<GameType, bool>();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        for (int i = 0; i < _gameTypeList.Count(); i++)
        {
            _gameTypeDic.Add((GameType)i, false);
        }
    }

    public void SceneLoader(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void GameSelected(GameType gameType)
    {
        _gameTypeDic[gameType] = true;
    }

    public bool SelectedChecker(GameType gameType)
    {
        return _gameTypeDic[gameType];
    }
}

public enum GameType
{
    BombGame,
    RouletteGame,
    TheButton,
}