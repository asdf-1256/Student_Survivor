using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_ComNeSil : MonoBehaviour
{
    public float coolTime;


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

        if(timer > coolTime)
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

        Transform bullet = GameManager.Instance.pool.Get(4).transform;

        bullet.position = transform.position + dir;
        bullet.rotation = Quaternion.FromToRotation(Vector3.left, dir);//회전결정
        bullet.GetComponent<Bullet_ComNeSil>().Init(dir);


        // 오디오
    }
    
}
