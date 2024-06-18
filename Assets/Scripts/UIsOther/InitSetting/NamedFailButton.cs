using UnityEngine;
using UniRx;
using UnityEngine.UI;
using System;

/// <summary>
/// 名付け失敗時のボタンコンポーネント
/// </summary>
public class NamedFailButton : MonoBehaviour
{
    public IObservable<Unit> OnClickObserver => _onClickSubject;

    [Header("変数")]
    [Tooltip("画面推移ボタン")]
    [SerializeField]
    Button _namedFailButton = default;

    private Subject<Unit> _onClickSubject = new Subject<Unit>();

    void Start()
    {
        _namedFailButton.OnClickAsObservable()
                        .TakeUntilDestroy(this)
                        .Subscribe(_ => _onClickSubject.OnNext(default));
    }
}
