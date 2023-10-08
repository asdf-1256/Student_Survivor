using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Grapics : MonoBehaviour
{
    public int bulletPrefabID;
    public float coolTime;
    public float lifeTime;

    public GameObject Bullet; // 총알이 어떤 프리팹인지 보여주기만 하는 용도


    Player player;
    float timer;
    private void Awake()
    {
        player = GameManager.Instance.player;

        A_Skill_Data skillData = GetComponentInParent<A_Skill_Data>();
        bulletPrefabID = skillData.bulletPrefabID;
        coolTime = skillData.coolTime;
        lifeTime = skillData.lifeTime;
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
        if (!player.scanner.nearestTarget)
            return;

        Vector2 randomCircle = Random.insideUnitCircle; // 원 내의 한 점
        Vector3 spawnPosition = new Vector3(randomCircle.x, randomCircle.y, 0);
        

        Transform bullet = GameManager.Instance.pool.Get(bulletPrefabID).transform;

        bullet.position = transform.position + spawnPosition * 10; // 캐릭터 중심으로 반지름 10인 원 내의 한 점
        bullet.rotation = Random.rotation; // 랜덤 회전
        bullet.GetComponent<Bullet_Grapics>().Init(lifeTime);


        // 오디오
    }
}
