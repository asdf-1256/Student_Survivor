using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

[CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Object/SkillData")]
public class SkillData : ScriptableObject
{
    public enum SkillType { 전공, 교양 }

    [Header("# Main Info")]
    public SkillType skillType;
    public int skillID;
    public int bulletPrefabID; // Bullet의 프리펩 ID
    public int level; // 0~2까지
    public string skillName;
    public int grade; // 전공인 경우 학년
    [TextArea]
    public string skillDesc;
    public Sprite skillIcon;

    [Header("# Major Subject Skill Info")]
    public GameObject bulletPrefab;
    public int pool_index;

    public enum GEType { Speed, Size, MagnetSize, Attack, Defense, EXP, MaxHealth, Recovery, AttackCoolDownReduction, SpawnCoolDownReduction }

    [Header("# General Elective Subject Skill Info ")]
    public GEType getype;

    [Header("# Level Data")]
    public float[] cooltimes;
    public float[] lifeTimes;
    public float[] damages;
    public float[] speeds;
    public float[] counts; // 한 번에 나가는 총알 개수? 일단 파이썬이 사용할까
    public float attackCoolTime; // 클라우드, IoT : 소환체가 공격하는 쿨타임
    public float scaleFactor; // 컴네실 : 에서 파동의 크기 증가 속도
    public float flightTime; // 자바 : 체공 시간 = 컵이 날아가 커피로 변하기 까지의 시간
    public float rotateSpeed; // 자바 : Bullet이 회전하는 속도


}
