using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class TweenBase : MonoBehaviour
{
    [Header("Variable")]
    [Tooltip("遅らせる時間")]
    [SerializeField]
    protected float _delayTime = 2f;

    [Tooltip("所要時間")]
    [SerializeField]
    protected float _requiredTime = 1f;

    [Tooltip("ターゲット値")]
    [SerializeField]
    protected float _targetAmout = 1f;

    [SerializeField]
    protected Image _targetImage = default;

    protected abstract void UiLoopAnimation();
}
