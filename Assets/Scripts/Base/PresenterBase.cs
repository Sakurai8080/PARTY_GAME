using System;
using UniRx;
using UnityEngine;

/// <summary>
/// プレゼンターの共通機能
/// </summary>
public abstract class PresenterBase : MonoBehaviour
{
    public IObservable<Unit> MainUIActiveObserver => _mainUIActiveSubject;
    private bool _isUIsActive = false;
    private Subject<Unit> _mainUIActiveSubject = new Subject<Unit>();

    [Header("変数")]
    [Tooltip("隠すキャンバスグループ")]
    [SerializeField]
    protected CanvasGroup _hideUIGroup;

    [Tooltip("アクティブの切り替えボタン")]
    [SerializeField]
    protected ToInGameButton _activeSwitchButton;

    [Tooltip("インゲームUIのグループ")]
    [SerializeField]
    private CanvasGroup _inGameUIGroup;

    [Tooltip("インゲームUIの親オブジェクト")]
    [SerializeField]
    private GameObject _InGameUIsParent;

    protected virtual void Start()
    {
        _activeSwitchButton.OnClickObserver
           　　　           .Subscribe(_ =>
                　　　      {
                               ToggleUIsVisibility();
                              _hideUIGroup.gameObject.SetActive(false);
                            }).AddTo(this);
    }

    protected virtual void ToggleUIsVisibility()
    {
        _isUIsActive = !_isUIsActive;
        _InGameUIsParent.gameObject.SetActive(_isUIsActive);
        _inGameUIGroup.alpha = Convert.ToInt32(_isUIsActive);
    }
}
