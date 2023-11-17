using UnityEngine;

public interface QuestChecker {
    public bool CheckAchieve();
    public float GetProgress();
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

    public float GetProgress() {
        return _spendTime / _goalTime;
    
    }

    public override string ToString() { 
        return _spendTime.ToString("F0") + " / " + _goalTime.ToString("F0");
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

    public float GetProgress()
    {
        return GameManager.Instance.health / GameManager.Instance.maxHealth / _goalRatio;

    }
    public override string ToString()
    {
        if (CheckAchieve()) return "1 / 1";
        return "0 / 0";

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
    public float GetProgress()
    {
        return GameManager.Instance.kill - _originKillCount / _goalCount;

    }
    public override string ToString()
    {
        return (GameManager.Instance.kill - _originKillCount).ToString() + " / " + _goalCount.ToString();
    }
}

public class WalkQuestChecker : QuestChecker {
    float _golaWalk;
    float _originalWalk;
    public WalkQuestChecker(float walkAmount) { 
        _golaWalk = walkAmount;
        _originalWalk = GameManager.Instance.manBoGi;
    }
    
    public bool CheckAchieve() {
        if (GameManager.Instance.manBoGi - _originalWalk < _golaWalk) return false;
        return true;
    }

    public float GetProgress()
    {
        return GameManager.Instance.manBoGi - _originalWalk / _golaWalk;
    }
    public override string ToString()
    {
        return (GameManager.Instance.manBoGi - _originalWalk).ToString("F0") + " / " + _golaWalk.ToString("F0");

    }
}