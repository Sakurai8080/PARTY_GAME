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

    Button _nextButton;
    RectTransform _textRect;
    Vector3 _initPosition;
    bool _inAnimation = false;

    private void Start()
    {
        _textRect = GetComponent<RectTransform>() ;
        _initPosition = _textRect.position;
        _nextButton = GetComponentInParent<Button>();
        if (_nextButton.interactable)
            TextAnimationStart();
    }

    public void TextAnimationStart()
    {
        if (!_inAnimation)
            StartCoroutine(UiLoopAnimationCoroutine());
    }

    private IEnumerator UiLoopAnimationCoroutine()
    {
        _inAnimation = true;
        while (true)
        {
            yield return new WaitForSeconds(_animStartDelayTime);
            transform.DOBlendableMoveBy(new Vector3(7f, 0, 0), 0.1f)
                                      .SetEase(Ease.InCirc);
            yield return new WaitForSeconds(_restartWaitTime);
            _textRect.position = _initPosition;
        }
    }
}
