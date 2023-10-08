using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_SystemProgramming : MonoBehaviour
{
    //흠... 총알을 날려 -> 맞으면 -> 1.collider 해제, 2.회전 변경 3.부모자식변경, 3-1.y축 local좌표 변경 4.적 속도 변경, 5.시간 다 지나면 부모자식 다시 풀매니저로 변경
    //5-1시간 다 지나면 이미지 변경.
    //만약 정지 중에 죽게 된다면? 부모자식 해제

    //총알 날리기 전에 로테이션 초기화, 콜라이더 활성

    public Sprite[] sprites;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;
    Collider2D coll;

    public float speed = 2.0f;
    private void Awake()
    {
        coll = GetComponent<Collider2D>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        coll.enabled = false;
    }
    private void OnEnable()
    {
        Transform target = GameManager.Instance.player.scanner.nearestTarget;
        Transform player = GameManager.Instance.player.transform;
        transform.position = player.position;

        Vector3 dir = target.position - player.position;
        transform.LookAt(dir);

        rigid.velocity = dir.normalized * speed;

        coll.enabled = true;

        spriteRenderer.sprite = sprites[0];
    }
}
