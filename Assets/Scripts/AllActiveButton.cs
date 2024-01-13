using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading;
using UnityEngine.UI;

public class AllActiveButton : MonoBehaviour
{

    [Header("Variable")]
    [Tooltip("TestUIをまとめたグループ")]
    [SerializeField]
    private CanvasGroup _testUIGroup;

    [Tooltip("TestUIをまとめら親空オブジェクト")]
    [SerializeField]
    private GameObject _testUIParent;

    [Tooltip("α値を切り替えるボタン")]
    [SerializeField]
    private Button _allActiveButton;

    // Start is called before the first frame update
    void Start()
    {
        _allActiveButton.OnClickAsObservable()
                        .TakeUntilDestroy(this)
                        .Subscribe(_ =>
                        {
                            _testUIGroup.alpha = 1;
                            _testUIParent.gameObject.SetActive(true);
                        });
                        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
