using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using TMPro;

/// <summary>
/// ルーレットを管理するコントローラー
/// </summary>
public class RouletteController : SingletonMonoBehaviour<RouletteController>
{
    [Header("変数")]
    [Tooltip("ルーレットプレハブ")]
    [SerializeField]
    private GameObject _rouletteObject;

    [Tooltip("回転開始ボタンのTMP")]
    [SerializeField]
    private TextMeshProUGUI _rotateStartTMP;

    private Tween _currentTween;
    private Dictionary<double, string> _angleNameDic = new Dictionary<double, string>();
    private List<float> _angleList = new List<float>();
    private int _peopleAmount = 0;

    private void Start()
    {
        _peopleAmount = NameLifeManager.Instance.GamePlayerAmount;
    }

    /// <summary>
    /// 角度と名前の紐づけ
    /// </summary>
    /// <param name="loseAngle">角度</param>
    /// <param name="name">名前</param>
    public void AngleNameDicAdd(float loseAngle,string name)
    {
        _angleNameDic.Add(loseAngle, name);
        _angleList.Add(loseAngle);
    }

    /// <summary>
    /// 回転ボタンの挙動
    /// </summary>
    /// <param name="count">何回押しているか</param>
    public void RouletteRotate(int count)
    {
        if (count > 1)
            return;
        else if (count == 1)
        {
            float rondomZvalue = UnityEngine.Random.Range(1080, 1440);
            _currentTween.Kill();
            _currentTween = _rouletteObject.transform.DOLocalRotate(new Vector3(0, 0, rondomZvalue), 4f, RotateMode.FastBeyond360)
                                                     .SetEase(Ease.OutBack)
                                                     .OnComplete(async () =>
                                                     {
                                                         DeterminePerson();
                                                         _rotateStartTMP.DOColor(Color.blue, 0);
                                                         await UniTask.Delay(TimeSpan.FromSeconds(1));
                                                         GameManager.Instance.SceneLoader("GameSelect");
                                                     });
        }
        else if (count == 0)
        {
            _currentTween = _rouletteObject.transform.DOLocalRotate(new Vector3(0, 0, 720), 1f,RotateMode.FastBeyond360)
                                                     .SetEase(Ease.InBack)
                                                     .OnComplete(() =>
                                                     {
                                                         _currentTween.Kill();
                                                         _currentTween = _rouletteObject.transform.DOLocalRotate(new Vector3(0, 0, 20), 0.01f)
                                                                                                  .SetEase(Ease.Linear)
                                                                                                  .SetLoops(-1, LoopType.Incremental);
                                                     });
        }
    }

    /// <summary>
    /// 回り終わったあとの負け判定
    /// </summary>
    private void DeterminePerson()
    {
        RectTransform stopRectTransform = _rouletteObject.GetComponent<RectTransform>();
        float angle = stopRectTransform.rotation.eulerAngles.z;

        if (angle <= 0)
            angle = 360 - angle;

        for (int i = 0; i < _angleList.Count; i++)
        {
            try
            {
                if (angle <= _angleList[_peopleAmount - 1 - i])
                {
                    double finaleRotateAmount = _angleList[_peopleAmount - 1 - i];
                    NameLifeManager.Instance.ReduceLife(_angleNameDic[finaleRotateAmount]);
                    break;
                }
            }
            catch
            {
                Debug.LogError($"回転の値が正しくありません。");
            }
        }
    }
}
