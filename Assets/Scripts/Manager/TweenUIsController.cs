using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TweenUIsController : SingletonMonoBehaviour<TweenUIsController>
{

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    /// <summary>
    /// 特定のTween削除
    /// </summary>
    /// <param name="tween">削除するTween</param>
    public void KillTweens(Tween tween)
    {
        tween?.Kill();
        tween?.Kill();
        tween = null;
        tween = null;
    }
}
