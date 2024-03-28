using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

/// <summary>
/// ルーレットの作成機能
/// </summary>
public class RouletteMaker : MonoBehaviour
{
    [Header("変数")]
    [Tooltip("ルーレットを格納する親")]
    [SerializeField]
    private Transform _imageParentTransform;

    [Tooltip("ルーレットのカラー")]
    [SerializeField]
    private List<Color> _rouletteColors;

    [Tooltip("ルーレットの基盤となる画像")]
    [SerializeField]
    private Image _rouletteImage;

    private void Start()
    {
        float ratePerRoulette = 1 / (float)NameLifeManager.Instance.GamePlayerAmount;
        float rotatePerRoulette = 360 / (float)NameLifeManager.Instance.GamePlayerAmount;
        int playerAmount = NameLifeManager.Instance.GamePlayerAmount;
        for (int i = 0; i < playerAmount; i++)
        {
            var obj = Instantiate(_rouletteImage, _imageParentTransform);
            obj.color = _rouletteColors[(playerAmount) - 1 - i];
            obj.fillAmount = ratePerRoulette * (playerAmount - i);
            string currentName = NameLifeManager.Instance.NameList[playerAmount - 1 - i];
            AngleAndNameLinker(obj.fillAmount, currentName);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = currentName;
            obj.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, ((rotatePerRoulette / 2) + rotatePerRoulette * i) - 90);
        }
            _imageParentTransform.transform.DOScale(1, 1.5f)
                                      .SetEase(Ease.OutBounce);
    }

    /// <summary>
    /// アングルに紐づける名前をコントローラーに渡す
    /// </summary>
    /// <param name="currentAngle">渡すアングル</param>
    /// <param name="name">紐づける名前</param>
    private void AngleAndNameLinker(float currentAngle, string name)
    {
        float angle = currentAngle * 360;
        RouletteController.Instance.AngleNameDicAdd(angle, name);
    }
}
