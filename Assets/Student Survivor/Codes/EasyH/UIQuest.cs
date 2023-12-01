using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuest : MonoBehaviour
{

    public Text SkillName;
    public Text SkillLevel;
    public Text QuestName;
    public Text QuestProgress;
    public Image QuestProgressBar;

    QuestChecker checker;

    // Start is called before the first frame update
    public void QuestSet(string skillName, int level, QuestChecker checker, QuestData data)
    {
        this.checker = checker;

        SkillName.text = skillName;
        SkillLevel.text = "예상 학점 : " + levelToGrade(level);
        QuestName.text = data.Name;
        QuestProgress.text = checker.ToString();
        QuestProgressBar.fillAmount = checker.GetProgress();
        
        gameObject.SetActive(true);
    }
    string levelToGrade(int level)
    {
        switch (level)
        {
            case 0:
                return "C+";
            case 1:
                return "B ";
            case 2:
                return "B+";
            case 3:
                return "A ";
            case 4:
                return "A+";
            default:
                return "error!!";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (checker == null) return;

        QuestProgress.text = checker.ToString();
        QuestProgressBar.fillAmount = checker.GetProgress();

    }
}
