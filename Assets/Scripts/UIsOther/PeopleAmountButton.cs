using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// 参加人数を選択するボタン
/// </summary>
public class PeopleAmountButton : MonoBehaviour
{
    public IObservable<int> PeopleButtonClickObserver => _peopleButtonClickSubject;

    [Header("変数")]
    [Tooltip("参加人数選択ボタン")]
    [SerializeField]
    private Button _peopleAmountButton = default;

    [Tooltip("参加人数")]
    [SerializeField]
    private int _peopleAmount = default;

    private Subject<int> _peopleButtonClickSubject = new Subject<int>();

    private void Start()
    {
        _peopleAmountButton.OnClickAsObservable()
                           .TakeUntilDestroy(this)
                           .Subscribe(_ => _peopleButtonClickSubject.OnNext(_peopleAmount));
    }
}
