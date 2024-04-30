using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.Audio;
using UniRx;

/// <summary>
/// オーディオ関連のマネージャー
/// </summary>
public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    public int CurrentBGMVolume { get; set; } = 5;
    public int CurrentSEVolume { get; set; } = 5;

    [Header("変数")]
    [Tooltip("全体の音量")]
    [SerializeField, Range(0f, 1f)]
    private float _masterVolume = 1.0f;

    [Tooltip("BGMの音量")]
    [SerializeField, Range(0f, 1f)]
    private float _bgmVolume = 1.0f;

    [Tooltip("SEの音量")]
    [SerializeField, Range(0f, 1f)]
    private float _seVolume = 1.0f;

    [Tooltip("SEのAudioSourceの生成数")]
    [SerializeField]
    private int _seAudioSourceAmount = 5;

    [Header("各音源のリスト")]
    [Tooltip("BGMのリスト")]
    [SerializeField]
    private List<BGM> _bgmList = new List<BGM>();

    [Tooltip("SEのリスト")]
    [SerializeField]
    private List<SE> _seList = new List<SE>();

    [Header("使用する各オブジェクト")]
    [Tooltip("BGM用のオーディオソース")]
    [SerializeField]
    private AudioSource _bgmSource = default;

    [Tooltip("SE用のAudioSourceをまとめるオブジェクト")]
    [SerializeField]
    private Transform _seSourcesParent = default;

    [Tooltip("AudioMixer")]
    [SerializeField]
    private AudioMixer _mixer = default;

    List<AudioSource> _seAudioSourceList = new List<AudioSource>();
    private bool _isStoping = false;

    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < _seAudioSourceAmount; i++)
        {
            var obj = new GameObject($"SESource{i + 1}");
            obj.transform.SetParent(_seSourcesParent);

            var source = obj.AddComponent<AudioSource>();

            if (_mixer != null)
            {
                source.outputAudioMixerGroup = _mixer.FindMatchingGroups("Master")[2];
            }

            _seAudioSourceList.Add(source);
        }
    }

    /// <summary>
    /// BGMの再生
    /// </summary>
    /// <param name="type">BGMの種類</param>
    /// <param name="loopType">ループするかしないか</param>
    public void PlayBGM(BGMType type, bool loopType = true)
    {
        var bgm = GetBGM(type);

        try
        {
            if (bgm != null)
            {
                if (Instance._bgmSource.clip == null)
                {
                    Instance._bgmSource.clip = bgm.Clip;
                    Instance._bgmSource.loop = loopType;
                    Instance._bgmSource.volume = Instance._bgmVolume * Instance._masterVolume * bgm.Volume;
                    Instance._bgmSource.Play();
                    Debug.Log($"{bgm.BGMName}を再生");
                }
                else
                {
                    Instance.StartCoroutine(Instance.SwitchingBgm(bgm, loopType));
                    Debug.Log($"{bgm.BGMName}を再生");
                }
            }
        }
        catch
        {
            Debug.LogError($"BGM:{type}を再生出来ませんでした");
        }
    }

    /// <summary>
    /// SEの再生
    /// </summary>
    /// <param name="type">SEの種類</param>
    public void PlaySE(SEType type)
    {
        var se = GetSE(type);

        try
        {
            if (se != null)
            {
                foreach (var s in Instance._seAudioSourceList)
                {
                    if (!s.isPlaying)
                    {
                        s.PlayOneShot(se.Clip, Instance._seVolume * Instance._masterVolume);
                        return;
                    }
                }
            }
        }
        catch
        {
            Debug.LogError($"BGM:{type}を再生出来ませんでした");
        }
    }

    /// <summary>
    /// BGMの停止　
    /// </summary>
    public void StopBGM()
    {
        Instance._bgmSource.Stop();
        Instance._bgmSource = null;
    }

    /// <summary>
    /// BGMの停止させる時間の操作
    /// </summary>
    /// <param name="stopTime">停止するまでの時間</param>
    public void GraduallStopBGM(float stopTime)
    {
        Instance.StartCoroutine(Instance.LowerVolume(stopTime));
    }

    /// <summary>
    /// SEを停止する処理
    /// </summary>
    public void StopSE()
    {
        foreach (var s in Instance._seAudioSourceList)
        {
            s.Stop();
            s.clip = null;
        }
    }

    /// <summary>
    /// マスターボリュームの変更
    /// </summary>
    /// <param name="masterValue">音量</param>
    public void MasterVolChange(float masterValue)
    {
        Instance._masterVolume = masterValue;
        Instance._bgmSource.volume = Instance._bgmVolume * Instance._masterVolume;
    }

    /// <summary>
    /// ミキサーの特定のボリュームを変更
    /// </summary>
    /// <param name="audioType">ミキサー内のタイプ</param>
    /// <param name="recieveVolume">変更後のボリューム</param>
    public void VolumeChange(string audioType, float recieveVolume)
    {
        var volume = Mathf.Clamp(Mathf.Log10(Mathf.Clamp(recieveVolume, 0f, 1f)) * 20f, -80f, 0f);
        Instance._mixer.SetFloat(audioType, volume);
    }

    /// <summary>
    /// BGMの取得
    /// </summary>
    /// <param name="type">BGMの種類</param>
    /// <returns>BGM</returns>
    private BGM GetBGM(BGMType type)
    {
        var bgm = Instance._bgmList.FirstOrDefault(b => b.BGMType == type);
        return bgm;
    }

    /// <summary>
    /// SEの取得
    /// </summary>
    /// <param name="type">SEの種類</param>
    /// <returns>SE</returns>
    private SE GetSE(SEType type)
    {
        var se = Instance._seList.FirstOrDefault(s => s.SEType == type);
        return se;
    }

    /// <summary>
    /// 再生中のBGMがあればBGMを変更する処理
    /// </summary>
    /// <param name="afterBgm">次のBGM</param>
    /// <param name="loopType">ループするかしないか</param>
    IEnumerator SwitchingBgm(BGM afterBgm, bool loopType = true)
    {
        _isStoping = false;
        float currentVol = _bgmSource.volume;

        while (_bgmSource.volume > 0)
        {
            _bgmSource.volume -= Time.deltaTime * 1.5f;
            yield return null;
        }

        _bgmSource.clip = afterBgm.Clip;
        _bgmSource.loop = loopType;
        _bgmSource.Play();

        while (_bgmSource.volume < currentVol)
        {
            _bgmSource.volume += Time.deltaTime * 1.5f;
            yield return null;
        }
        _bgmSource.volume = currentVol;
    }

    /// <summary>
    /// ボリュームを徐々に下げる処理
    /// </summary>
    /// <param name="time">下げるために要する時間</param>
    IEnumerator LowerVolume(float time)
    {
        float currentVol = _bgmSource.volume;
        _isStoping = true;

        while (_bgmSource.volume > 0)
        {
            _bgmSource.volume -= Time.deltaTime * currentVol / time;

            if (!_isStoping)
            {
                yield break;
            }
            yield return null;
        }

        _isStoping = false;
        Instance._bgmSource.Stop();
        Instance._bgmSource.clip = null;
    }

}
