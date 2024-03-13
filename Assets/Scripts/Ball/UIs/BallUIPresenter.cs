using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class BallUIPresenter : PresenterBase
{
    [SerializeField]
    BallFallButton _fallButton;

    [SerializeField]
    GameObject _backGround = default;

    protected override void Start()
    {
        _currentHideUIs.OnClickObserver
                       .TakeUntilDestroy(this)
                       .Subscribe(_ =>
                       {
                           _nextActiveUIs.ToggleUIsVisibility();
                           _currentHideUIs.gameObject.SetActive(false);
                           _backGround.SetActive(false);
                           BallController.Instance.Setup();
                       });
        
        _fallButton.FallButtonClickObserver
                   .TakeUntilDestroy(this)
                   .Subscribe(_ =>
                   {
                       CinemaChineController.Instance.ActivateCameraChange(CameraType.cam2);
                       CinemaChineController.Instance.DollySet();
                       BallController.Instance.RotateBallParent();
                   });
    }
}
