using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private void Start()
    {
        float ratePerRoulette = 1 / (float)_rouletteColors.Count;
        float rotatePerRoulette = 360 / (float)(_peopleAmount.Count);

        for (int i = 0; i < _peopleAmount.Count; i++)
        {
            var obj = Instantiate(_rouletteImage, _imageParentTransform);
            obj.color = _rouletteColors[(_peopleAmount.Count - 1 - i)];
            obj.fillAmount = ratePerRoulette + (_peopleAmount.Count - i);
            obj.GetComponent<Text>().text = _peopleAmount[(_peopleAmount.Count -1 -i)];
            obj.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, ((rotatePerRoulette / 2) + rotatePerRoulette * i));
        }
    }
}
