using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine.UI;

public class BallController : SingletonMonoBehaviour<BallController>
{
    [SerializeField]
    Transform _ballParent = default;

    [SerializeField]
    Ball _ballPrefab = default;

    [SerializeField]
    float _radius = 0.5f;

    [SerializeField]
    List<Color> _ballColor = default;

    List<Ball> _ballObjList = new List<Ball>();

    Dictionary<Ball, string> _ballNameDic = new Dictionary<Ball, string>();

    public void Setup()
    {
        StartCoroutine(BallInstance());
    }

    public void RotateBallParent()
    {
        _ballParent.DOLocalRotate(new Vector3(90, 0, 0), 5)
                   .SetEase(Ease.InExpo);
    }

    public void OnAllGravity()
    {
        List<Rigidbody> rigidbodies = new List<Rigidbody>();
        rigidbodies.Clear();
        rigidbodies = _ballObjList.Select(x => x.GetComponent<Rigidbody>()).ToList();
        rigidbodies.ForEach(x => x.useGravity = true);
        rigidbodies.ForEach(x => RandomForce(x));
    }

    public void BallListRemover(Ball removeball)
    {
        _ballObjList.Remove(removeball);
        if (_ballObjList.Count() <= 1)
        {
            string remainingName = _ballNameDic[_ballObjList[0]];
            NameLifeManager.Instance.ReduceLife(remainingName);
            GameManager.Instance.SceneLoader("GameSelect");
        }
    }

    public void BallAddDictionary(Ball ball)
    {
        _ballNameDic.Add(ball, NameLifeManager.Instance.CurrentNameReciever());
    }

    private void SetBallColor(ref Ball ball,int currentNum)
    {
        Renderer renderer = ball.GetComponent<Renderer>();
        renderer.material.DOColor(_ballColor[currentNum], 0);
        renderer.material.DOFade(0, 0f);
        ball.gameObject.SetActive(true);

        renderer.material.DOFade(1, 0.75f)
                         .SetEase(Ease.Linear);
    }

    private void RandomForce(Rigidbody rigidbody)
    {
        float randomXforce = UnityEngine.Random.Range(-100f, 100f), randomZforce = UnityEngine.Random.Range(-100f, 100f);
        rigidbody.AddForce(randomXforce, 0, randomZforce);
    }


    private IEnumerator BallInstance()
    {
        WaitForSeconds waitTime = new WaitForSeconds(0.5f);
        _ballParent.gameObject.SetActive(true);

        for (int i = 0; i < NameLifeManager.Instance.GamePlayerAmount; i++)
        {
            float angle = (360 / NameLifeManager.Instance.GamePlayerAmount) * i + 90f;

            float radian = angle * Mathf.Deg2Rad;

            Vector3 ballPos = _ballParent.transform.position + new Vector3(_radius * MathF.Cos(radian), _radius * Mathf.Sin(radian),0f);

            Ball currentBall = Instantiate(_ballPrefab, ballPos, Quaternion.identity,_ballParent);
            SetBallColor(ref currentBall, i);
            _ballObjList.Add(currentBall);
            yield return waitTime;
        }
    }
}
