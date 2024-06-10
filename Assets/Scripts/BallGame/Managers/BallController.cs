using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine.UI;

/// <summary>
/// ボールの挙動を管理するコンポーネント
/// </summary>
public class BallController : SingletonMonoBehaviour<BallController>
{
    public IObservable<Unit> AllBallInstancedObserver => _allBallInstancedSubject;

    [Header("変数")]
    [Tooltip("ボールを配置する親オブジェクト")]
    [SerializeField]
    private Transform _ballParent = default;

    [Tooltip("転がすボール")]
    [SerializeField]
    private Ball _ballPrefab = default;

    [Tooltip("ボールを生成する半径")]
    [SerializeField]
    private float _radius = 0.5f;

    [Tooltip("ボールを見分けるための色")]
    [SerializeField]
    private List<Color> _ballColor = default;

    private List<Ball> _ballObjList = new List<Ball>();
    private Dictionary<Ball, string> _ballNameDic = new Dictionary<Ball, string>();

    private Subject<Unit> _allBallInstancedSubject = new Subject<Unit>();

    protected override void Awake(){}

    public void Setup()
    {
        StartCoroutine(BallInstance());
    }

    /// <summary>
    /// ゲーム開始時アングル変更に伴う回転
    /// </summary>
    public void RotateBallParent()
    {
        _ballParent.DOLocalRotate(new Vector3(90, 0, 0), 5)
                   .SetEase(Ease.InExpo);
    }

    /// <summary>
    /// ゲーム開始時全てのボールを落とす処理
    /// </summary>
    public void OnAllGravity()
    {
        List<Rigidbody> rigidbodies = new List<Rigidbody>();
        rigidbodies.Clear();
        rigidbodies = _ballObjList.Select(x => x.GetComponent<Rigidbody>()).ToList();
        rigidbodies.ForEach(x => x.useGravity = true);
        rigidbodies.ForEach(x => RandomForce(x));
    }

    /// <summary>
    /// ボールが脱出したときの処理
    /// </summary>
    /// <param name="removeball">負けを逃れたボール</param>
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

    /// <summary>
    /// 誰のボールかを紐づける処理
    /// </summary>
    /// <param name="ball">名前と紐づけるためのボール</param>
    public void BallAddDictionary(Ball ball)
    {
        _ballNameDic.Add(ball, NameLifeManager.Instance.CurrentNameReciever());
    }

    /// <summary>
    /// ボールインスタンス時の色付け
    /// </summary>
    /// <param name="ball">色付けするボール</param>
    /// <param name="currentNum">カラーの要素数</param>
    private void SetBallColor(ref Ball ball,int currentNum)
    {
        Renderer renderer = ball.GetComponent<Renderer>();
        renderer.material.DOColor(_ballColor[currentNum], 0);
        renderer.material.DOFade(0, 0f);
        ball.gameObject.SetActive(true);
        renderer.material.DOFade(1, 0.75f)
                         .SetEase(Ease.Linear)
                         .SetDelay(0.1f);
    }

    /// <summary>
    /// ゲーム開始時にランダムの方向に飛ばすための処理
    /// </summary>
    /// <param name="rigidbody">ボール飛ばすため</param>
    private void RandomForce(Rigidbody rigidbody)
    {
        float randomXforce  = UnityEngine.Random.Range(-12f, 12f), randomZforce = UnityEngine.Random.Range(-10f, 16f);
        rigidbody.AddForce(randomXforce, 1, randomZforce);
    }

    /// <summary>
    /// ボールを順々に生成する処理
    /// </summary>
    /// <returns>生成間隔</returns>
    private IEnumerator BallInstance()
    {
        WaitForSeconds waitTime = new WaitForSeconds(0.35f);
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
        _allBallInstancedSubject.OnNext(Unit.Default);
    }
}
