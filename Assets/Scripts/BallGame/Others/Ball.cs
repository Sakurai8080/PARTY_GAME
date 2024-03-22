using UnityEngine;
using TMPro;

/// <summary>
/// ボールの機能
/// </summary>
public class Ball : MonoBehaviour
{
    private TextMeshProUGUI _nameText = default;

    private void Start()
    {
        _nameText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _nameText.enabled = false;
        BallController.Instance.BallListRemover(this);
    }
}
