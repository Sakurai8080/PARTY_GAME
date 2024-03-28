using UnityEngine;
using UniRx;
using TMPro;

/// <summary>
/// ボタンゲームのポップアップ機能
/// </summary>
public class TBIngamePopup : MonoBehaviour
{
    [Header("変数")]
    [Tooltip("確率表示用TMP")]
    [SerializeField]
    private TextMeshProUGUI _percentageText = default;

    private void Start()
    {
        TBGameManager.Instance.TurnChangeObserver
                              .TakeUntilDestroy(this)
                              .Subscribe(activeAmount => PercentCheckAndPopup(activeAmount));
    }

    /// <summary>
    /// 継続率の計算
    /// </summary>
    /// <param name="activeAmount">ボタンの数</param>
    private void PercentCheckAndPopup(int activeAmount)
    {
        if (activeAmount == 1)
        {
            PercentPopup(100);
            return;
        }
        int missPercent = 100 / activeAmount;
        int persistenceRate = 100 - missPercent;
        PercentPopup(persistenceRate);
    }

    /// <summary>
    /// ポップアップ処理
    /// </summary>
    /// <param name="percent"></param>
    private void PercentPopup(int percent)
    {
        _percentageText.text = $"継続率{percent} %";
    }
}
