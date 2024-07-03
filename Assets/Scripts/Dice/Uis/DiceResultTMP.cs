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

/// <summary>
/// サイコロの最終結果を表示するTMP
/// </summary>
public class DiceResultTMP : MonoBehaviour
{
    public IObservable<Unit> ResultInActiveObserver => _resultInactivedSubject;

    [SerializeField]
    private TextMeshProUGUI _resultTMP = default;

    private Subject<Unit> _resultInactivedSubject = new Subject<Unit>();

    /// <summary>
    /// TMPのフェード機能
    /// </summary>
    /// <param name="diceResult">和の数</param>
    public void FadeTMP(int diceResult)
    {
        _resultTMP.text = diceResult.ToString();
        _resultTMP.DOFade(1, 0.5f)
                  .SetEase(Ease.InFlash)
                  .SetDelay(2)
                  .OnComplete(async () =>
                  {
                      await _resultTMP.DOFade(0, 0.5f)
                                      .SetDelay(2)
                                      .AsyncWaitForCompletion();

                      _resultInactivedSubject.OnNext(Unit.Default);
                  });
    }
}
