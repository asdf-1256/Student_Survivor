using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_MachhineLearning : MonoBehaviour
{

    public float damage;
    public float speed;
    public float lifeTime;


    Rigidbody2D rigid;
    Scanner scanner;
    Transform target;
    SpriteRenderer spriteRenderer;

    float timer;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        scanner = GetComponent<Scanner>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init(float damage, float speed, float lifeTime)
    {
        this.damage = damage;
        this.speed = speed;
        this.lifeTime = lifeTime;
    }

    private void Update()
    {
        if (!GameManager.Instance.isLive)
            return;

        timer += Time.deltaTime;

        if (timer > lifeTime)
        {
            timer = 0f;
            gameObject.SetActive(false); // lifeTime 다 되면 비활성화
        }
    }

    private void FixedUpdate()
    {
        if (!scanner.nearestTarget)
            return;

        target = scanner.nearestTarget;// 객체의 scanner를 통해 최단거리 적 찾아감
        Vector3 dirVec = target.position - transform.position;
        Vector3 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(transform.position + nextVec);
        rigid.velocity = Vector2.zero;
    }
    private void LateUpdate()
    {
        if (!GameManager.Instance.isLive || !target) // 플레이어 사망 혹은 타겟이 null인 경우 제외
            return;
        spriteRenderer.flipX = target.position.x < rigid.position.x;
    }
}
