using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

public class GameSelectButton : MonoBehaviour
{

    [SerializeField]
    private Button _gameTransferBottun = default;

    [SerializeField]
    private GameType _gameType = default;

    private GameSelectUIAnimation _gameSelectUIAnimation;

    void Start()
    {
        _gameSelectUIAnimation = GetComponent<GameSelectUIAnimation>();
        InteractiveSet();

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

    private void InteractiveSet()
    {
        bool isSelected = GameManager.Instance.SelectedChecker(_gameType);
        _gameTransferBottun.interactable = !isSelected;
    }
}