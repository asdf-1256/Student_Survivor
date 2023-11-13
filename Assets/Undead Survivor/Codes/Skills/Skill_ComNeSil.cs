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


        Transform bullet = GameManager.Instance.pool.Get(4).transform;


        // ¿Àµð¿À
    }
    
}
