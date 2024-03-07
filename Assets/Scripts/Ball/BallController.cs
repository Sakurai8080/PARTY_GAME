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
    GameObject _ballPrefab = default;

    [SerializeField]
    float _radius = 0.5f;

    [SerializeField]
    List<Color> _ballColor = default;

    List<GameObject> _ballObjList = new List<GameObject>();

    public void Setup()
    {
        StartCoroutine(BallInstance());
    }

    private void SetBallColor(ref GameObject ball,int currentNum)
    {
        Renderer renderer = ball.GetComponent<Renderer>();
        renderer.material.DOColor(_ballColor[currentNum], 0);
        renderer.material.DOFade(0, 0f);
        ball.SetActive(true);

        renderer.material.DOFade(1, 0.75f)
                         .SetEase(Ease.Linear);
    }

    public void RotateBallParent()
    {
        _ballParent.DOLocalRotate(new Vector3(90, 0, 0),5)
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

    private void RandomForce(Rigidbody rigidbody)
    {
        float randomXforce = UnityEngine.Random.Range(-50f, 50f), randomZforce = UnityEngine.Random.Range(-50f, 50f);
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

            GameObject currentBall = Instantiate(_ballPrefab, ballPos, Quaternion.identity,_ballParent);
            SetBallColor(ref currentBall, i);
            _ballObjList.Add(currentBall);
            yield return waitTime;
        }
    }
}
