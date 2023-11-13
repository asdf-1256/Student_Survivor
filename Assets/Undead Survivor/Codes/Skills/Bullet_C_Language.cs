using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_C_Language : BulletBase
{
    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public override void Init(bool isAI, SkillData skillData, int level)
    {
        base.Init(isAI, skillData, level);
        Debug.Log("C언어 Init 실행됨!!");
        transform.position = playerTransform.position;
        Vector3 targetPos = playerTransform.GetComponent<Scanner>().nearestTarget.position;
        Vector3 dir = targetPos - playerTransform.position;
        dir = dir.normalized;//방향 구하기
        transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        rigid.velocity = dir * speed;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
            return;
        rigid.velocity = Vector2.zero;
        gameObject.SetActive(false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;

        gameObject.SetActive(false);
    }
}
