using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using static SkillData;

[CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Object/SkillData")]
public class SkillData : ScriptableObject
{
    public enum SkillType { 교양, 전공 } //으음ㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁ............??????????????????????????????????어렵
    public int id;
    public Sprite icons;

    public string skillName;
    [TextArea]
    public string skillDesc;
    public int poolIndex;
    public float[] damages;
    public float[] cooltimes;

    [Header("IoT")]
    public float[] summonTime;

    
    public enum 교양능력치 { MaxHp, Magnet };
    [Header("교양")]
    public 교양능력치 능력치;


}
