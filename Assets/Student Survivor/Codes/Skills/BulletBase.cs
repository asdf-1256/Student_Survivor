using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    public float lifeTime;
    public float damage;
    public float speed;
    public float count;
    public float attackCoolTime; // Ŭ����, IoT : ��ȯü�� �����ϴ� ��Ÿ��
    public float scaleFactor; // �ĳ׽� : ���� �ĵ��� ũ�� ���� �ӵ�
    public float flightTime; // �ڹ� : ü�� �ð� = ���� ���ư� Ŀ�Ƿ� ���ϱ� ������ �ð�
    public float rotateSpeed; // �ڹ� : Bullet ������Ʈ�� ȸ���ϴ� �ӵ�

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
