using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

[CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Object/SkillData")]
public class SkillData : ScriptableObject
{
    public enum SkillType { ����, ���� }

    [Header("# Main Info")]
    public SkillType skillType;
    public int skillID;
    public int bulletPrefabID; // Bullet�� ������ ID
    public int level; // 0~2����
    public string skillName;
    public int grade; // ������ ��� �г�
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
    public float[] counts; // �� ���� ������ �Ѿ� ����? �ϴ� ���̽��� ����ұ�
    public float attackCoolTime; // Ŭ����, IoT : ��ȯü�� �����ϴ� ��Ÿ��
    public float scaleFactor; // �ĳ׽� : ���� �ĵ��� ũ�� ���� �ӵ�
    public float flightTime; // �ڹ� : ü�� �ð� = ���� ���ư� Ŀ�Ƿ� ���ϱ� ������ �ð�
    public float rotateSpeed; // �ڹ� : Bullet�� ȸ���ϴ� �ӵ�


}
