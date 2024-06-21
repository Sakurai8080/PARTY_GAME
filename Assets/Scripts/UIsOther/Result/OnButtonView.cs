using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ボタンのクリックを検知するビュー
/// </summary>
public class OnButtonView : MonoBehaviour
{
    public IObservable<Unit> OnClickObserver => _onClickSubject;

    [Header("変数")]
    [Tooltip("クリックするボタン")]
    [SerializeField]
    private Button _transitionButton = default;

    private Subject<Unit> _onClickSubject = new Subject<Unit>();

    void Start()
    {
        _transitionButton.OnClickAsObservable()
                         .TakeUntilDestroy(this)
                         .Subscribe( _ => _onClickSubject.OnNext(Unit.Default));
    }
}
