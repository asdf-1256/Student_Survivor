using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using static SkillData;

[CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Object/SkillData")]
public class SkillData : ScriptableObject
{
    public enum SkillType { 嘘丞, 穿因 } //生製けけけけけけけけけけけ............??????????????????????????????????嬢憩
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

    
    public enum 嘘丞管径帖 { MaxHp, Magnet };
    [Header("嘘丞")]
    public 嘘丞管径帖 管径帖;


}
