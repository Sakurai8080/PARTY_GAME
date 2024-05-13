using UnityEngine;
using System;

[Serializable]
public class SE
{
    public string SEName;
    public SEType SEType;
    public AudioClip Clip;
    [Range(0f, 1f)]
    public float Volume = 1f;
}

public enum SEType
{
    Dice,
    Fifty,
    SixtySix,
    SeventyFive,
    Eighty,
    OneHundred,
}
