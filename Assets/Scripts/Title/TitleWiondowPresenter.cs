using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

/// <summary>
/// タイトルのUIを仲介するプレゼンター
/// </summary>
public class TitleWiondowPresenter : MonoBehaviour
{
    [Header("変数")]
    [Tooltip("タイトルに表示するロゴ")]
    [SerializeField]
    private NaviTextAnimation _titleLogoImage = default;

    [Tooltip("次シーンへ推移する画面を覆うボタン")]
    [SerializeField]
    private OnButtonView _toSettingSceneButton = default;

    void Start()
    {
        _titleLogoImage.AnimationStart();

        _toSettingSceneButton.OnClickObserver
                             .TakeUntilDestroy(this)
                             .Subscribe(_ => GameManager.Instance.SceneLoader("MainScene", BGMType.InitSetting));
    }
}
