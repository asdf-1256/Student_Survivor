using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Microprocessor : MonoBehaviour
{
    public float cooltime = 5f;
    public float timer = 5f;

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
        GameManager.Instance.pool.Get(14);
    }
}
