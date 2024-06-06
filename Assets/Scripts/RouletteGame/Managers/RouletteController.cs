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

    private Tween _currentTween;
    private Dictionary<double, string> _angleNameDic = new Dictionary<double, string>();
    private List<float> _angleList = new List<float>();
    private int _peopleAmount = 0;

    protected override void Awake(){}

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
        if (count > 2)
            return;
        else if (count == 2)
        {
            Ease easeType =  Ease.OutBack;
            RouletteFinishEaseDecide(out easeType, UnityEngine.Random.Range(0,3));
            Debug.Log(easeType);
            float randomZvalue = UnityEngine.Random.Range(720, 2000);
            _currentTween.Kill();
            _currentTween = _rouletteObject.transform.DOLocalRotate(new Vector3(0, 0, randomZvalue), 4f, RotateMode.FastBeyond360)
                                                     .SetEase(easeType)
                                                     .OnComplete(async () =>
                                                     {
                                                         DeterminePerson();
                                                         await UniTask.Delay(TimeSpan.FromSeconds(1));
                                                         GameManager.Instance.SceneLoader("GameSelect");
                                                     });
        }
        else if (count == 1)
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

    private void RouletteFinishEaseDecide(out Ease changedEase, int randomNum)
    {
        Debug.Log(randomNum);
        switch(randomNum)
        {
            case 0: changedEase = Ease.OutBack;
                break;
            case 1: changedEase = Ease.OutBounce;
                break;
            default:changedEase = Ease.OutCirc;
                break;
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
