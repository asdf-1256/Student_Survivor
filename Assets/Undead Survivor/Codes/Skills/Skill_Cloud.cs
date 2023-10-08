using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Cloud : MonoBehaviour
{
    public int bulletPrefabID;
    public float coolTime; // lifeTime 다 되면 그때부터 coolTime 카운트 함
    public float lifeTime; // lifeTime << coolTime 지금은 lifeTime이 coolTime에 포함됨
    public float damage;
    public float speed;
    public float attackCoolTime; // 구름이 전자기기를 떨어뜨리는 주기

    public GameObject Bullet;

    float timer;

    // 지금은 일단 coolTime에 LifeTime이 포함되도록 설계하겠음.

    private void Awake()
    {
        A_Skill_Data skillData = GetComponentInParent<A_Skill_Data>();
        bulletPrefabID = skillData.bulletPrefabID;
        coolTime = skillData.coolTime;
        lifeTime = skillData.lifeTime;
        damage = skillData.damage;
        speed = skillData.speed;
        attackCoolTime = skillData.attackCoolTime;
    }
    private void Update()
    {
        if (!GameManager.Instance.isLive)
            return;

        timer += Time.deltaTime;

        if (timer > coolTime)
        {
            timer = 0f;
            Fire();
        }
    }
    void Fire()
    {
        Vector2 randomCircle = Random.insideUnitCircle.normalized; // 원 위의 한 점
        Vector3 spawnPosition = new Vector3(randomCircle.x, randomCircle.y, 0);


        Transform bullet = GameManager.Instance.pool.Get(bulletPrefabID).transform;

        bullet.position = transform.position + spawnPosition * 5; // 캐릭터 중심으로 반지름 5인 원 위의 한 점
        bullet.GetComponent<Bullet_Cloud>().Init(damage, speed, lifeTime, attackCoolTime);


        // 오디오
    }
}
