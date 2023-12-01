using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_IoT : MonoBehaviour
{
    public float coolTime; // lifeTime �� �Ǹ� �׶����� coolTime ī��Ʈ ��
    public float lifeTime; // lifeTime << coolTime ������ lifeTime�� coolTime�� ���Ե�
    public float damage;
    public float speed;
    public float attackCoolTime; // ����� ���ݼӵ�
    public float movePosTime;


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

        Transform bullet = GameManager.Instance.pool.Get(13).transform;
        // bullet.GetComponent<Bullet_IoT>().Init(damage, speed, lifeTime, attackCoolTime, movePosTime);


        // �����
    }
}
