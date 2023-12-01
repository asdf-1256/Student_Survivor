using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet_SystemProgramming : BulletBase
{
    Rigidbody2D rigid;
    Collider2D coll;
    Coroutine timer;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
        rigid = GetComponent<Rigidbody2D>();
    }

    public override void Init(bool isAI, SkillData skillData, int level)
    {
        base.Init(isAI, skillData, level);

        Vector3 playerPos = playerTransform.position;
        Vector3 targetPos = playerTransform.GetComponent<Scanner>().nearestTarget.position;

        transform.position = playerPos;
        Vector3 dir = targetPos - playerPos;
        transform.LookAt(dir);

        rigid.velocity = dir.normalized * speed;

        coll.enabled = true;
    }
    private void OnEnable()
    {
        timer = StartCoroutine(ExtinctTimerRoutine());
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
            return;

        coll.enabled = false;

        rigid.velocity = Vector3.zero;

        transform.parent = collision.transform;
        transform.rotation = Quaternion.identity;
        transform.localPosition = new Vector3(0, 1, 0);
    }
    //�߻�ǰ� ���� ������ ��Ȱ��ȭ ��Ű�� ��ƾ
    //CompareTag("Area") �Ϸ��� �ߴµ� �� �� ���� �Ƹ� transform.parent�� �ٲٴ� �������� Area���� ����� �׷���...
    IEnumerator ExtinctTimerRoutine()
    {
        yield return new WaitForSeconds(lifeTime + 10f);
        gameObject.SetActive(false);
    }

    public void DisableTimer()
    {
        if (timer != null)
            StopCoroutine(timer);

    }
}
