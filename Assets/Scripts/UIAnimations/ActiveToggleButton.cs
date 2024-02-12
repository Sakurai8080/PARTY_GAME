using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using UniRx;
using System;
using System.Linq;
using UnityEngine.UI;
using TMPro;

namespace TweenGroup
{
    public class ActiveToggleButton : TweenBase
    {

        [SerializeField]
        Button _button = default;

        bool _isActeved = false;
        TextMeshProUGUI _switchingText = default;

        protected override void Start()
        {
            _switchingText = _button.GetComponentInChildren<TextMeshProUGUI>();
            _button.onClick.AddListener(TextChange);
        }

        private void OnEnable()
        {
            UiLoopAnimation();
        }

        private void TextChange()
        {
            _isActeved = !_isActeved;
            _switchingText.text = _isActeved ? "ON" : "OFF";
        }

        protected override void PlayAnimation()
        {
            throw new NotImplementedException();
        }

        protected override void UiLoopAnimation()
        {
            _currentFadeTween = _targetImage.transform.DOScale(0.8f, 1)
                                                      .SetEase(Ease.InFlash)
                                                      .SetLoops(-1, LoopType.Yoyo)
                                                      .SetUpdate(true);
        }
    }
}