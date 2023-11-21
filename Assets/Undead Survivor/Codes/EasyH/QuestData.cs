using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffData", menuName = "Scriptable Object/QuestData")]
public class QuestData : ScriptableObject
{
    public enum QuestType { 
        killQuiz, killHW, killTest, walk, safeTime, survive
    }

    public QuestType Type;
    public string Name;

    public int IntValue;
    public float FloatValue;

}
