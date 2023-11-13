using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Grapics : MonoBehaviour
{
    public float coolTime;
    public float lifeTime;

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

        

        Transform bullet = GameManager.Instance.pool.Get(9).transform;

        // bullet.GetComponent<Bullet_Grapics>().Init(lifeTime);


        // ¿Àµð¿À
    }
}
