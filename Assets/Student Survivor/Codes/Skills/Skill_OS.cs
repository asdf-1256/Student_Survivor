using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_OS : MonoBehaviour
{
    public int bulletPrefabID;
    public float coolTime;
    public float damage;
    public float speed;
    public float lifeTime; // �����ϴ� �ð�?

    public GameObject Bullet; // �Ѿ��� � ���������� �����ֱ⸸ �ϴ� �뵵

    Player player;
    float timer;
    private void Awake()
    {
        player = GameManager.Instance.player;

        A_Skill_Data skillData = GetComponentInParent<A_Skill_Data>();
        bulletPrefabID = skillData.bulletPrefabID;
        coolTime = skillData.coolTime;
        damage = skillData.damage;
        lifeTime = skillData.lifeTime;
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

        
        Transform bullet = GameManager.Instance.pool.Get(bulletPrefabID).transform;

        // bullet.rotation = Quaternion.FromToRotation(Vector3.left, dir); // ȸ�� �� ��
        // bullet.GetComponent<Bullet_OS>().Init(damage, speed, lifeTime);


        // �����
    }
}
