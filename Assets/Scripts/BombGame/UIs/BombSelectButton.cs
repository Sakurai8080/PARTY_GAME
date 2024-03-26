using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// 選択するカードのボタン
/// </summary>
public class BombSelectButton : MonoBehaviour
{
    [Header("カードに付くボタン")]
    [SerializeField]
    private Button _bombButton = default;

    private BombAnim _bombAnim;

    private void Start()
    {
        _bombAnim = GetComponent<BombAnim>();

        _bombButton.OnClickAsObservable()
                   .TakeUntilDestroy(this)
                   .Subscribe(_ =>
                   {
                       BombUIsAnimationController.Instance.CardSelected(_bombAnim);
                       _bombAnim.SelectedAnimation();
                   });
    }
}
