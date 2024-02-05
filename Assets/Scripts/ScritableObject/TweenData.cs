using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Linq;

[Serializable]
[CreateAssetMenu(fileName = "Tweens",menuName = "Scritable Data/Create TweenData")]
public class TweenData : ScriptableObject
{
    public float FareDuration => _fadeDuration;
    public float ScaleDuration => _scaleDuration;
    public float AnimationDelayTime => _animationDelayTime;
    public Ease FadeEasing => _fadeEasing;
    public Ease ScaleEasing => _scaleEasing;
    public Ease LoopEasing => _loopEasing;
    public LoopType LoopType => _loopType;

    [Header("Variables")]
    [Tooltip("フェードにかける時間")]
    [SerializeField]
    private float _fadeDuration = 1f;

    [Tooltip("スケーリングにかける時間")]
    [SerializeField]
    private float _scaleDuration = 1f;

    [Tooltip("アニメーションを遅らせる時間")]
    [SerializeField]
    private float _animationDelayTime = 0.5f;

    [Tooltip("フェード用のイージング")]
    [SerializeField]
    private Ease _fadeEasing = default;

    [Tooltip("スケール用のイージング")]
    [SerializeField]
    private Ease _scaleEasing = default;

    [Tooltip("ループ時のイージング")]
    [SerializeField]
    private Ease _loopEasing = default;

    [Tooltip("ループのタイプ")]
    [SerializeField]
    private LoopType _loopType = default;
}
