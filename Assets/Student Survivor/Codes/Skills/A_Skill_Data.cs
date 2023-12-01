using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Skill_Data : MonoBehaviour
{
    public int bulletPrefabID; // Bullet의 프리펩 ID
    public int level; // 0~2까지
    public float coolTime;
    public float lifeTime;
    public float damage;
    public float speed;
    public float scaleFactor; // 컴네실 : 에서 파동의 크기 증가 속도
    public float flightTime; // 자바 : 체공 시간 = 컵이 날아가 커피로 변하기 까지의 시간
    public float rotateSpeed; // 자바 : 회전하는 속도
    public float attackCoolTime; // 클라우드 : 소환체가 공격하는 쿨타임
}
