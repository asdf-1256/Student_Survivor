using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private static DataManager instance = null;
    public int money = 0; //��
    public int selectedCharacterId = 0; //���� ���õ� ĳ����
    public bool[] isUnlockCharacters = new bool[4]; //ĳ���Ͱ� �رݵ� �������� ����
    [Header("��Ų ����")]
    public int selectedSkinID;
    public bool[] isUnLockedSkins;

    [SerializeField] public float bgmVolume;
    [SerializeField] public float sfxVolume;
    public static DataManager Instance //�� Ŭ������ instance�� �޾ƿ� �� �ִ� �Ӽ� - ���ӸŴ����� ���� �Ȱ��� ���� ��.
    {
        get { return instance; }
    }
    private void Awake()
    {
        if (instance == null) //���ݱ��� ������ ��ü�� ������
            instance = this; //�̰� static���� ����.
        else if (instance != this) //������ ��ü�� �ִµ� �װ� �� ��ü�� instance�� �ƴϸ�
            Destroy(gameObject); //���� ��ü�� ���ش�.
        
        Init(); //�ʱ�ȭ�� �����Ѵ�.
        DontDestroyOnLoad(gameObject);//������ ������Ͽ� LoadScene�� ȣ��Ǿ �� ������Ʈ�� �ı����� �ʴ´�.
    }
    private void Init()//�ʱ�ȭ �Լ� - ��⿡ ����� �����͸� �ҷ��� ��� �������� �ʱ�ȭ�Ѵ�.
    {
        money = PlayerPrefs.GetInt("money", 0); 
        selectedCharacterId = PlayerPrefs.GetInt("selectedCharacterId", 0); 
        for (int i = 0; i < isUnlockCharacters.Length; i++) {
            isUnlockCharacters[i] = Convert.ToBoolean(PlayerPrefs.GetInt(string.Format("isUnlockCharacter{0}", i), 0));
        }
        bgmVolume = PlayerPrefs.GetFloat("bgmVolume", 0.3f);
        sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 0.5f);
        //(key, key�� �ش��ϴ� ���� ���� �� ������ default value) ���� ��⿡ ������ �� ���� ��ȯ�ȴ�.

        selectedSkinID = PlayerPrefs.GetInt("selectedSkinID", 0);
        for (int i = 0; i < isUnLockedSkins.Length; i++)
        {
            isUnLockedSkins[i] = Convert.ToBoolean(PlayerPrefs.GetInt(string.Format("isUnLockedSkins{0}", i), 0));
        }
    }
    public void SetSelectedCharacter(int id)//ĳ���� ���� ��ư�� ����Ǵ� �޼ҵ�
    {
        selectedCharacterId = id;
        Save();
    }
    public void SetSelectedSkinID(int id)//ĳ���� ���� ��ư�� ����Ǵ� �޼ҵ�
    {
        selectedSkinID = id;
        Save();
    }
    public void Save()//��⿡ �����ϴ� �޼ҵ�
    {
        //PlayerPrefs.SetInt("UserData", 1);

        PlayerPrefs.SetInt("money", money);
        PlayerPrefs.SetInt("selectedCharacterId", selectedCharacterId);
        for (int i = 0; i < isUnlockCharacters.Length; i++)
        {
            PlayerPrefs.SetInt(string.Format("isUnlockCharacter{0}", i), Convert.ToInt32(isUnlockCharacters[i]));
        }

        PlayerPrefs.SetInt("selectedSkinID", selectedSkinID);
        for (int i = 0; i < isUnLockedSkins.Length; i++)
        {
            PlayerPrefs.SetInt(string.Format("isUnLockedSkins{0}", i), Convert.ToInt32(isUnLockedSkins[i]));
        }

        bgmVolume = AudioManager.Instance.bgmVolume;//���� ������ ������ �޾ƿͼ�
        sfxVolume = AudioManager.Instance.sfxVolume;

        PlayerPrefs.SetFloat("bgmVolume", bgmVolume);//�����Ѵ�.
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
    }

    //UnlockCharacter, CheckMoney �� ���� �׽�Ʈ �� ��. �̷��� ���� ������ �޼ҵ�

    public void UnlockCharacter(int id, int price)//ĳ���� �ر� �޼ҵ�
    {
        if(!CheckMoney(price))//�� ���� �˻��Ѵ�.
            return;

        SubMoney(price);//���� ����
        isUnlockCharacters[id] = true;//�ر��Ѵ�.

        Save();//�����Ѵ�.
    }
    public void UnlockSkin(int id)//ĳ���� �ر� �޼ҵ�
    {
        isUnLockedSkins[id] = true;//�ر��Ѵ�.

        Save();//�����Ѵ�.
    }
    public bool CheckMoney(int price)//���� ���ڶ��� �ʴ��� �˻��ϴ� �޼ҵ�. return value = if �� ��� - true, else �� �� ��� - false. 
    {
        return money >= price;
    }

    public void AddMoney(int value)
    {
        money += value;
    }
    public void SubMoney(int value)
    {
        money -= value;
    }
}
