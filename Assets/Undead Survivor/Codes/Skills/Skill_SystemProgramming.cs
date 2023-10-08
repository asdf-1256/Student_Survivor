using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_SystemProgramming : MonoBehaviour
{
    public float cooltime = 5f;
    public float timer = 5f;
    public float speed = 7f;

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = cooltime;
            Fire();
        }
    }
    void Fire()
    {
        Transform target = GameManager.Instance.player.scanner.nearestTarget;
        if (target == null)
        {
            return;
        }
        Transform player = GameManager.Instance.player.transform;

        Transform bullet = GameManager.Instance.pool.Get(15).transform;
        bullet.position = player.position;

        Vector3 dir = target.position - player.position;
        transform.LookAt(dir);

        bullet.GetComponent<Rigidbody2D>().velocity = dir.normalized * speed;
    }

}