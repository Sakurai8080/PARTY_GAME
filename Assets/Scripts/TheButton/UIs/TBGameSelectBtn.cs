using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;

public class TBGameSelectBtn : MonoBehaviour
{
    [SerializeField]
    Button _theButton = default;

    void Start()
    {
        _theButton.OnClickAsObservable()
                  .TakeUntilDestroy(this)
                  .Subscribe(_ =>
                  {
                      TBGameManager.Instance.Test(_theButton);
                      ButtonCheck();
                  });
    }

    private void ButtonCheck()
    {
        bool isMiss = TBGameManager.Instance.MissButtonChecker(_theButton);
        if (isMiss)
        {
            GameManager.Instance.SceneLoader("MainScene");
            return;
        }
        else
        {
            //PopUp機能で名前表示してボタンをリセット
            TBGameManager.Instance.buttonReconfigure();
        }
    }
}
