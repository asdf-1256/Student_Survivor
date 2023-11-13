using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_IoT : MonoBehaviour
{
    public float coolTime; // lifeTime 다 되면 그때부터 coolTime 카운트 함
    public float lifeTime; // lifeTime << coolTime 지금은 lifeTime이 coolTime에 포함됨
    public float damage;
    public float speed;
    public float attackCoolTime; // 드론의 공격속도
    public float movePosTime;


    float timer;

    // 지금은 일단 coolTime에 LifeTime이 포함되도록 설계하겠음.
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

        Transform bullet = GameManager.Instance.pool.Get(13).transform;
        // bullet.GetComponent<Bullet_IoT>().Init(damage, speed, lifeTime, attackCoolTime, movePosTime);


        // 오디오
    }
}
