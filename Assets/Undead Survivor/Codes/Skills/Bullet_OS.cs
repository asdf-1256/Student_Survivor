using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_OS : MonoBehaviour
{

    [SerializeField]
    Sprite[] sprites; // 0:로봇 이미지, 1: 일단 커피 이미지

    public float duration;
    public float damage;

    Collider2D coll;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, Vector3 dir)
    {
        this.damage = damage;
        rigid.velocity = dir * 5f; // 속도 일단 5로 둬. 하드코딩
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
            return;
        rigid.velocity = Vector2.zero;

        spriteRenderer.sprite = sprites[1];//이미지를 일단 커피로 변경 -> 추후 폭발로 바꿔
        float timer = 0f;
        while (timer < duration)
        { //n초 동안 대기
            timer += Time.deltaTime;
        }

        gameObject.SetActive(false);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;

        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        spriteRenderer.sprite = sprites[0];
        transform.position = new Vector3(0, 0, 0);
        // transform.rotation = Quaternion.identity;
        // StopAllCoroutines();
    }
}
