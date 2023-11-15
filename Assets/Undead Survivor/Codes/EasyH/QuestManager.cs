using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{

    QuestChecker _checker;
    Skill _achieveSkill;

    public void SetQuest(QuestData newQuest, Skill skill) {
        _achieveSkill = skill;
        switch (newQuest.Type) {
            case QuestData.QuestType.killQuiz:
                _checker = new KillCountQuestChecker(newQuest.IntValue);
                break;
            case QuestData.QuestType.killHW:
                _checker = new KillCountQuestChecker(newQuest.IntValue);
                break;
            case QuestData.QuestType.killTest:
                _checker = new KillCountQuestChecker(newQuest.IntValue);
                break;
            case QuestData.QuestType.walk:
                _checker = new WalkQuestChecker(newQuest.FloatValue);
                break;
            case QuestData.QuestType.safeTime:
                _checker = new SafeTimeQuestChecker(newQuest.FloatValue);
                break;
            default:
                _checker = new HealthMakeToQuestChecker(newQuest.FloatValue);
                break;
        }
    }

    public void QuestAchieve()
    {
        _achieveSkill.LevelUp();
        Debug.Log("Äù½ºÆ® ¼º°ø");

    }

    // Update is called once per frame
    void Update()
    {
        if (_checker == null) return;
        if (!_checker.CheckAchieve()) return;

        QuestAchieve();
        _checker = null;

    }
}
