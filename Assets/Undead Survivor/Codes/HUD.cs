using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//Text 넣을 때 알아서 추가됨

public class HUD : MonoBehaviour
{
    public enum InfoType { Exp, Level, NextPhase, Kill, Time, Health, BossHealth, Coin }
    public InfoType type;

    string[] Phases = { "1학년 1학기", "1학년 2학기", "2학년 1학기", "2학년 2학기", "3학년 1학기", "3학년 2학기", "4학년 1학기", "4학년 2학기", "5학년 1학기" };
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
                myText.text = string.Format("졸업 학점 : {0:F0}", GameManager.Instance.level);
                break;
            case InfoType.NextPhase:
                myText.text = string.Format(Phases[GameManager.Instance.currentPhase] + 
                    "\n다음 페이즈까지 {0:F0} / {1:F0}", 
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
