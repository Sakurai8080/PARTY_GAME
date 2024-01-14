using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading;
using UnityEngine.UI;
using System;

public class ActiveSwitcherUI : MonoBehaviour
{

    [Header("Variable")]
    [Tooltip("TestUIをまとめたグループ")]
    [SerializeField]
    private CanvasGroup _testUIGroup;

    [Tooltip("TestUIをまとめら親空オブジェクト")]
    [SerializeField]
    private GameObject _testUIParent;

    [Tooltip("α値を切り替えるボタン")]
    [SerializeField]
    private Button _activeSwitchButton;

    private bool _isUIsActive = false;

    void Start()
    {
        _activeSwitchButton.OnClickAsObservable()
                           .TakeUntilDestroy(this)
                           .Subscribe(_ =>
                           {
                               ToggleUIsVisibility();
                           });
    }

    void ToggleUIsVisibility()
    {
        _isUIsActive = !_isUIsActive;
        _testUIGroup.alpha = Convert.ToInt32(_isUIsActive);
        _testUIParent.gameObject.SetActive(_isUIsActive);
    }
}
