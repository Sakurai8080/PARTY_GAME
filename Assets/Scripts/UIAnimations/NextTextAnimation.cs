using System;
using DG.Tweening;
using UnityEngine;
using System.Linq;

/// <summary>
/// 設定画面のNextに使うアニメーション
/// </summary>
public class NextTextAnimation : MonoBehaviour, IDisposable
{
    [Header("変数")]
    [Tooltip("アニメーションの開始時間")]
    [SerializeField]
    private float _animStartDelayTime = 0;

    [Tooltip("アニメーションを待ち合わせる時間")]
    [SerializeField]
    private float _restartWaitTime = 0;

    private Sequence _sequence;
    private RectTransform _textRect;
    private Vector3 _initPosition;

    private void Start()
    {
        _textRect = GetComponent<RectTransform>() ;
        _initPosition = _textRect.position;
    }

    private void OnDisable()
    {
        Dispose();
    }

    /// <summary>
    /// アニメ用シーケンスを作成
    /// </summary>
    public void MakeTweenSequence()
    {
        _sequence = DOTween.Sequence();
        _sequence.AppendInterval(_animStartDelayTime);
        _sequence.Append(transform.DOBlendableMoveBy(new Vector3(7f, 0, 0), 0.1f).SetEase(Ease.InCirc));
        _sequence.AppendInterval(_restartWaitTime);
        _sequence.SetLoops(-1, LoopType.Restart);
        _sequence.Play()
                 .SetLink(gameObject);
    }

    /// <summary>
    /// シーケンスの削除
    /// </summary>
    public void StopTween()
    {
        _textRect.position = _initPosition;
        Dispose();
    }

    public void Dispose()
    {
        _sequence?.Kill();
        _sequence = null;
    }
}
