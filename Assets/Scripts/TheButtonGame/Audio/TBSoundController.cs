using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TBSoundController : MonoBehaviour
{
    private SEType _se;

    void Start()
    {
        TBGameManager.Instance.TurnChangeObserver
                              .TakeUntilDestroy(this)
                              .Subscribe(activeAmount => ButtonAmountCheck(activeAmount));
    }

    /// <summary>
    /// ボタンの数に応じてSEを変更する。
    /// </summary>
    /// <param name="activeButtonAmount">出現するボタンの数</param>
    private void ButtonAmountCheck(int activeButtonAmount)
    {
        switch (activeButtonAmount)
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
                Debug.LogError($"<colot=red>ボタンの数:{activeButtonAmount}</color> が使用範囲外です。");
                break;
        }
        OnSound(_se);
    }

    private void OnSound(SEType se)
    {
        AudioManager.Instance.PlaySE(se);
    }
}
