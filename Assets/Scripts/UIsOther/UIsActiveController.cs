using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Cysharp.Threading.Tasks;

/// <summary>
/// UIの操作をするクラス
/// </summary>
public class UIsActiveController : MonoBehaviour
{
    [Header("Variable")]
    [Tooltip("TestUIをまとめたグループ")]
    [SerializeField]
    private CanvasGroup _testUIGroup;

    [Tooltip("TestUIをまとめた親空オブジェクト")]
    [SerializeField]
    private GameObject _testUIParent;

    private bool _isUIsActive = false;

    public void ToggleUIsVisibility()
    {
        _isUIsActive = !_isUIsActive;
        _testUIGroup.alpha = Convert.ToInt32(_isUIsActive);
        _testUIParent.gameObject.SetActive(_isUIsActive);
    }
}
