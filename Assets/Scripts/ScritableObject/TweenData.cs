using System;
using UnityEngine;
using DG.Tweening;

[Serializable]
[CreateAssetMenu(fileName = "TweenAnimation",menuName = "Scritable Data/Create TweenData")]
public class TweenData : ScriptableObject
{
    public float FadeDuration => _fadeDuration;
    public float ScaleDuration => _scaleDuration;
    public float AnimationDelayTime => _animationDelayTime;
    public float ScaleAmount => _scaleAmount;
    public Ease FadeEasing => _fadeEasing;
    public Ease ScaleEasing => _scaleEasing;
    public Ease LoopEasing => _loopEasing;
    public LoopType LoopType => _loopType;
    public Color LoopColor => _loopColor;

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

    [Tooltip("スケールに使う値")]
    [SerializeField]
    private float _scaleAmount = 0.9f;

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

    [Tooltip("カラー変更")]
    [SerializeField]
    private Color _loopColor = default;
}
