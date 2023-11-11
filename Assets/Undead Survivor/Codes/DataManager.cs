using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private static DataManager instance = null;
    public int money = 0; //돈
    public int selectedCharacterId = 0; //선택된 캐릭터
    public bool[] isUnlockCharacters = new bool[4]; //캐릭터가 해금된 상태인지 여부
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
    private void Init()
    {
        if (!PlayerPrefs.HasKey("UserData"))
            Save();
        money = PlayerPrefs.GetInt("money"); 
        selectedCharacterId = PlayerPrefs.GetInt("selectedCharacterId"); 
        for (int i = 0; i < isUnlockCharacters.Length; i++) {
            isUnlockCharacters[i] = Convert.ToBoolean(PlayerPrefs.GetInt(string.Format("isUnlockCharacter{0}", i)));
        }
    }
    public void SetSelectedCharacter(int id)//캐릭터 선택 버튼에 연결되는 메소드
    {
        selectedCharacterId = id;
        Save();
    }
    public void Save()//기기에 저장하는 메소드
    {
        PlayerPrefs.SetInt("UserData", 1);

        PlayerPrefs.SetInt("money", money);
        PlayerPrefs.SetInt("selectedCharacterId", selectedCharacterId);
        for (int i = 0; i < isUnlockCharacters.Length; i++)
        {
            PlayerPrefs.SetInt(string.Format("isUnlockCharacter{0}", i), Convert.ToInt32(isUnlockCharacters[i]));
        }
    }
    public void UnlockCharacter(int id, int price)//캐릭터 해금 함수.
    {
        if(!CheckMoney(price))//돈 양을 검사한다.
            return;

        SubMoney(price);//돈을 뺀다
        isUnlockCharacters[id] = true;//해금한다.

        Save();//저장한다.
    }
    public bool CheckMoney(int price)//돈이 모자라지 않는지 검사한다. 돈 충분 - true, 돈 안 충분 - false. 
    {
        return money > price;
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
