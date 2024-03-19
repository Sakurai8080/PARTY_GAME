using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;
using TMPro;
using System.Threading;
using System.Diagnostics;

public class SecondsController : SingletonMonoBehaviour<SecondsController>
{
    private ReactiveProperty<float> _currentActiveTime = new ReactiveProperty<float>();

    [SerializeField]
    private TextMeshProUGUI _secondsText = default;

    private CancellationTokenSource _cancellationTokenSource;

    private bool _inProgress = false;

    private TimeSpan _currentTime = default;
    private Action _resetUpCallBack = default;

    private Stopwatch _stopWatch = new Stopwatch();

    private void Awake()
    {
        SetupSecounds();
        _resetUpCallBack += SetupSecounds;
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

    public void SetupSecounds()
    {
        _secondsText.text = "00:00";
        _currentTime = TimeSpan.Zero;
        _stopWatch.Reset();
    }

    public async UniTask SecondsCountUpAsync()
    {
        _stopWatch.Start();
        while (_inProgress)
        {
            _currentActiveTime.Value = (float)_stopWatch.Elapsed.TotalSeconds;
            await UniTask.Yield();
        }
        _stopWatch.Stop();
        TimeAndNamePass();
    }

    public void ToggleInProgress(bool onButton)
    {
        _inProgress = onButton;
    }

    public void CancelCountUp()
    {
        _cancellationTokenSource?.Cancel();
    }

    private void TimeAndNamePass()
    {
        string currentName = NameLifeManager.Instance.CurrentNameReciever();
        SecondsGameManager.Instance.TimeNameAdd(_currentTime, currentName, _resetUpCallBack);
        NameLifeManager.Instance.NameListOrderChange();
    }
}
