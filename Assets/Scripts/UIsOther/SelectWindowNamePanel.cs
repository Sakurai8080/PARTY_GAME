using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲームセレクト画面の名前とライフ操作
/// </summary>
public class SelectWindowNamePanel : MonoBehaviour
{
    [Header("変数")]
    [Tooltip("ライフ画像を格納している親オブジェクト")]
    [SerializeField]
    private LifeImage[] _lifeUIParents = default;

    [Tooltip("名前表示用のTMP")]
    [SerializeField]
    private List<TextMeshProUGUI> _nameTMP = new List<TextMeshProUGUI>();

    private void Awake()
    {
        for (int i = 0; i < NameLifeManager.Instance.GamePlayerAmount; i++)
        {
            string currentPlayerName = $"{NameLifeManager.Instance.NameList[i]}";
            string sceneName = SceneManager.GetActiveScene().name;
            if (sceneName == "GameSelect")
            { 
                _lifeUIParents[i].gameObject.SetActive(true);
            }
            _lifeUIParents[i].NameReciever(currentPlayerName);
            _nameTMP[i].text = (currentPlayerName.Length >= 4) ? currentPlayerName.Substring(0, 3) +':' : currentPlayerName +':';
            _nameTMP[i].gameObject.SetActive(true);
        }
    }
}
