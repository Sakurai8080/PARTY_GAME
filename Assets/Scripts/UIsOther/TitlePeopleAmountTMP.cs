using DG.Tweening;
using TMPro;
using UnityEngine;

/// <summary>
/// 初期設定画面の参加人数を表示するTMPコンポーネント
/// </summary>
public class TitlePeopleAmountTMP : MonoBehaviour
{
    [Header("変数")]
    [Tooltip("人数を表示するTMP")]
    [SerializeField]
    private TextMeshProUGUI _joinAmountDisplayTMP = default;

    /// <summary>
    /// 参加人数を示すTMPの表示機能
    /// </summary>
    /// <param name="choosedNum"></param>
    public void JoinAmountTMPControl(int choosedNum)
    {
        _joinAmountDisplayTMP.text = $"{choosedNum}";
        _joinAmountDisplayTMP.transform.DOScale(0, 0);
        _joinAmountDisplayTMP.transform.DOScale(1, 0.2f)
                                       .SetEase(Ease.InOutBounce);
    }
}
