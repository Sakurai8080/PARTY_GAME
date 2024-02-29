using UnityEngine;
using UniRx;
using UnityEngine.UI;
using System;
using TMPro;

/// <summary>
/// UIの押下を検知するクラス
/// </summary>
public sealed class ActiveUIInput : MonoBehaviour
{
    public IObservable<Unit> OnClickObserver => _onClickSubject;

    Subject<Unit> _onClickSubject = new Subject<Unit>();

    [Header("Variable")]
    [Tooltip("全UIをコントロールするUI")]
    [SerializeField]
    Button _toggleButton = default;

    void Start()
    {
        _toggleButton.OnClickAsObservable()
                           .TakeUntilDestroy(this)
                           .Subscribe(_ =>
                           {
                               _onClickSubject.OnNext(default);
                           });
    }
}
