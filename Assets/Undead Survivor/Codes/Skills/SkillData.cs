using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

[CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Object/SkillData")]
public class SkillData : ScriptableObject
{
    //public enum SkillType { 교양, 전공 }//으음ㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁ............??????????????????????????????????어렵
    public int id;
    public int pool_index;
    public float[] damages;
    //public float[] damages_rate;
    public float[] cooltimes;


}
