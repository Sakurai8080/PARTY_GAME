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

    private GameSelectUIAnimation _gameSelectUIAnimation;

    void Start()
    {
        _gameSelectUIAnimation = GetComponent<GameSelectUIAnimation>();

        _gameTransferBottun.OnClickAsObservable()
                             .TakeUntilDestroy(this)
                             .Subscribe(_ =>
                             {
                                 GameTransition(_gameType);
                                 _gameSelectUIAnimation.CurrentTweendKill();
                                 GameManager.Instance.GameSelected(_gameType);
                             });
    }

    private void GameTransition(GameType selectedGame)
    {
        string sceneName = selectedGame.ToString();
        GameManager.Instance.SceneLoader(sceneName);
    }
}
