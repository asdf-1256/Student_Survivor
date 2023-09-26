using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface StatModifier
{
    void ModifyStat(string statName, float modifier);
}
public class Buff : MonoBehaviour
{
    [SerializeField]
    public string statName;
    public int buffId;
    public int buffLevel;
    public float value;
    public float duration;
    public float remainTime;

    void ApplyBuff(StatModifier obj, System.Action cbdone)
    {
        obj.ModifyStat("attack", 1.5f);
        StartCoroutine(buffRoutine(obj, () => { obj.ModifyStat("attack", 1 / 1.5f); }));
        
    }
    IEnumerator buffRoutine(StatModifier obj, System.Action cbdone)
    {
        yield return new WaitForSeconds(remainTime);
    }
    void ResetDuration()
    {
        remainTime = duration;
    }
}
