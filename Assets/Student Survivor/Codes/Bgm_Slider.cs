using UnityEngine;
using UnityEngine.UI;

public class Bgm_Slider : MonoBehaviour
{
    public Slider slider;
    public AudioManager audioManager;
    public DataManager dataManager;

    void Start()
    {
        // �����̴��� �ʱⰪ�� �����մϴ�.(�����͸޴����� ������ ������)
        slider.value = dataManager.bgmVolume;

        // �����̴��� ���� ����� ������, ChangeBgmVolume �޼��尡 ȣ��ǵ��� �����մϴ�.
        slider.onValueChanged.AddListener(ChangeBgmVolume);
    }

    void ChangeBgmVolume(float value)
    {
        // �����̴��� ���ο� ���� AudioManager�� �������� �����մϴ�.
        audioManager.bgmVolume = value;
    }
}