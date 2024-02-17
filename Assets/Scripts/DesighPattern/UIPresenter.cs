using System;
using UniRx;
using UnityEngine;

namespace TweenGroup
{
    /// <summary>
    /// UI同士を仲介するプレゼンター
    /// </summary>
    public class UIPresenter : MonoBehaviour
    {
        public IObservable<Unit> UIGroupObserver => _presenterSubject;

        Subject<Unit> _presenterSubject = new Subject<Unit>();

        [Header("Variable")]
        [Tooltip("UIを操作するクラスの参照")]
        [SerializeField]
        TweenUIsController _testUIActivator;

        [Tooltip("押下を検知するクラスの参照")]
        [SerializeField]
        ActiveUIInput _activeUIInput;

        void Start()
        {
            _activeUIInput.OnClickObserver
                          .Subscribe(_ =>
                          {
                              _testUIActivator.ToggleUIsVisibility();
                              _activeUIInput.gameObject.SetActive(false);
                          }).AddTo(this);
        }
    }
}