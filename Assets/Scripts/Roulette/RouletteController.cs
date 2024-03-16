using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class RouletteController : SingletonMonoBehaviour<RouletteController>
{
    public IObservable<Unit> RouletteStopObserver => _rouletteStopSubject;

    private Subject<Unit> _rouletteStopSubject = new Subject<Unit>();

    [SerializeField]
    private GameObject _rouletteObject;

    [SerializeField]
    private TextMeshProUGUI _text;

    private Tween _currentTween;
    private Dictionary<double, string> _angleNameDic = new Dictionary<double, string>();
    private List<float> _angleList = new List<float>();
    private int _peopleAmount = 0;

    private void Start()
    {
        _peopleAmount = NameLifeManager.Instance.GamePlayerAmount;
    }

    public void AngleNameDicAdd(float currentAngle,string name)
    {
        _angleNameDic.Add(currentAngle, name);
        _angleList.Add(currentAngle);
    }

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
                                                         RouletteStop();
                                                         DeterminePerson();
                                                         _text.DOColor(Color.blue, 0);
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

    private void RouletteStop()
    {
        _rouletteStopSubject.OnNext(Unit.Default);
    }
}
