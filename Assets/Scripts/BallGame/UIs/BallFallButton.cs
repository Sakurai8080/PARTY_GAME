using System;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

/// <summary>
/// /ボールを落とすボタン
/// </summary>
public class BallFallButton : MonoBehaviour
{
    public IObservable<Unit> FallButtonClickObserver => _fallButtonClickSubject;

    private Subject<Unit> _fallButtonClickSubject = new Subject<Unit>();

    private void Start()
    {
        GetComponent<Button>().OnClickAsObservable()
                              .TakeUntilDestroy(this)
                              .Subscribe(_ =>
                              {
                                  _fallButtonClickSubject.OnNext(Unit.Default);
                                  gameObject.SetActive(false);
                                  BallGameManager.Instance.GameStateSwitcher(true);
                              });
    }
}
