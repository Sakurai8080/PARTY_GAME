using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class GameSelectButton : MonoBehaviour
{
    [SerializeField]
    private GameType _gameType = default;

    [SerializeField]
    private Button _gameTransferBottun = default;

    // Start is called before the first frame update
    void Start()
    {
        _gameTransferBottun.OnClickAsObservable()
                             .TakeUntilDestroy(this)
                             .Subscribe(_ =>
                             {
                                 GameTransition(_gameType);
                             });
    }

    private void GameTransition(GameType selectedGame)
    {
        string sceneName = selectedGame.ToString();
        GameManager.SceneLoader(sceneName);
    }
}

public enum GameType
{
    BombGame,
    RouletteGame,
    Test
}