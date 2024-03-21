using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;
using TMPro;

public class NameSecondDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _currentSecondTMP = default;

    [SerializeField]
    private List<TextMeshProUGUI> _nameSecoundTMP = default;

    int _currentOrder = 0;

    public void ResultTMPActivator()
    {
        _nameSecoundTMP[_currentOrder].text = _currentSecondTMP.text;
        _nameSecoundTMP[_currentOrder].DOFade(1, 0.25f);
        _currentOrder++;
    }

    public void FinalResultTextChange(int order)
    {
        _nameSecoundTMP[order].DOColor(Color.red, 3)
                              .SetEase(Ease.OutQuad);
    }
}
