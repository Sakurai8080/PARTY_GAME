using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// ボールの機能
/// </summary>
public class Ball : MonoBehaviour
{
    private BallUIsController _ballUIsController;

    private void Start()
    {
        _ballUIsController = GetComponentInChildren<BallUIsController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        BallController.Instance.GoaledBallCountUp();
        _ballUIsController.GoalTextChange();
        BallController.Instance.BallGoalChecker(this);
    }
}
