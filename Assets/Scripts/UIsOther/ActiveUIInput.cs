using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading;
using UnityEngine.UI;
using System;

namespace TweenGroup
{
    /// <summary>
    /// UIの押下を検知するクラス
    /// </summary>
    public sealed class ActiveUIInput : MonoBehaviour
    {
        public IObservable<Unit> OnClickObserver => _onClickSubject;

        Subject<Unit> _onClickSubject = new Subject<Unit>();

        [Tooltip("α値を切り替えるボタン")]
        [SerializeField]
        private Button _activeSwitchButton;

        void Start()
        {
            _activeSwitchButton.OnClickAsObservable()
                               .TakeUntilDestroy(this)
                               .Subscribe(_ =>
                               {
                                   _onClickSubject.OnNext(default);
                               });
        }
    }
}