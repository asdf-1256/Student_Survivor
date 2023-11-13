using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Algorithm : MonoBehaviour
{
    // RB 트리를 생성 후 쓰러트리기? 트리는 생성되면 1초 후 좌/우로 90도 쓰러지게


    public float coolTime = 5f;
    float timer;
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
        Transform bullet = GameManager.Instance.pool.Get(6).transform;

    }

}
