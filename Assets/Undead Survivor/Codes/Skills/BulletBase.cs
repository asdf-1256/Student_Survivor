using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    public float lifeTime;
    public float damage;
    public float speed;
    public float count;
    public float attackCoolTime; // 클라우드, IoT : 소환체가 공격하는 쿨타임
    public float scaleFactor; // 컴네실 : 에서 파동의 크기 증가 속도
    public float flightTime; // 자바 : 체공 시간 = 컵이 날아가 커피로 변하기 까지의 시간
    public float rotateSpeed; // 자바 : Bullet 오브젝트가 회전하는 속도

    public Transform playerTransform;

    public virtual void Init(bool isAI, SkillData skillData, int level)
    {
        lifeTime = skillData.lifeTimes[level];
        damage = skillData.damages[level];
        speed = skillData.speeds[level];
        count = skillData.counts[level];
        attackCoolTime = skillData.attackCoolTime;
        scaleFactor = skillData.scaleFactor;
        flightTime = skillData.flightTime;
        rotateSpeed = skillData.rotateSpeed;

        if (isAI)
        {
            playerTransform = GameManager.Instance.ai_Player.transform;
        }
        else
        {
            playerTransform = GameManager.Instance.player.transform;
        }
    }

    public float getDamage()
    {
        return damage;
    }
    public void putDamage(float damage)
    {
        this.damage = damage;
    }
}
