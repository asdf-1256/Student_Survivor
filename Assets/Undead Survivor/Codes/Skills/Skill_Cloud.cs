using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Cloud : MonoBehaviour
{
    public float coolTime; // lifeTime 다 되면 그때부터 coolTime 카운트 함
    public float lifeTime; // lifeTime << coolTime 지금은 lifeTime이 coolTime에 포함됨
    public float damage;
    public float speed;
    public float dropCycleTime; // 구름이 전자기기를 떨어뜨리는 주기


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
        Vector2 randomCircle = Random.insideUnitCircle.normalized; // 원 위의 한 점
        Vector3 spawnPosition = new Vector3(randomCircle.x, randomCircle.y, 0);


        Transform bullet = GameManager.Instance.pool.Get(12).transform;

        bullet.position = transform.position + spawnPosition * 5; // 캐릭터 중심으로 반지름 5인 원 위의 한 점
        bullet.GetComponent<Bullet_Cloud>().Init(damage, speed, lifeTime, dropCycleTime);


        // 오디오
    }
}
