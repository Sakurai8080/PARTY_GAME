using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TweenBase : MonoBehaviour
{
    protected float a = 1.0f;

    protected abstract void UiLoopAnimation();
}
