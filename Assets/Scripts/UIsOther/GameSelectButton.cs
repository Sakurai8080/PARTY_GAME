using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// ゲームを選択するボタン
/// </summary>
public class GameSelectButton : MonoBehaviour
{
    [Header("変数")]
    [Tooltip("ゲーム移行するボタン")]
    [SerializeField]
    private Button _gameTransferBottun = default;

    [Tooltip("ゲームの種類")]
    [SerializeField]
    private GameType _gameType = default;

    private void Awake()
    {
        InteractiveSet();
    }

    void Start()
    {
        _gameTransferBottun.OnClickAsObservable()
                             .TakeUntilDestroy(this)
                             .Subscribe(_ =>
                             {
                                 GameTransition(_gameType);
                                 GameManager.Instance.GameSelected(_gameType);
                             });
    }

    /// <summary>
    /// ゲームへの移行処理
    /// </summary>
    /// <param name="selectedGame"></param>
    private void GameTransition(GameType selectedGame)
    {
        string sceneName = selectedGame.ToString();
        GameManager.Instance.SceneLoader(sceneName);
    }

    /// <summary>
    /// 選択済みのゲームなら選択不可に
    /// </summary>
    private void InteractiveSet()
    {
        bool isSelected = GameManager.Instance.SelectedChecker(_gameType);
        _gameTransferBottun.interactable = !isSelected;
    }
}
