using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using UniRx;
using System;
using System.Linq;

public class ActiveToggleButton : TweenBase
{
    protected override void Start()
    {
    }

    private void OnEnable()
    {
        UiLoopAnimation();
    }

    private void OnDisable()
    {
        KillTweens();
    }

    protected override void PlayAnimation()
    {
        throw new NotImplementedException();
    }

    protected override void UiLoopAnimation()
    {
        _currentFadeTween = _targetImage.transform.DOScale(0.8f, 1)
                                                  .SetEase(Ease.InFlash)
                                                  .SetLoops(-1, LoopType.Yoyo)
                                                  .SetUpdate(true);
    }
}
