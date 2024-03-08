using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine.UI;

public class BallFallButton : MonoBehaviour
{
    public IObservable<Unit> FallButtonClickObserver => _fallButtonClickSubject;

    private Subject<Unit> _fallButtonClickSubject = new Subject<Unit>();

    [SerializeField]
    Button _ballFallButton = default;

    private void Start()
    {
        _ballFallButton.OnClickAsObservable()
                       .TakeUntilDestroy(this)
                       .Subscribe(_ =>
                       {
                           _fallButtonClickSubject.OnNext(Unit.Default);
                           gameObject.SetActive(false);
                           BallManager.Instance.GameStateSwitcher(true);
                       });
    }
}
