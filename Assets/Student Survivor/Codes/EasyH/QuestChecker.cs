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
        if (GameManager.Instance.health < _originHealth)
        {
            _originHealth = GameManager.Instance.health;
            _spendTime = 0;
            return false;
        }
        _originHealth = GameManager.Instance.health;
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
    float _goalSpendHealth;
    float _spendHealth;
    public HealthMakeToQuestChecker(float ratio)
    {
        _goalRatio = ratio;
    }

    public bool CheckAchieve()
    {
        _goalSpendHealth = GameManager.Instance.maxHealth * (1 - _goalRatio);
        _spendHealth = GameManager.Instance.maxHealth - GameManager.Instance.health;
        if (_spendHealth < _goalSpendHealth)
            return false;

        return true;
    }

    public float GetProgress()
    {
        return (_spendHealth + 1) / _goalSpendHealth;

    }
    public override string ToString()
    {
        return _spendHealth.ToString("F0") + " / " + _goalSpendHealth.ToString("F0");

    }
}

public class GetDamageQuestChecker : QuestChecker
{

    float _goalDamage;
    float _currentHealth;
    float _totalDamage;
    public GetDamageQuestChecker(float damage)
    {
        _goalDamage = damage;
        _currentHealth = GameManager.Instance.health;
    }

    public bool CheckAchieve()
    {
        if (GameManager.Instance.health < _currentHealth)
        {
            _totalDamage += _currentHealth - GameManager.Instance.health;
        }
        _currentHealth = GameManager.Instance.health;

        if (_totalDamage < _goalDamage)
            return false;

        return true;
    }

    public float GetProgress()
    {
        return (_totalDamage + 1) / _goalDamage;

    }
    public override string ToString()
    {
        return _totalDamage.ToString("F0") + " / " + _goalDamage.ToString("F0");

    }
}

public class KillCountQuestChecker : QuestChecker {

    int _originKillCount;
    int _goalCount;
    int _killType;

    public KillCountQuestChecker(int killType, int goalCount)
    {
        _originKillCount = GameManager.Instance.killByType[killType];
        _goalCount = goalCount;
        _killType = killType;
    }

    public bool CheckAchieve()
    {
        if (GameManager.Instance.killByType[_killType] - _originKillCount < _goalCount) return false;

        return true;
    }
    public float GetProgress()
    {
        return (GameManager.Instance.killByType[_killType] - _originKillCount) / _goalCount;

    }
    public override string ToString()
    {
        return (GameManager.Instance.killByType[_killType] - _originKillCount).ToString() + " / " + _goalCount.ToString();
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
        return (GameManager.Instance.manBoGi - _originalWalk) / _golaWalk;
    }
    public override string ToString()
    {
        return (GameManager.Instance.manBoGi - _originalWalk).ToString("F0") + " / " + _golaWalk.ToString("F0");

    }
}