using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;



public class Mixer_Sound : MonoBehaviour
{
    public AudioMixer mixer;


    [Range(-80, 0)]
    public float Master = 0;

    [Range(-80, 0)]
    public float Bgm = 0;

    [Range(-80, 0)]
    public float Sfx = 0;

    public void MixerControl()
    {
        mixer.SetFloat(nameof(Master), Master);
        mixer.SetFloat(nameof(Bgm), Bgm);
        mixer.SetFloat(nameof(Sfx), Sfx);
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        MixerControl();
    }
}
