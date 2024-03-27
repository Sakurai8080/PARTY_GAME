using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;

public class InitSettingPresenter : MonoBehaviour
{
    [SerializeField]
    GameObject _nextUis = default;

    [SerializeField]
    GameObject _currentUis = default;

    [SerializeField]
    private GameObject _namedFailPopUp = default;

    [SerializeField]
    TextMeshProUGUI _joinAmountDisplayTMP = default;

    [SerializeField]
    TextMeshProUGUI _explosionTMP = default;

    [SerializeField]
    PeopleAmountButton[] _peopleButton = default;

    [SerializeField]
    TransitionButton _transitionButton = default;

    [SerializeField]
    NamedFailButton _namedFailButton = default;

    [SerializeField]
    NameInputField _nameInputField = default;

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
                                    StartNextTextAnimation();
                                }
                            });
        }

        _transitionButton.NextClickObserver
                         .TakeUntilDestroy(this)
                         .Subscribe(clickCount =>
                         {
                             if (clickCount == 1)
                             {
                                 _nameInputField.NameFieldNonAvailable(_joinAmount);
                                 _nextUis.SetActive(true);
                                 _currentUis.SetActive(false);
                                 KillNextTextAnimation();
                             }
                             else
                                 _nameInputField.NameAndCountChecker();
                         });

        _namedFailButton.OnClickObserver
                        .TakeUntilDestroy(this)
                        .Subscribe(_ => NamedFailSwitch(false));

        _nameInputField.NamedFailObserver
                       .TakeUntilDestroy(this)
                       .Subscribe(_ =>
                       {
                           NamedFailSwitch(true);
                           transitionButton.interactable = true;
                           StartNextTextAnimation();
                       });

        _nameInputField.AllEndEditObserver
                       .TakeUntilDestroy(this)
                       .Subscribe(_ => StartNextTextAnimation());
    }

    private void StartNextTextAnimation()
    {
        for (int i = 0; i < _nextTextAnimations.Length; i++)
        {
            _nextTextAnimations[i].MakeTweenSequence();
        }
    }

    private void KillNextTextAnimation()
    {
        for (int i = 0; i < _nextTextAnimations.Length; i++)
        {
            _nextTextAnimations[i].StopTween();
        }
    }

    private void NamedFailSwitch(bool valid)
    {
        _namedFailPopUp.SetActive(valid);
        _explosionTMP.gameObject.SetActive(!valid);
    }

    private void JoinAmountTMPControl(int choosedNum)
    {
        _joinAmount = choosedNum;
        _joinAmountDisplayTMP.text = $"{choosedNum}";
        _joinAmountDisplayTMP.transform.DOScale(0, 0);
        _joinAmountDisplayTMP.transform.DOScale(1, 0.2f)
                                       .SetEase(Ease.InOutBounce);
    }
}
