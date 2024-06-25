using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// リザルト画面のUIを仲介するプレゼンター
/// </summary>
public class ResultPresenter : MonoBehaviour
{
    [Header("変数")]
    [Tooltip("タイトル画面に推移するボタン")]
    [SerializeField]
    private OnButtonView _titleTransButton = default;

    [Tooltip("人はそのままでリスタートするボタン")]
    [SerializeField]
    private OnButtonView _restartButton = default;

    void Start()
    {
        _titleTransButton.OnClickObserver
                         .TakeUntilDestroy(this)
                         .Subscribe(_ => GameSetupUtility.AllManagerSetup());

        _restartButton.OnClickObserver
                      .TakeUntilDestroy(this)
                      .Subscribe(_ => GameSetupUtility.ResetartSetup());
    }
}
