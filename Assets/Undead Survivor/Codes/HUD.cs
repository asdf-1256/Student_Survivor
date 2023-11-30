using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//Text ���� �� �˾Ƽ� �߰���

public class HUD : MonoBehaviour
{
    public enum InfoType { Exp, Level, NextPhase, Kill, Time, Health, BossHealth, Coin }
    public InfoType type;

    string[] Phases = { "1�г� 1�б�", "1�г� 2�б�", "2�г� 1�б�", "2�г� 2�б�", "3�г� 1�б�", "3�г� 2�б�", "4�г� 1�б�", "4�г� 2�б�", "5�г� 1�б�" };
    Text myText;
    Slider mySlider;

    private void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    private void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Exp:
                float curExp = GameManager.Instance.exp;
                float maxExp = GameManager.Instance.nextExp[Mathf.Min(GameManager.Instance.level, GameManager.Instance.nextExp.Length-1)];
                mySlider.value = curExp / maxExp;
                break;
            case InfoType.Level:
                myText.text = string.Format("���� ���� : {0:F0}", GameManager.Instance.level);
                break;
            case InfoType.NextPhase:
                myText.text = string.Format(Phases[GameManager.Instance.currentPhase] + 
                    "\n���� ��������� {0:F0} / {1:F0}", 
                    GameManager.Instance.level, GameManager.Instance.levelPerPhase[GameManager.Instance.currentPhase]);
                mySlider.value = GameManager.Instance.getRateForNextPhase();
                break;
            case InfoType.Kill:
                myText.text = string.Format("{0:F0}", GameManager.Instance.kill);
                break;
            case InfoType.Time:
                float currentTime = GameManager.Instance.gameTime;
                int min = Mathf.FloorToInt(currentTime / 60);
                int sec = Mathf.FloorToInt(currentTime % 60);
                myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;
            case InfoType.Health:
                float curHealth = GameManager.Instance.health;
                float maxHealth = GameManager.Instance.maxHealth;
                mySlider.value = curHealth / maxHealth;
                break;
            case InfoType.BossHealth:
                mySlider.value = GameManager.Instance.SpawnedBoss.health / GameManager.Instance.SpawnedBoss.maxHealth;
                break;
            case InfoType.Coin:
                myText.text = DataManager.Instance.money.ToString();
                break;
        }
    }
}
