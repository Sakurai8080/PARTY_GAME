using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine.UI;

public class RouletteButton : MonoBehaviour
{
    public IObservable<int> RouletteButtonClickObserver => _rouletteButtonClickSubject; 

    private Subject<int> _rouletteButtonClickSubject = new Subject<int>();

    [SerializeField]
    private Button _rotateStartButton;

    private RouletteStartButtonAnim _rouletteStartButtonAnim;

    int _clickCount = 0;

    void Start()
    {
        _rouletteStartButtonAnim = GetComponent<RouletteStartButtonAnim>();

        _rotateStartButton.OnClickAsObservable()
                          .TakeUntilDestroy(this)
                          .Subscribe(_ =>
                          {
                              ButtonAnimationController();
                              _rouletteButtonClickSubject.OnNext(_clickCount);
                              _clickCount++;
                          });
    }

    private void ButtonAnimationController()
    {
        _rouletteStartButtonAnim.UILoopAnimation(_clickCount);
    }
}
