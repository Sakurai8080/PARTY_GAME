using TMPro;
using UnityEngine;

/// <summary>
/// 名付け失敗時のポップアップ
/// </summary>
public class TitleNameFailPopUp : MonoBehaviour
{
    [Header("変数")]
    [Tooltip("名付け失敗時に出すポップアップ")]
    [SerializeField]
    private GameObject _namedFailPopUp = default;

    [Tooltip("見出しTMP")]
    [SerializeField]
    private TextMeshProUGUI _explonasionTMP = default;

    /// <summary>
    /// ポップアップの表示処理
    /// </summary>
    /// <param name="onPopUp">表示の切り替え</param>
    public void NamedFailSwitch(bool onPopUp)
    {
        _namedFailPopUp.SetActive(onPopUp);
        _explonasionTMP.gameObject.SetActive(!onPopUp);
    }
}
