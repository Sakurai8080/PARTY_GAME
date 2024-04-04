using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine.UI;
using Cinemachine;

/// <summary>
/// ボールゲーム用中のカメラ操作コンポーネント
/// </summary>
public class CinemaChineController : SingletonMonoBehaviour<CinemaChineController>
{
    [Header("変数")]
    [Tooltip("シネマシーンカメラ")]
    [SerializeField]
    private ActivationCamera[] _virtualCamera;

    [SerializeField]
    private CameraType _currentMoveCam;

    [SerializeField]
    private GameType _currentTypes;

    private CinemachineTrackedDolly _dolly;
    private Dictionary<CameraType, CinemachineVirtualCamera> _cameraDic = new Dictionary<CameraType, CinemachineVirtualCamera>();
    private float _pathPositionMax;
    private float _pathPositionMin;
    private Action _dollyFinalCallBack;
    private Action _returnDollyCallBack;

    private const int PRIORITY_AMOUNT = 11;

    
    private void Awake()
    {
        for (int i = 0; i < _virtualCamera.Length; i++)
            _cameraDic.Add((CameraType)i, _virtualCamera[i].Camera);
    }

    /// <summary>
    /// シネマシーンの変更
    /// </summary>
    /// <param name="cameraType">使用するカメラタイプ</param>
    public void ActivateCameraChange(CameraType cameraType)
    {
        int initPriority = 10;
        _cameraDic.Values.ToList().ForEach(vCam => vCam.Priority = initPriority);
        _cameraDic[cameraType].Priority = PRIORITY_AMOUNT;
    }

    public void DiceCheck(Action callBack = null)
    {
        _returnDollyCallBack += callBack;
        _returnDollyCallBack += () => CameraReturnMove();
        _dolly = _cameraDic[CameraType.cam2].GetCinemachineComponent<CinemachineTrackedDolly>();
        DollyMoveTask(2, 2, 5,Ease.OutExpo , ()=>CameraReturnMove(_returnDollyCallBack)).Forget();
    }

    private void CameraReturnMove(Action callBack = null)
    {
        _pathPositionMin = _dolly.m_Path.MinPos;
        DOTween.To(() => _dolly.m_PathPosition,
        value => _dolly.m_PathPosition = value,
        _pathPositionMin, 0)
        .SetEase(Ease.InFlash);
        callBack?.Invoke();
        _returnDollyCallBack = null;
    }

    /// <summary>
    /// ドリーカメラを動かす処理
    /// </summary>
    public void DollySet(Action callBack = null)
    {
        switch (_currentTypes)
        {
            case GameType.Ball:
                _dolly = _cameraDic[CameraType.cam2].GetCinemachineComponent<CinemachineTrackedDolly>();
                _pathPositionMax = _dolly.m_Path.MaxPos;
                _dollyFinalCallBack += BallController.Instance.OnAllGravity;
                DollyMoveTask(500, 5,1, Ease.InOutFlash, _dollyFinalCallBack).Forget();
                break;

            case GameType.Dice:
                _dolly = _cameraDic[_currentMoveCam].GetCinemachineComponent<CinemachineTrackedDolly>();
                _pathPositionMax = _dolly.m_Path.MaxPos;
                callBack += () => ActivateCameraChange(CameraType.cam2);

                DollyMoveTask(0, 5,0, Ease.Linear, callBack).Forget();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// ドリーカメラの操作
    /// </summary>
    /// <param name="callBack">ドリーが終わったあとのEvent</param>
    /// <returns>待ち合わせ時間</returns>
    private async UniTask DollyMoveTask(float waitTime, float durationTime, float afterWaitTime, Ease ease, Action callBack = null)
    {
        await UniTask.Delay(TimeSpan.FromMilliseconds(waitTime));
        float duration = durationTime;

        DOTween.To(() => _dolly.m_PathPosition,
                value => _dolly.m_PathPosition = value,
                _pathPositionMax, duration)
                .SetEase(ease)
                .OnComplete(async () =>
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(afterWaitTime));
                    callBack?.Invoke();
                    _dollyFinalCallBack = null;
                });
    }
}

[Serializable]
public class ActivationCamera
{
    public string CameraName;
    public CinemachineVirtualCamera Camera;
}

public enum CameraType
{
    cam1,
    cam2
}