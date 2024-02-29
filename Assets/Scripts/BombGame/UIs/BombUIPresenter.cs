using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine.UI;


/// <summary>
/// BombGameのUI同士を仲介するプレゼンター
/// </summary>
public class BombUIPresenter : MonoBehaviour
{
    public IObservable<Unit> UIGroupObserver => _presenterSubject;

    Subject<Unit> _presenterSubject = new Subject<Unit>();

    [Header("Variable")]
    [Tooltip("UIを操作するクラスの参照")]
    [SerializeField]
    UIsActiveController _cardUIActivator;

    [Tooltip("押下を検知するクラスの参照")]
    [SerializeField]
    ActiveUIInput _activeUIInput;

    [SerializeField]
    List<Image> _bombCardImageList = new List<Image>();

    void Start()
    {
        _activeUIInput.OnClickObserver
                      .Subscribe(_ =>
                      {
                          _cardUIActivator.ToggleUIsVisibility();
                          _activeUIInput.gameObject.SetActive(false);
                          BombManager.Instance.BombSet(_bombCardImageList);
                          BombAnimationController.CardSet(_bombCardImageList);
                      }).AddTo(this);
    }
}
