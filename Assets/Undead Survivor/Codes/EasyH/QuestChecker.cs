using UnityEngine;

public interface QuestChecker {
    public bool CheckAchieve();
}

public class SafeTimeQuestChecker : QuestChecker {

    float _goalTime;
    float _originHealth;
    float _spendTime;

    public SafeTimeQuestChecker(float time) {
        _goalTime = time;
        _originHealth = GameManager.Instance.health;
    }

    public bool CheckAchieve()
    {
        if (_originHealth != GameManager.Instance.health)
        {
            _originHealth = GameManager.Instance.health;
            _spendTime = 0;
            return false;
        }

        _spendTime += Time.deltaTime;

        if (_spendTime < _goalTime)
            return false;

        return true;
    }
}

public class HealthMakeToQuestChecker : QuestChecker {

    float _goalRatio;

    public HealthMakeToQuestChecker(float ratio)
    {
        _goalRatio = ratio;
    }

    public bool CheckAchieve()
    {
        if (GameManager.Instance.health / GameManager.Instance.maxHealth > _goalRatio)
            return false;

        return true;
    }
}

public class KillCountQuestChecker : QuestChecker {

    int _originKillCount;
    int _goalCount;

    public KillCountQuestChecker(int goalCount)
    {
        _originKillCount = GameManager.Instance.kill;
        _goalCount = goalCount;
    }

    public bool CheckAchieve()
    {
        if (GameManager.Instance.kill - _originKillCount < _goalCount) return false;

        return true;
    }
}

public class WalkQuestChecker : QuestChecker {
    float _walkAmount;
    public WalkQuestChecker(float walkAmount) { 
        _walkAmount = walkAmount;
    }

    public bool CheckAchieve() {
        return false;
    }

}