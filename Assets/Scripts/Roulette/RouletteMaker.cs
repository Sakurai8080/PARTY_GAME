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
    private List<string> _peopleAmount;

    [SerializeField]
    private List<Color> _rouletteColors;

    [SerializeField]
    private Image _rouletteImage;

    [SerializeField]
    private Image _rouletteFrame;

    private void Start()
    {
        float ratePerRoulette = 1 / (float)_peopleAmount.Count;
        float rotatePerRoulette = 360 / (float)(_peopleAmount.Count);

        for (int i = 0; i < _peopleAmount.Count; i++)
        {
            var obj = Instantiate(_rouletteImage, _imageParentTransform);
            obj.color = _rouletteColors[(_peopleAmount.Count - 1 - i)];
            obj.fillAmount = ratePerRoulette * (_peopleAmount.Count - i);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = _peopleAmount[(_peopleAmount.Count -1 -i)];
            obj.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, ((rotatePerRoulette / 2) + rotatePerRoulette * i));
            obj.DOFade(1, 0);
        }
        var frame = Instantiate(_rouletteFrame, _imageParentTransform);
    }
}
