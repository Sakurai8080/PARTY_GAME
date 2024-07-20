using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;

/// <summary>
/// 初期設定画面のプレゼンター
/// </summary>
public class InitSettingPresenter : MonoBehaviour
{
    [Header("変数")]
    [Tooltip("ボタン押下で表示するUI親オブジェクト")]
    [SerializeField]
    private GameObject _nextActiveUiParent = default;

    [Tooltip("ボタン押下で隠すUI親オブジェクト")]
    [SerializeField]
    private GameObject _nextHideUiParent = default;

    [Tooltip("参加人数選択ボタン")]
    [SerializeField]
    private PeopleAmountButton[] _peopleDesideButton = default;

    [Tooltip("次に移行するボタン")]
    [SerializeField]
    private TransitionButton _transitionButton = default;

    [Tooltip("名付け失敗時に出すボタン")]
    [SerializeField]
    private NamedFailButton _namedFailButton = default;

    [Tooltip("名前を入力するフィールド")]
    [SerializeField]
    private NameInputField _nameInputField = default;

    [Tooltip("参加人数を表示するTMP")]
    [SerializeField]
    private TitlePeopleAmountTMP _titlePeopleAmountTMP = default;

    [Tooltip("名付け失敗時のポップアップ")]
    [SerializeField]
    private TitleNameFailPopUp _nameSettingFailPopUp = default;

    [Tooltip("次に移行するボタンのアニメーション")]
    [SerializeField]
    private NextTMPAnimController _nextTMPAnimController = default;

    private int _joinAmount = 0;
    private bool _namedCorrect = false;

    void Start()
    {
        Button transitionButton = _transitionButton.GetComponent<Button>();
        _peopleDesideButton.ToList().ForEach(button => button.PeopleButtonClickObserver
                                                     .TakeUntilDestroy(this)
                                                     .Subscribe(chooseNum =>
                                                     {
                                                         _joinAmount = chooseNum;
                                                         _titlePeopleAmountTMP.JoinAmountTMPControl(chooseNum);
                                                         NextButtonState(transitionButton, true);
                                                         AudioManager.Instance.PlaySE(SEType.Choose);
                                                     }));

        _namedFailButton.OnClickObserver
                        .TakeUntilDestroy(this)
                        .Subscribe(_ => _nameSettingFailPopUp.NamedFailSwitch(false));

        _nameInputField.AllEndEditObserver
                       .TakeUntilDestroy(this)
                       .Subscribe(_ => NextButtonState(transitionButton,true));


        _nameInputField.NamedFailObserver
                       .TakeUntilDestroy(this)
                       .Subscribe(_ =>
                       {
                           _nameSettingFailPopUp.NamedFailSwitch(true);
                           NextButtonState(transitionButton, false);
                           AudioManager.Instance.PlaySE(SEType.Cancel);
                       });

        _transitionButton.NextClickObserver
                 .TakeUntilDestroy(this)
                 .Subscribe(clickCount =>
                 {
                     if (clickCount < 2 || !_namedCorrect)
                         AudioManager.Instance.PlaySE(SEType.Decide1);
                     NextTransitionButtonClick(clickCount);
                 });
    }

    /// <summary>
    /// 次の画面に推移するボタンの処理
    /// </summary>
    /// <param name="clickCount">クリック回数</param>
    private void NextTransitionButtonClick(int clickCount)
    {
        if (clickCount == 1)
        {
            _nameInputField.NameFieldNonAvailable(_joinAmount);
            _nextActiveUiParent.SetActive(true);
            _nextHideUiParent.SetActive(false);
            _nextTMPAnimController.TMPAnimationSwitcher(false);
        }
        else
            _namedCorrect = _nameInputField.NameAndCountChecker();
    }

    /// <summary>
    /// 次の画面に推移するボタンのアニメーションの状況変更
    /// </summary>
    /// <param name="transitionButton">推移するボタン</param>
    private void NextButtonState(Button transitionButton, bool isStart)
    {
        if (isStart && !transitionButton.interactable)
            transitionButton.interactable = _joinAmount > 0;
        if (isStart == true && _nextTMPAnimController.OnAnimation)
            return;

        _nextTMPAnimController.TMPAnimationSwitcher(isStart);
    }
}
