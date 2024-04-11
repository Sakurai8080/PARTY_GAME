using UnityEngine;
using TMPro;

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
        _ballUIsController.TextFadeSwitcher(1, 0);
        BallController.Instance.BallListRemover(this);
    }
}
