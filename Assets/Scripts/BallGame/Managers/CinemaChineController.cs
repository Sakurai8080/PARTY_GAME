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

    private CinemachineTrackedDolly _dolly;
    private Dictionary<CameraType, CinemachineVirtualCamera> _cameraDic = new Dictionary<CameraType, CinemachineVirtualCamera>();
    private float _pathPositionMax;
    private Action _dollyFinalCallBack;

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

    /// <summary>
    /// ドリーカメラを動かす処理
    /// </summary>
    public void DollySet()
    {
        _dolly = _cameraDic[CameraType.cam2].GetCinemachineComponent<CinemachineTrackedDolly>();
        _pathPositionMax = _dolly.m_Path.MaxPos;
        _dollyFinalCallBack += BallController.Instance.OnAllGravity;
        DollyMoveTask(_dollyFinalCallBack).Forget();
    }

    /// <summary>
    /// ドリーカメラの操作
    /// </summary>
    /// <param name="callBack">ドリーが終わったあとのEvent</param>
    /// <returns>待ち合わせ時間</returns>
    private async UniTask DollyMoveTask(Action callBack = null)
    {
        await UniTask.Delay(TimeSpan.FromMilliseconds(500));
        float duration = 5f;

        DOTween.To(() => _dolly.m_PathPosition,
                value => _dolly.m_PathPosition = value,
                _pathPositionMax, duration)
                .SetEase(Ease.InOutFlash)
                .OnComplete(async () =>
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(1));
                    callBack?.Invoke();
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
