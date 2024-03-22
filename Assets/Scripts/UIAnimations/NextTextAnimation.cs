using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;
using TMPro;

public class NextTextAnimation : MonoBehaviour
{
    [SerializeField]
    private float _animStartDelayTime = 0;

    [SerializeField]
    private float _restartWaitTime = 0;

    [SerializeField]
    private NameInputField _nameiInputField = default;

    RectTransform _textRect;
    Vector3 _initPosition;
    bool _inAnimation = false;
    Coroutine _currentCoroutine;

    private void Start()
    {
        _textRect = GetComponent<RectTransform>() ;
        _initPosition = _textRect.position;

        _nameiInputField.AllEndEditObserver
                        .TakeUntilDestroy(this)
                        .Subscribe(_ =>
                        {
                            if (!_inAnimation)
                                TextAnimationControl();
                        });
    }


    private void OnDisable()
    {
        if(_currentCoroutine != null)
        {
            _currentCoroutine = null;
        }
    }

    public void TextAnimationControl()
    {
        if (!_inAnimation)
        {
            _currentCoroutine = StartCoroutine(UiLoopAnimationCoroutine());
        }
        else if (_inAnimation)
        {
            _inAnimation = false;
            _currentCoroutine = null;
        }
    }

    private IEnumerator UiLoopAnimationCoroutine()
    {
        _textRect.position = _initPosition;
        _inAnimation = true;
        while (_inAnimation)
        {
            yield return new WaitForSeconds(_animStartDelayTime);
            transform.DOBlendableMoveBy(new Vector3(7f, 0, 0), 0.1f)
                                      .SetEase(Ease.InCirc);
            yield return new WaitForSeconds(_restartWaitTime);
            _textRect.position = _initPosition;
        }
    }
}
