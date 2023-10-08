using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_OS : MonoBehaviour
{
    public float coolTime;
    public float damage;
    public float speed;

    Player player;
    float timer;
    private void Awake()
    {
        player = GameManager.Instance.player;

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

        Transform bullet = GameManager.Instance.pool.Get(7).transform;

        bullet.position = transform.position + dir;
        // bullet.rotation = Quaternion.FromToRotation(Vector3.left, dir); // 회전 안 함
        bullet.GetComponent<Bullet_OS>().Init(damage, speed);


        // 오디오
    }
}
