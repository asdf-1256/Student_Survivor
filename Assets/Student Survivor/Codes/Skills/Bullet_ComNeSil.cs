using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_ComNeSil : BulletBase
{
    Rigidbody2D rigid;
    float timer;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        InvokeRepeating("ScaleUp", 0.1f, 0.1f);  // 0.1f�ʸ��� �Լ� ����, ��� ��Ȱ��ȭ ���¿����� ��� �ݺ���
    }

    public override void Init(bool isAI, SkillData skillData, int level)
    {
        base.Init(isAI, skillData, level);

        Vector3 playerPos = playerTransform.position;
        Vector3 targetPos = playerTransform.GetComponent<Scanner>().nearestTarget.position;
        Vector3 dir = targetPos - playerPos;
        dir = dir.normalized;//���� ���ϱ�

        transform.position = playerPos + dir;
        transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);//ȸ������

        rigid.velocity = dir * speed;
    }


    private void Update() // �Ѿ� �ϳ��ϳ��� Ÿ�̸�
    {
        timer += Time.deltaTime;

        if (timer > lifeTime)
        {
            timer = 0f;
            transform.localScale = new Vector3(2f, 2f, 0f);
            gameObject.SetActive(false);
        }
    }
    private void ScaleUp()
    {
        if (!gameObject.activeSelf)
            return;
        Vector3 newScale = transform.localScale + new Vector3(scaleFactor, scaleFactor, 0f);
        transform.localScale = newScale;
    }
}
