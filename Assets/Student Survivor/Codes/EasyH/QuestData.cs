using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffData", menuName = "Scriptable Object/QuestData")]
public class QuestData : ScriptableObject
{
    public enum QuestType { 
        killHW, killQuiz, killTest, walk, safeTime, survive, getDamage
    }

    public QuestType Type;
    public string Name;

    public int[] IntValues;
    public float[] FloatValues;

}
