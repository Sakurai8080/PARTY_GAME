using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;

/// <summary>
/// ボールに関わるUIを操作するコンポーネント
/// </summary>
public class BallUIsController : MonoBehaviour
{
    [Header("変数")]
    [Tooltip("名前表示用テキスト")]
    [SerializeField]
    private TextMeshProUGUI _nameTMP;

    [Tooltip("名前とボールを関連付けするボタン")]
    [SerializeField]
    private Button _ballNamedButon = default;

    [Tooltip("UIを表示させるボール")]
    [SerializeField]
    private Ball _targetBall;


    private RectTransform _nameTextRect;
    private RectTransform _ballButtonRect;
    private Vector3 _textOffsetYpos = new Vector3(0,0.2f, 0);


    void Start()
    {
        _nameTextRect = _nameTMP.GetComponent<RectTransform>();
        _ballButtonRect = _ballNamedButon.GetComponent<RectTransform>();
        _ballButtonRect.position = RectTransformUtility.WorldToScreenPoint(Camera.main, _targetBall.transform.position);

        BallGameManager.Instance.InGameObserver
                                .TakeUntilDestroy(this)
                                .Subscribe(value =>
                                {
                                    if (value)
                                        TextOffsetChange();
                                });

        _ballNamedButon.OnClickAsObservable()
                       .TakeUntilDestroy(this)
                       .Subscribe(_ =>
                       {
                           _ballNamedButon.interactable = false;
                           BallController.Instance.BallAddDictionary(_targetBall);
                           TextSetup();
                           BallGameManager.Instance.ChooseBall();
                           NameLifeManager.Instance.NameListOrderChange();
                       });
    }

    void FixedUpdate()
    {
        _nameTextRect.position = RectTransformUtility.WorldToScreenPoint(Camera.main,_targetBall.transform.position + _textOffsetYpos);
    }

    /// <summary>
    /// ボール上部に名前の表記
    /// </summary>
    private void TextSetup()
    {
        string currentName = NameLifeManager.Instance.CurrentNameReciever();
        _nameTMP.text = currentName.Length >= 3 ? currentName.Substring(0, 3) : currentName;
        _nameTMP.DOFade(1, 0.25f);
    }

    /// <summary>
    /// カメラアングルが変わったときのポジション変更
    /// </summary>
    private void TextOffsetChange()
    {
        Vector3 onGameOffset = new Vector3(0, 0, 0.2f);
        float duration = 6f;
        TextFadeSwitcher(0.25f);
        DOTween.To(() => _textOffsetYpos, (value) => _textOffsetYpos = value, onGameOffset, duration).SetEase(Ease.InOutSine);
    }

    /// <summary>
    /// カメラ移動演出時のテキストを隠す処理
    /// </summary>
    /// <param name="duration">アクティブを切り替える時間</param>
    private void TextFadeSwitcher(float duration)
    {
        _nameTMP.DOFade(0, duration)
                 .SetEase(Ease.Linear)
                 .OnComplete(async () =>
                 {
                     await UniTask.Delay(TimeSpan.FromSeconds(4));
                     duration = 3;
                     _nameTMP.DOFade(1, duration)
                              .SetEase(Ease.InFlash);
                 });
    }
}
