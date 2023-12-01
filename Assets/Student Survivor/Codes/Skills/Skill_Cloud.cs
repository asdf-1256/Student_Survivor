using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Cloud : MonoBehaviour
{
    public float coolTime; // lifeTime �� �Ǹ� �׶����� coolTime ī��Ʈ ��
    public float lifeTime; // lifeTime << coolTime ������ lifeTime�� coolTime�� ���Ե�
    public float damage;
    public float speed;
    public float dropCycleTime; // ������ ���ڱ�⸦ ����߸��� �ֱ�


    float timer;

    // ������ �ϴ� coolTime�� LifeTime�� ���Եǵ��� �����ϰ���.
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
        Transform bullet = GameManager.Instance.pool.Get(12).transform;

        // bullet.GetComponent<Bullet_Cloud>().Init(damage, speed, lifeTime, dropCycleTime);


        // �����
    }
}
