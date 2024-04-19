using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// シーン上のUIアニメーションに共通するコントロール機能
/// </summary>
public class TweenUIsController : SingletonMonoBehaviour<TweenUIsController>
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 特定のTween削除
    /// </summary>
    /// <param name="tween">削除するTween</param>
    public void KillTweens(Tween tween)
    {
        tween?.Kill();
        tween = null;
    }

    /// <summary>
    /// 複数のTween削除
    /// </summary>
    /// <param name="tween">削除するTweenの配列</param>
    public void KillTweens <T> (IEnumerable<T> tweens) where T : Tween
    {
        foreach (var tween in tweens)
            KillTweens(tween);
    }
}
