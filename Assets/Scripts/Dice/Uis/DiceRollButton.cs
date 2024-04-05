using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

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
                   .Subscribe(_ =>
                   {
                       _rollButton.interactable = false;
                       _rollButton.gameObject.SetActive(false);
                       _rollSubject.OnNext(Unit.Default);
                   });
    }
}
