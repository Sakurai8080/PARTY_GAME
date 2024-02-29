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

    [SerializeField]
    List<Button> _bombButtonList = new List<Button>();

    void Start()
    {
        _activeUIInput.OnClickObserver
                      .Subscribe(_ =>
                      {
                          Setup();
                      }).AddTo(this);
    }

    private void Setup()
    {
        _cardUIActivator.ToggleUIsVisibility();
        _activeUIInput.gameObject.SetActive(false);
        BombManager.AddListButton(_bombButtonList);
        BombManager.BombSet(_bombCardImageList);
        BombManager.InteractableValidTask(false, 2).Forget();
        AllBombAnimationController.SetCards(_bombCardImageList);
    }
}
