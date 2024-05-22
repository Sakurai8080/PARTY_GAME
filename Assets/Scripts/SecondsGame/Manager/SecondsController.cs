using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;
using System.Threading;
using System.Diagnostics;

/// <summary>
/// 10秒ゲームの秒数コントローラー
/// </summary>
public class SecondsController : SingletonMonoBehaviour<SecondsController>
{
    [Header("変数")]
    [Tooltip("秒数を表示するTMP")]
    [SerializeField]
    private TextMeshProUGUI _secondsText = default;

    [Tooltip("カウントアップスタートボタン")]
    [SerializeField]
    private Button _countToggleButton = default;

    private bool _inProgress = false;
    private TimeSpan _currentTime = default;
    private Stopwatch _stopWatch = new Stopwatch();
    private CancellationTokenSource _cancellationTokenSource;

    private Action _resetUpCallBack;
    private ReactiveProperty<float> _currentActiveTime = new ReactiveProperty<float>();

    protected override void Awake()
    {
        SetupSecounds();
        _resetUpCallBack += () =>{
                                    FadeManager.Instance.OrderChangeFadeAnimation().Forget();
                                    SetupSecounds();
                                 };
    }

    private void Start()
    {
        _currentActiveTime.TakeUntilDestroy(this)
                          .Subscribe(value =>
                          {
                              _currentTime = TimeSpan.FromSeconds(value);
                              _secondsText.text = string.Format("{0:D2}:{1:D2}", _currentTime.Seconds, _currentTime.Milliseconds/10);
                          });
    }

    /// <summary>
    /// 秒数の初期設定
    /// </summary>
    private void SetupSecounds()
    {
        _secondsText.text = "00:00";
        _currentTime = TimeSpan.Zero;
        _stopWatch.Reset();
    }

    /// <summary>
    /// 時間と名前をマネージャーへ紐づける
    /// </summary>
    private void TimeAndNamePass()
    {
        string currentName = NameLifeManager.Instance.CurrentNameReciever();
        SecondsGameManager.Instance.TimeNameAdd(_currentTime, currentName, _resetUpCallBack);
        NameLifeManager.Instance.NameListOrderChange();
    }

    /// <summary>
    /// カウントアップタスク
    /// </summary>
    public async UniTask SecondsCountUpAsync()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _stopWatch.Start();
        int inactiveTime = UnityEngine.Random.Range(2, 10);
        try
        {
            while (_inProgress && !_cancellationTokenSource.IsCancellationRequested)
            {
                _currentActiveTime.Value = (float)_stopWatch.Elapsed.TotalSeconds;

                await UniTask.Yield(_cancellationTokenSource.Token);
                if (_currentActiveTime.Value > inactiveTime)
                {
                    _secondsText.DOFade(0, 0.5f).SetEase(Ease.Linear);
                }
            }
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError($"カウントアップ中にエラーが発生: {ex.Message}");
        }
        finally
        {
            _stopWatch.Stop();
            AfterCountUp(_cancellationTokenSource.Token).Forget();
        }
    }

    /// <summary>
    /// カウント中の切り替え
    /// </summary>
    /// <param name="onButton">カウントアップボタン</param>
    public void ToggleInProgress(bool onButton)
    {
        _inProgress = onButton;
    }

    /// <summary>
    /// カウント後のタスク
    /// </summary>
    /// <param name="cancellationToken">キャンセル処理トークン</param>
    private async UniTask AfterCountUp(CancellationToken cancellationToken)
    {
        try
        {
            _countToggleButton.enabled = false;
            _secondsText.DOFade(1, 1).SetEase(Ease.Linear);
            await UniTask.Delay(TimeSpan.FromSeconds(2), cancellationToken: cancellationToken);
            TimeAndNamePass();
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError($"カウントアップ後にエラーが発生: {ex.Message}");
        }
        finally
        {
            _countToggleButton.enabled = (SecondsGameManager.Instance.CurrentCompleteAmount == NameLifeManager.Instance.GamePlayerAmount) ? false : true;
            CancelToken();
        }
    }

    private void CancelToken()
    {
        _cancellationTokenSource?.Cancel();
    }
}
