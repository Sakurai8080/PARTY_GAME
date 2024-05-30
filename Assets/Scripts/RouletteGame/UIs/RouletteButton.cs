using System;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

/// <summary>
/// 回転させるボタンの機能
/// </summary>
public class RouletteButton : MonoBehaviour
{
    public IObservable<int> RouletteButtonClickObserver => _rouletteButtonClickSubject;

    [Header("変数")]
    [Tooltip("回転させるボタン")]
    [SerializeField]
    private Button _rotateStartButton;

    private int _clickCount = 0;

    private Subject<int> _rouletteButtonClickSubject = new Subject<int>();

    void Start()
    {
        _rotateStartButton.OnClickAsObservable()
                          .TakeUntilDestroy(this)
                          .Subscribe(_ =>
                          {
                              _clickCount++;
                              _rouletteButtonClickSubject.OnNext(_clickCount);
                          });
    }
}
