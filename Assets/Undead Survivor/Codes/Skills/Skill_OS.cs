using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_OS : MonoBehaviour
{
    public int bulletPrefabID;
    public float coolTime;
    public float damage;
    public float speed;
    public float lifeTime; // 폭발하는 시간?

    public GameObject Bullet; // 총알이 어떤 프리팹인지 보여주기만 하는 용도

    Player player;
    float timer;
    private void Awake()
    {
        player = GameManager.Instance.player;

        A_Skill_Data skillData = GetComponentInParent<A_Skill_Data>();
        bulletPrefabID = skillData.bulletPrefabID;
        coolTime = skillData.coolTime;
        damage = skillData.damage;
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

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;//방향 구하기

        Transform bullet = GameManager.Instance.pool.Get(bulletPrefabID).transform;

        bullet.position = transform.position + dir;
        // bullet.rotation = Quaternion.FromToRotation(Vector3.left, dir); // 회전 안 함
        bullet.GetComponent<Bullet_OS>().Init(damage, speed, lifeTime);


        // 오디오
    }
}
