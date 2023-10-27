using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_C_Language : BulletBase
{
    Rigidbody2D rigid;
    Player player;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        player = GameManager.Instance.player;
    }

    private void OnEnable()
    {
        transform.position = player.transform.position;
        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - player.transform.position;
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
