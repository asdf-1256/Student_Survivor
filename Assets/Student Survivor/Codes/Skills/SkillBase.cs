using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase : MonoBehaviour //���߿� �̸� �ٲٱ�.
{
    public SkillData data;
    int level;
    public IEnumerator TimerRoutine(float duration, System.Action done)
    {
        float timer = 0f;

        while (timer < duration) { yield return null; }
        
        done.Invoke();
    }

    public void Init(int level) 
    {
        this.level = level;
    }
    public int GetLevel()
    {
        return level;
    }
    public void SetLevel(int level) 
    { 
        this.level = level;
    }
}


