using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class RouletteMaker : MonoBehaviour
{
    [SerializeField]
    private Transform _imageParentTransform;

    [SerializeField]
    private List<Color> _rouletteColors;

    [SerializeField]
    private Image _rouletteImage;

    [SerializeField]
    private Image _rouletteFrame;

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
            AngleAndNamelinker(obj.fillAmount, currentName);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = currentName;
            obj.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, ((rotatePerRoulette / 2) + rotatePerRoulette * i) - 90);
            obj.DOFade(1, 5);
        }
        var frame = Instantiate(_rouletteFrame, _imageParentTransform);
    }

    private void AngleAndNamelinker(float currentAngle, string name)
    {
        float angle = currentAngle * 360;
        RouletteController.Instance.AngleNameDicAdd(angle, name);
    }
}
