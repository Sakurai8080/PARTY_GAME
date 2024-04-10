using System.Collections;
using System.Collections.Generic;
using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;
using System.Threading;
using System.Linq;

public class DiceResultTMP : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _resultTMP = default;

    public void FadeTMP(int diceResult)
    {
        _resultTMP.text = diceResult.ToString();
        _resultTMP.DOFade(1, 0.5f)
                  .SetEase(Ease.InFlash)
                  .SetDelay(2)
                  .OnComplete(() =>
                  {
                      _resultTMP.DOFade(0, 0.5f)
                                .SetDelay(2);
                  });
    }
}
