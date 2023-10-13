using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_OS : BulletBase
{

    [SerializeField]
    Sprite[] sprites; // 0:로봇 이미지, 1: 폭발 이미지


    Collider2D collExplosion, collOSBot; // 콜라이더들
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;
    Scanner scanner;
    Transform target;

    bool isExplosion;

    private void Awake()
    {
        collExplosion = GetComponent<Collider2D>(); // 폭발모드 콜라이더
        collOSBot = GetComponentInChildren<Collider2D>(); // OS로봇 모드 콜라이더
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        scanner = GetComponent<Scanner>();
        isExplosion = false;
    }

    private void OnEnable()
    {
        Vector3 playerPos = GameManager.Instance.player.transform.position;
        Vector3 targetPos = GameManager.Instance.player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - playerPos;
        dir = dir.normalized;//방향 구하기

        transform.position = playerPos + dir;
    }

    private void FixedUpdate()
    {
        if (isExplosion) // 적과 만나 폭발했으면 이동X
            return;
        if (!scanner.nearestTarget)
            return;

        target = scanner.nearestTarget;// OS 객체의 scanner를 통해 최단거리 적 찾아감
        Vector3 dirVec = target.position - transform.position;
        Vector3 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(transform.position + nextVec);
        rigid.velocity = Vector2.zero;
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
            return;
        rigid.velocity = Vector2.zero;

        spriteRenderer.sprite = sprites[1];//이미지를 폭발로 변경
        
        isExplosion = true; // 폭발했음으로 바꿈

        StartCoroutine(ExplosionRoutine(() => { gameObject.SetActive(false); }));
        
    }
    private void OnDisable()
    {
        collExplosion.enabled = false;
        collOSBot.enabled = true;

        spriteRenderer.sprite = sprites[0];
        transform.position = new Vector3(0, 0, 0);
        isExplosion = false;
        // transform.rotation = Quaternion.identity;
    }
    IEnumerator ExplosionRoutine(System.Action done)
    {
        collOSBot.enabled = false;
        collExplosion.enabled = true;

        float timer = 0f;
        while (timer <= lifeTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        done.Invoke();
    }
}
