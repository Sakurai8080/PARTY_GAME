using UnityEngine;
using TMPro;

/// <summary>
/// TMPの表示限度機能
/// </summary>
public class TMProLimit : MonoBehaviour
{
    [Header("変数")]
    [Tooltip("最大のTMP表示数")]
    [SerializeField]
    private int _textMaxAmount = 3;

    private TextMeshProUGUI _activeText;

    private void Start()
    {
        _activeText = GetComponent<TextMeshProUGUI>();
        TextLimitter();
    }

    /// <summary>
    /// テキストの限度だけ表示する機能
    /// </summary>
    private void TextLimitter()
    {
        string currentName = _activeText.text.ToString();
        _activeText.text = (currentName.Length > _textMaxAmount) ? currentName.Substring(0, _textMaxAmount) : currentName;
    }
}
