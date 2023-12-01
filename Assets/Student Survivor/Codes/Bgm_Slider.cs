using UnityEngine;
using UnityEngine.UI;

public class Bgm_Slider : MonoBehaviour
{
    public Slider slider;
    public AudioManager audioManager;
    public DataManager dataManager;

    void Start()
    {
        // 슬라이더의 초기값을 설정합니다.(데이터메니저의 음향을 가져옴)
        slider.value = dataManager.bgmVolume;

        // 슬라이더의 값이 변경될 때마다, ChangeBgmVolume 메서드가 호출되도록 설정합니다.
        slider.onValueChanged.AddListener(ChangeBgmVolume);
    }

    void ChangeBgmVolume(float value)
    {
        // 슬라이더의 새로운 값을 AudioManager의 볼륨으로 설정합니다.
        audioManager.bgmVolume = value;
    }
}