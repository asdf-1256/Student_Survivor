using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasedSkill_Park : MonoBehaviour
{
    [SerializeField] private float cooltime;
    [SerializeField] private float timer;
    [SerializeField] private GameObject bulletPark;

    private void Awake()
    {
        timer = 0f;
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > cooltime)
            Fire();
    }
    private void Fire()
    {
        bulletPark.SetActive(true);
    }
}
