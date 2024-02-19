using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine.UI;

namespace TweenGroup
{
    /// <summary>
    /// UI同士を仲介するプレゼンター
    /// </summary>
    public class UIPresenter : MonoBehaviour
    {
        public IObservable<Unit> UIGroupObserver => _presenterSubject;

        Subject<Unit> _presenterSubject = new Subject<Unit>();

        [Header("Variable")]
        [Tooltip("UIを操作するクラスの参照")]
        [SerializeField]
        TweenUIsController _cardUIActivator;

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
                              BombSet();
                          }).AddTo(this);
        }

        void BombSet()
        {
            _bombCardImageList.ForEach(card =>
            {
                BombManager._allBombUIdic.Add(card, false);
            });

            Debug.Log(BombManager._allBombUIdic.Count());
            BombManager.BombRandomInstallation();
        }
    }
}