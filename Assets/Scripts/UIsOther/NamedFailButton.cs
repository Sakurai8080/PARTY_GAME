using UnityEngine;
using UniRx;
using UnityEngine.UI;
using System;
using TMPro;

public class NamedFailButton : MonoBehaviour
{
    public IObservable<Unit> OnClickObserver => _onClickSubject;

    Subject<Unit> _onClickSubject = new Subject<Unit>();

    [SerializeField]
    Button _namedFailButton = default;

    void Start()
    {
        _namedFailButton.OnClickAsObservable()
                        .TakeUntilDestroy(this)
                        .Subscribe(_ =>
                        {
                            _onClickSubject.OnNext(default);
                        });
    }
}
