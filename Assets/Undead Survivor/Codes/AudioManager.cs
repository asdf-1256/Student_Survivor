using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [Header("#BGM")]
    public AudioClip bgmClip;
    public AudioMixerGroup bgmOutput;
    public float bgmVolume;
    AudioSource bgmPlayer;
    AudioHighPassFilter bgmEffect;

    [Header("#MBGM")]
    [SerializeField] AudioSource mbgmPlayer;
    [SerializeField] public AudioClip mbgmClip;

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public AudioMixerGroup sfxOutput;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    public enum Sfx { Dead, Hit, LevelUp = 3, Lose, Melee, Range = 7, Select, Win }

    private void Awake()
    {
        Instance = this;
    }
    private void Start()//DataManager�� Instance�� ����� ���� ������ �� �ֵ��� �����ֱ� ���� Start �Լ����� ���� �ҷ�����.
    {
        bgmVolume = DataManager.Instance.bgmVolume;
        sfxVolume = DataManager.Instance.sfxVolume;
        Init();

        mbgmPlayer.Play();
    }
    void Init()
    {
        // ����� �÷��̾� �ʱ�ȭ
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;
        bgmPlayer.outputAudioMixerGroup = bgmOutput;
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        // ȿ���� �÷��̾� �ʱ�ȭ
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].bypassEffects = true;
            sfxPlayers[index].volume = sfxVolume;
            sfxPlayers[index].outputAudioMixerGroup = sfxOutput;
        }

    }

    public void PlaySfx(Sfx sfx)
    {
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)
                continue;

            int ranIndex = 0;
            if (sfx == Sfx.Hit || sfx == Sfx.Melee)
                ranIndex = Random.Range(0, 2);

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + ranIndex];
            sfxPlayers[loopIndex].Play();
            break;

        }

    }
    public void PlayBgm(bool isPlay)
    {
        if (isPlay)
        {
            mbgmPlayer.Stop();
            bgmPlayer.Play();
        }
        else
            bgmPlayer.Stop();
    }

    public void EffectBgm(bool isPlay)
    {
        bgmEffect.enabled = isPlay;
    }

    public float BgmVolume
    {
        get // �б�
        {
            return bgmPlayer.volume;
        }
        set // ����, set��ü�� ���� �ٲ۴ٴ� �ǹ�, value �ݵ�� ���(�ٸ��� ��� ����)
        {   
            if(bgmVolume != 0) 
            {
                bgmVolume = value;
                bgmPlayer.volume = value;
            }
        }
    }
}