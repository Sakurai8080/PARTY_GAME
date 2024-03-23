using System;
using UniRx;
using UnityEngine;

/// <summary>
/// プレゼンターの共通機能
/// </summary>
public abstract class PresenterBase : MonoBehaviour
{
    public IObservable<Unit> MainUIActiveObserver => _mainUIActiveSubject;

    private Subject<Unit> _mainUIActiveSubject = new Subject<Unit>();

   
    [Header("変数")]
    [Tooltip("次に表示するUIグループ")]
    [SerializeField]
    protected UIsActiveController _nextActiveUIs;

    [Tooltip("隠すUIグループ")]
    [SerializeField]
    protected ActiveUIInput _currentHideUIs;

    protected virtual void Start()
    {
        _currentHideUIs.OnClickObserver
                      .Subscribe(_ =>
                      {
                          _nextActiveUIs.ToggleUIsVisibility();
                          _currentHideUIs.gameObject.SetActive(false);
                      }).AddTo(this);
    }
}
