using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 画面のフェードを管理するクラス
/// </summary>
public class FadeManager : SingletonMonoBehaviour<FadeManager>
{

    [Header("変数")]
    [Tooltip("フェードにかける時間")]
    [SerializeField]
    private float _fadeTime = 1.0f;

    [Tooltip("フェード用マテリアル")]
    [SerializeField]
    private Material _fadeMaterial = null;

    private readonly int _progressId = Shader.PropertyToID("_progress");
    private bool _isFading = false;
    private float _fadeWaitTime = 1.0f;

    public void Fade(FadeType type, Action callback = null)
    {
        if (_isFading)
            return;

        StartCoroutine(ShaderFade(type, callback));
        
    }



    private IEnumerator ShaderFade(FadeType type, Action callBack = null)
    {
        (float currentValue, float endValue) = type == FadeType.In ? (0f, 1f) : (1f, 0);

        yield return DOTween.To(() => currentValue,
                                 x => currentValue = x,
                                 endValue,
                                 _fadeTime)
                                 .SetEase(Ease.Linear)
                                 .OnUpdate(() =>
                                 {
                                     _fadeMaterial.SetFloat(_progressId, currentValue);
                                 })
                                 .WaitForCompletion();
        _fadeMaterial.SetFloat(_progressId, (type == FadeType.In) ? 1f : 0f);
        _isFading = false;

        callBack?.Invoke();
    }
}

public enum FadeType
{
    In,
    Out
}
