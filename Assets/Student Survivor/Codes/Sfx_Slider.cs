using UnityEngine;
using UnityEngine.UI;

public class Sfx_Slider : MonoBehaviour
{
    public Slider slider;
    public AudioManager audioManager;
    public DataManager dataManager;

    void Start()
    {
        // �����̴��� �ʱⰪ�� �����մϴ�.(�����͸޴����� ������ ������)
        slider.value = dataManager.sfxVolume;

        // �����̴��� ���� ����� ������, ChangeBgmVolume �޼��尡 ȣ��ǵ��� �����մϴ�.
        slider.onValueChanged.AddListener(ChangeSfxVolume);
    }

    void ChangeSfxVolume(float value)
    {
        // �����̴��� ���ο� ���� AudioManager�� �������� �����մϴ�.
        audioManager.sfxVolume = value;
    }
}