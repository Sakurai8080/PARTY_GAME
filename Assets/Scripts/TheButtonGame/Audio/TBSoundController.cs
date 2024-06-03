using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TBSoundController : MonoBehaviour
{
    private SEType _se;

    void Start()
    {
        FadeManager.Instance.NameAnimFadeCompletedObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(_ => ButtonAmountCheck());
    }

    /// <summary>
    /// ボタンの数に応じてSEを変更する。
    /// </summary>
    private void ButtonAmountCheck()
    {
        int currentActiveButtonAmount = TBGameManager.Instance.CurrentActiveAmount;
        switch (currentActiveButtonAmount)
        {
            case 2:
                _se = SEType.Fifty;
                break;
            case 3:
                _se = SEType.SixtySix;
                break;
            case 4:
                _se = SEType.SeventyFive;
                break;
            case 5:
                _se = SEType.Eighty;
                break;
            case 1:
                _se = SEType.OneHundred;
                break;
            default:
                Debug.LogError($"<colot=red>ボタンの数:{currentActiveButtonAmount}</color> が使用範囲外です。");
                break;
        }
        OnSound(_se);
    }

    private void OnSound(SEType se)
    {
        AudioManager.Instance.PlaySE(se);
    }
}
