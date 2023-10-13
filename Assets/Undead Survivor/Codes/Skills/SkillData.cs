using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

[CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Object/SkillData")]
public class SkillData : ScriptableObject
{
    public enum SkillType { 전공, 교양 }

    public SkillType skillType;
    public int skillID;
    public int bulletPrefabID; // Bullet의 프리펩 ID
    public int level; // 0~2까지
    public string skillName;
    [TextArea]
    public string skillDesc;
    public Sprite skillIcon;

    public int pool_index;



    [Header("# Level Data")]
    public float[] cooltimes;
    public float[] lifeTime;
    public float[] damages;
    public float[] speed;
    public float attackCoolTime; // 클라우드, IoT : 소환체가 공격하는 쿨타임
    public float scaleFactor; // 컴네실 : 에서 파동의 크기 증가 속도
    public float flightTime; // 자바 : 체공 시간 = 컵이 날아가 커피로 변하기 까지의 시간
    public float rotateSpeed; // 자바 : Bullet이 회전하는 속도


}
