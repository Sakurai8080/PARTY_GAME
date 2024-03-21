using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;
using TMPro;

public class PeopleAmountPresenter : MonoBehaviour
{
    [SerializeField]
    PeopleAmountButton[] _peopleButton = default;

    [SerializeField]
    TransitionButton _transitionButton = default;

    [SerializeField]
    NameInputField _nameInputField = default;

    [SerializeField]
    GameObject _nextUis = default;

    [SerializeField]
    GameObject _currentUis = default;

    [SerializeField]
    TextMeshProUGUI _joinAmountDisplayTMP = default;

    [SerializeField]
    NextTextAnimation[] _nextTextAnimations = default;

    private int _joinAmount = 0;

    void Start()
    {
        Button transitionButton = _transitionButton.GetComponent<Button>();
        for (int i = 0; i < _peopleButton.Length; i++)
        {
            _peopleButton[i].PeopleButtonClickObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(chooseNum =>
                            {
                                _joinAmount = chooseNum;
                                JoinAmountTMPControl(chooseNum);
                                if (!transitionButton.interactable)
                                {
                                    transitionButton.interactable = true;
                                    for (int i = 0; i < _nextTextAnimations.Length; i++)
                                        _nextTextAnimations[i].TextAnimationStart();
                                }
                            });
        }

        _transitionButton.NextClickObserver
                         .TakeUntilDestroy(this)
                         .Subscribe(_ =>
                         {
                             _nameInputField.NameFieldNonAvailable(_joinAmount);
                             _nextUis.SetActive(true);
                             _currentUis.SetActive(false);
                         });

    }

    private void JoinAmountTMPControl(int choosedNum)
    {
        _joinAmount = choosedNum;
        _joinAmountDisplayTMP.text = $"{choosedNum}";
        _joinAmountDisplayTMP.transform.DOScale(0, 0);
        _joinAmountDisplayTMP.transform.DOScale(1, 0.1f)
                                       .SetEase(Ease.InOutBounce);
    }
}
