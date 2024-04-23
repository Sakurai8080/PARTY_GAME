using UnityEngine;
using System;

[Serializable]
public class BGM
{
    public string BGMName;
    public BGMType BGMType;
    public AudioClip Clip;
    [Range(0, 1f)]
    public float Volume = 1f;
}

public enum BGMType
{
    None,
    InitSetting,
    InGame,
    Result
}
