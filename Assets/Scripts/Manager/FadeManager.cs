using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using TMPro;
using UniRx;

/// <summary>
/// 画面のフェードを管理するクラス
/// </summary>
public class FadeManager : SingletonMonoBehaviour<FadeManager>
{
     
    public IObservable<Unit> NameAnimStartObserver => _nameAnimStartSubject;
    public IObservable<Unit> NameAnimFadeCompletedObserver => _nameAnimFadeCompletedSubject;

    [Header("変数")]
    [Tooltip("フェードにかける時間")]
    [SerializeField]
    private float _fadeTime = 1.0f;

    [Tooltip("フェード用マテリアル")]
    [SerializeField]
    private Material _fadeMaterial = null;

    [Tooltip("順番変更時のフェード用パネル")]
    [SerializeField]
    private Image _orderChangePanel = default;

    [Tooltip("次の順番を表示する名前用TMP")]
    [SerializeField]
    private TextMeshProUGUI _nextOrderName = default;

    [Tooltip("ネクストTMP")]
    [SerializeField]
    private TextMeshProUGUI _nextTMP = default;

    [Tooltip("名前表示周りのグループ")]
    [SerializeField]
    private GameObject _nameGroup = default;

    private readonly int _progressId = Shader.PropertyToID("_progress");
    private bool _isFading = false;
    private Vector3 _nextTextInitPos;
    private Vector3 _nameGroupInitPos;
    private Sequence _nextTMPSequence;
    private Sequence _nameGroupSequence;

    private Subject<Unit> _nameAnimStartSubject = new Subject<Unit>();
    private Subject<Unit> _nameAnimFadeCompletedSubject = new Subject<Unit>();

    private void Start()
    {
        InitializeSequence();
        _nextTextInitPos = _nextTMP.transform.position;
        _nameGroupInitPos = _nameGroup.transform.position;
    }

    private void InitializeSequence()
    {
        _nextTMPSequence = DOTween.Sequence();
        _nextTMPSequence.Append(_nextTMP.transform.DOLocalMoveX(-30, 0.25f).SetEase(Ease.InBack))
                        .Append(_nextTMP.transform.DOLocalMoveX(400, 0.25f).SetEase(Ease.InBack).SetDelay(2.5f))
                        .SetAutoKill(false)
                        .Pause();

        _nameGroupSequence = DOTween.Sequence();

        _nameGroupSequence.Append(_nameGroup.transform.DOLocalMoveX(0, 0.25f).SetEase(Ease.OutQuad).SetDelay(0.25f))
                          .Append(_nameGroup.transform.DOLocalMoveX(400, 0.25f).SetEase(Ease.InBack).SetDelay(2.5f)
                          .OnComplete(NameFadeReset))
                          .SetAutoKill(false)
                          .Pause();
    }

    private void NameFadeReset()
    {
        _nextTMP.transform.position = _nextTextInitPos;
        _nameGroup.transform.position = _nameGroupInitPos;
        _nameAnimFadeCompletedSubject.OnNext(Unit.Default);
        _orderChangePanel.DOFade(0, 0.25f)
                         .OnComplete(() =>
                         {
                             _orderChangePanel.gameObject.SetActive(false);
                         });
    }

    public async UniTask OrderChangeFadeAnimation(float durationTime = 0)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(durationTime));
        _orderChangePanel.gameObject.SetActive(true);
        string nextName = NameLifeManager.Instance.CurrentNameReceiver();
        _nextOrderName.text = nextName.Length > 5 ? nextName.Substring(0, 5) : nextName;
        _orderChangePanel.DOFade(0.95f, 0.25f)
                         .SetEase(Ease.InQuint)
                         .OnComplete(() =>
                         {
                             _nextTMPSequence.Restart();
                             _nameGroupSequence.Restart();
                             _nameAnimStartSubject.OnNext(Unit.Default);
                         });
    }

    public void Fade(FadeType type, Action callback = null)
    {
        if (_isFading)
            return;

        StartCoroutine(ShaderFade(type, callback));
    }

    private IEnumerator ShaderFade(FadeType type, Action callBack = null)
    {
        (float currentValue, float endValue) = type == FadeType.In ? (0f, 1f) : (1f, 0);

        yield return DOTween.To(() => currentValue,
                                 x => currentValue = x,
                                 endValue,
                                 _fadeTime)
                                 .SetEase(Ease.Linear)
                                 .OnUpdate(() =>
                                 {
                                     _fadeMaterial.SetFloat(_progressId, currentValue);
                                 })
                                 .WaitForCompletion();
        _fadeMaterial.SetFloat(_progressId, (type == FadeType.In) ? 1f : 0f);
        _isFading = false;

        callBack?.Invoke();
    }
}

public enum FadeType
{
    In,
    Out
}
