using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface QuestReward {
    public void Reward();
}

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    class QuestInfor {
        internal string skillName;
        internal QuestChecker Checker;
        internal QuestReward Reward;
        internal UIQuest UI;
    }

    public List<string> _doingQuestName;
    List<QuestInfor> _doingQuest;

    public void AddQuest(string skillName, int level, QuestData newQuest, QuestReward reward) {

        _doingQuestName.Add(skillName);

        QuestInfor newQuestInfor = new QuestInfor();
        newQuestInfor.skillName = skillName;
        newQuestInfor.Reward = reward;

        switch (newQuest.Type) {
            case QuestData.QuestType.killQuiz:
                newQuestInfor.Checker = new KillCountQuestChecker(newQuest.IntValue);
                break;
            case QuestData.QuestType.killHW:
                newQuestInfor.Checker = new KillCountQuestChecker(newQuest.IntValue);
                break;
            case QuestData.QuestType.killTest:
                newQuestInfor.Checker = new KillCountQuestChecker(newQuest.IntValue);
                break;
            case QuestData.QuestType.walk:
                newQuestInfor.Checker = new WalkQuestChecker(newQuest.FloatValue);
                break;
            case QuestData.QuestType.safeTime:
                newQuestInfor.Checker = new SafeTimeQuestChecker(newQuest.FloatValue);
                break;
            default:
                newQuestInfor.Checker = new HealthMakeToQuestChecker(newQuest.FloatValue);
                break;
        }

        newQuestInfor.UI = GameManager.Instance.AddQuest(skillName, level, newQuestInfor.Checker, newQuest);

        _doingQuest.Add(newQuestInfor);

    }

    private void _QuestAchieve(QuestInfor infor)
    {
        _doingQuestName.Remove(infor.skillName);
        _doingQuest.Remove(infor);
        UIManager.Instance.Notice(string.Format("Äù½ºÆ® ¼º°ø"));
        SkillTreeManager.instance.AddSkillLevelPair(infor.skillName);

        GameManager.Instance.EndQuest(infor.UI);
        infor.Reward.Reward();

    }

    public bool IsQuestDoing(string name) {
        return _doingQuestName.Contains(name);
    }

    private void Awake()
    {
        Instance = this;
        _doingQuestName = new List<string>();
        _doingQuest = new List<QuestInfor>();

    }

    // Update is called once per frame
    void Update()
    {
        if (_doingQuest.Count == 0) return;

        List<QuestInfor> achieveQuest = new List<QuestInfor>();

        foreach (QuestInfor quest in _doingQuest) {
            if (!quest.Checker.CheckAchieve()) continue;

            achieveQuest.Add(quest);
        }

        foreach (QuestInfor quest in achieveQuest) {
            _QuestAchieve(quest);
        }
    }
}
