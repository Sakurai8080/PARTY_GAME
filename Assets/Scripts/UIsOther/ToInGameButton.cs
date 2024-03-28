using UnityEngine;
using UniRx;
using UnityEngine.UI;
using System;

/// <summary>
/// UIの押下を検知するクラス
/// </summary>
public class ToInGameButton : MonoBehaviour
{
    public IObservable<Unit> OnClickObserver => _onClickSubject;

    [Header("Variable")]
    [Tooltip("全UIをコントロールするUI")]
    [SerializeField]
    Button _toggleButton = default;

    Subject<Unit> _onClickSubject = new Subject<Unit>();

    void Start()
    {
        _toggleButton.OnClickAsObservable()
                           .TakeUntilDestroy(this)
                           .Subscribe(_ => _onClickSubject.OnNext(default));
    }
}
