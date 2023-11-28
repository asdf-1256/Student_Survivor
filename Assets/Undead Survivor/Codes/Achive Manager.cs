using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchiveManager : MonoBehaviour
{
    public GameObject[] lockCharacter;
    public GameObject[] unlockCharacter;
    public GameObject uiNotice;

    enum Achive { UnlockPotato, UnlockBean }
    Achive[] achives;
    WaitForSecondsRealtime wait;//최적화를 위해 따로 선언
    //timescale의 영향을 받는다?

    private void Awake()
    {
        achives = (Achive[])Enum.GetValues(typeof(Achive));
        wait = new WaitForSecondsRealtime(5);

        //플레이한 후 게임을 끄면 보존되도록
        if (!PlayerPrefs.HasKey("MyData"))
        {
            Init();
        }
        //edit - clear all playerpref 하면 데이터 지워짐
    }
    void Init()
    {
        //저장하는 함수
        PlayerPrefs.SetInt("MyData", 1);
        foreach (Achive achive in achives)
        {
            PlayerPrefs.SetInt(achive.ToString(), 0);
        }

        //PlayerPrefs.SetInt("UnlockPotato", 0);
        //PlayerPrefs.SetInt("UnlockBean", 0);
    }
    private void Start()
    {
        UnlockCharacter();
    }
    void UnlockCharacter()
    {
        for (int index = 0 ; index < lockCharacter.Length; index++)
        {
            string achiveName = achives[index].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;
            lockCharacter[index].SetActive(!isUnlock);
            unlockCharacter[index].SetActive(isUnlock);
        }
    }
    private void LateUpdate()
    {
        foreach(Achive achive in achives)
        {
            CheckAchive(achive);
        }
    }
    void CheckAchive(Achive achive)
    {
        bool isAchive = false;

        switch(achive)
        {
            case Achive.UnlockPotato:
                isAchive = GameManager.Instance.kill >= 10;
                break;
            case Achive.UnlockBean:
                //isAchive = GameManager.Instance.gameTime == GameManager.Instance.maxGameTime;
                break;
        }

        if (isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0)
        {
            PlayerPrefs.SetInt(achive.ToString(), 1);

            for (int index = 0 ; index < uiNotice.transform.childCount ; index++)
            {
                bool isActive = index == (int)achive;
                uiNotice.transform.GetChild(index).gameObject.SetActive(isActive);
            }

            StartCoroutine(NoticeRoutine());


        }
    }

    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.LevelUp);

        yield return wait;

        uiNotice.SetActive(false);
    }
}
