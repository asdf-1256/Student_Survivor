using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private static DataManager instance = null;
    public int money = 0; //돈
    public int selectedCharacterId = 0; //현재 선택된 캐릭터
    public bool[] isUnlockCharacters = new bool[4]; //캐릭터가 해금된 상태인지 여부
    [SerializeField] public float bgmVolume;
    [SerializeField] public float sfxVolume;
    public static DataManager Instance //이 클래스의 instance를 받아올 수 있는 속성 - 게임매니저쓸 때랑 똑같이 쓰면 됨.
    {
        get { return instance; }
    }
    private void Awake()
    {
        if (instance == null) //지금까지 생성된 객체가 없으면
            instance = this; //이걸 static으로 쓴다.
        else if (instance != this) //생성된 객체가 있는데 그게 이 객체의 instance가 아니면
            Destroy(gameObject); //지금 객체는 없앤다.
        
        Init(); //초기화를 수행한다.
        DontDestroyOnLoad(gameObject);//게임을 재시작하여 LoadScene이 호출되어도 이 오브젝트는 파괴되지 않는다.
    }
    private void Init()//초기화 함수 - 기기에 저장된 데이터를 불러와 멤버 변수들을 초기화한다.
    {
        money = PlayerPrefs.GetInt("money", 0); 
        selectedCharacterId = PlayerPrefs.GetInt("selectedCharacterId", 0); 
        for (int i = 0; i < isUnlockCharacters.Length; i++) {
            isUnlockCharacters[i] = Convert.ToBoolean(PlayerPrefs.GetInt(string.Format("isUnlockCharacter{0}", i), 0));
        }
        bgmVolume = PlayerPrefs.GetFloat("bgmVolume", 0.3f);
        sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 0.5f);
        //(key, key에 해당하는 값이 없을 때 가져올 default value) 값이 기기에 있으면 그 값이 반환된다.
    }
    public void SetSelectedCharacter(int id)//캐릭터 선택 버튼에 연결되는 메소드
    {
        selectedCharacterId = id;
        Save();
    }
    public void Save()//기기에 저장하는 메소드
    {
        //PlayerPrefs.SetInt("UserData", 1);

        PlayerPrefs.SetInt("money", money);
        PlayerPrefs.SetInt("selectedCharacterId", selectedCharacterId);
        for (int i = 0; i < isUnlockCharacters.Length; i++)
        {
            PlayerPrefs.SetInt(string.Format("isUnlockCharacter{0}", i), Convert.ToInt32(isUnlockCharacters[i]));
        }

        bgmVolume = AudioManager.Instance.bgmVolume;//현재 수정된 볼륨을 받아와서
        sfxVolume = AudioManager.Instance.sfxVolume;

        PlayerPrefs.SetFloat("bgmVolume", bgmVolume);//저장한다.
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
    }

    //UnlockCharacter, CheckMoney 는 아직 테스트 못 함. 미래를 위해 만들어둔 메소드

    public void UnlockCharacter(int id, int price)//캐릭터 해금 메소드
    {
        if(!CheckMoney(price))//돈 양을 검사한다.
            return;

        SubMoney(price);//돈을 뺀다
        isUnlockCharacters[id] = true;//해금한다.

        Save();//저장한다.
    }
    public bool CheckMoney(int price)//돈이 모자라지 않는지 검사하는 메소드. return value = if 돈 충분 - true, else 돈 안 충분 - false. 
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
