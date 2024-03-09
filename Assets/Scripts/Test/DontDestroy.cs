using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : SingletonMonoBehaviour<DontDestroy>
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
