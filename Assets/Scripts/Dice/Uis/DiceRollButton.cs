using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;
using System.Threading;

/// <summary>
/// サイコロを振るボタン
/// </summary>
public class DiceRollButton : MonoBehaviour
{
    public IObservable<Unit> IsRollObserver => _rollSubject;

    [Header("変数")]
    [Tooltip("サイコロを振るボタン")]
    [SerializeField]
    private Button _rollButton = default;

    private Subject<Unit> _rollSubject = new Subject<Unit>();

    private void Start()
    {
        _rollButton.OnClickAsObservable()
                   .TakeUntilDestroy(this)
                   .Subscribe(_ =>_rollSubject.OnNext(Unit.Default));
    }
}
