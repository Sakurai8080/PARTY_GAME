using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// 初期設定画面の推移ボタン
/// </summary>
public class TransitionButton : MonoBehaviour
{
    public IObservable<int> NextClickObserver => _nextClickSubject;

    [Header("変数")]
    [Tooltip("推移ボタン")]
    [SerializeField]
    private Button _transitionButton = default;

    private Subject<int> _nextClickSubject = new Subject<int>();

    private void Start()
    {
        int clickCount = 0;
        _transitionButton.OnClickAsObservable()
                         .TakeUntilDestroy(this)
                         .Subscribe(_ =>
                         {
                             clickCount++;
                             _nextClickSubject.OnNext(clickCount);
                         });
    }
}
