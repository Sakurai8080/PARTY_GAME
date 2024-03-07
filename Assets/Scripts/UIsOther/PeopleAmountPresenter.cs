using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;


public class PeopleAmountPresenter : MonoBehaviour
{
    [SerializeField]
    PeopleAmountButton[] _peopleButton = default;

    [SerializeField]
    NameInputField _nameInputField = default;

    [SerializeField]
    GameObject _nextUis = default;

    [SerializeField]
    GameObject _currentUis = default;

    void Start()
    {
        for (int i = 0; i < _peopleButton.Length; i++)
        {
            _peopleButton[i].PeopleButtonClickObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(peopleAmount =>
                            {
                                _nameInputField.NameFieldNonAvailable(peopleAmount);
                                _nextUis.SetActive(true);
                                _currentUis.SetActive(false);
                            });
        }
    }
}
