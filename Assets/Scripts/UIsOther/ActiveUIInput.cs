using UnityEngine;
using UniRx;
using UnityEngine.UI;
using System;
using TMPro;

namespace TweenGroup
{
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

        bool _isActeved = false;
        TextMeshProUGUI _switchingText = default;

        void Start()
        {
            _switchingText = _toggleButton.GetComponentInChildren<TextMeshProUGUI>();

            _toggleButton.OnClickAsObservable()
                               .TakeUntilDestroy(this)
                               .Subscribe(_ =>
                               {
                                   _onClickSubject.OnNext(default);
                                   TextChange();
                               });
        }

        private void TextChange()
        {
            _isActeved = !_isActeved;
            _switchingText.text = _isActeved ? "ON" : "OFF";
        }
    }
}