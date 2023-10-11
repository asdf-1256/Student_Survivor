using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private static DataManager instance;

    [SerializeField]
    private int money = 0;

    //이후 저장되어야할 변수 추가
    public static DataManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<DataManager>();

            if (instance == null)
                Debug.LogError("singleton error");
            return instance;
        }
    }

    public int Money
    {
        get { return money; }
        set { money = value; }
    }
    private void Awake()
    {
        if (instance == null)
            instance = this;

        else if(instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }


    //이후 PlayerPref에 저장하는 코드를 추가
    //이후 데이터를 게임 내에서 증가, 감소 시키는 함수를 추가
}
