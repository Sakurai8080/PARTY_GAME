using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoTweenAnim1 : TweenBase
{
    private Tween _currentTween;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.zero;
        _currentTween = transform.DOScale(1, 1f)
                                 .SetEase(Ease.InSine)
                                 .SetDelay(2)
                                 .OnComplete(() =>
                                 {
                                     UiLoopAnimation();
                                 });
    }

    protected override void UiLoopAnimation()
    {
        _currentTween = transform.DOScale(0.8f, 1)
                                 .SetLoops(-1,LoopType.Yoyo)
                                 .SetEase(Ease.Linear);

    }
}
