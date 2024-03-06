using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    [SerializeField]
    Transform _ballParent = default;

    [SerializeField]
    GameObject _ballPrefab = default;

    [SerializeField]
    float _radius = 0.5f;

    [SerializeField]
    List<Color> _ballColor = default;

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

    private IEnumerator BallInstance()
    {
        WaitForSeconds waitTime = new WaitForSeconds(0.5f);
        _ballParent.gameObject.SetActive(true);

        //TODO:参加人数に変更
        for (int i = 0; i < 8; i++)
        {
            float angle = (360 / 8) * i + 90f;

            float radian = angle * Mathf.Deg2Rad;

            Vector3 ballPos = _ballParent.transform.position + new Vector3(_radius * MathF.Cos(radian), _radius * Mathf.Sin(radian),0f);

            GameObject currentBall = Instantiate(_ballPrefab, ballPos, Quaternion.identity,_ballParent);
            SetBallColor(ref currentBall, i);
            yield return waitTime;
        }
    }
}
