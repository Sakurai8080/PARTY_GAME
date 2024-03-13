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

public class TBIngamePopup : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _percentageText = default;

    public void PercentPopup(int percent)
    {
        _percentageText.text = $"継続率{percent} %";
    }
}
