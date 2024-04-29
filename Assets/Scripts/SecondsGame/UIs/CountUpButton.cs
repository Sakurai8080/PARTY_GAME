using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// カウントの操作をするボタン
/// </summary>
public class CountUpButton : MonoBehaviour
{
    public IObservable<bool> InProgressObservable => _inProgressSubject;

    [Header("変数")]
    [Tooltip("カウント操作ボタン")]
    [SerializeField]
    private Button _countUpButton = default;

    private bool _inProgress = false;

    private Subject<bool> _inProgressSubject = new Subject<bool>();

    private void Start()
    {
        _countUpButton.OnClickAsObservable()
                      .TakeUntilDestroy(this)
                      .Subscribe(_ =>
                      {
                          _inProgress = !_inProgress;
                          _inProgressSubject.OnNext(_inProgress);
                      });
    }
}
