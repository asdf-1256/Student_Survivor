using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Mobile: BulletBase
{
    // ���� OS �ҽ��ڵ�
    [SerializeField]
    Sprite[] sprites; // 0:�κ� �̹���, 1: ���� �̹���


    Collider2D collExplosion, collOSBot; // �ݶ��̴���
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;
    Scanner scanner;
    Transform target;

    bool isExplosion;

    private void Awake()
    {
        collExplosion = GetComponent<Collider2D>(); // ���߸�� �ݶ��̴�
        collOSBot = GetComponentInChildren<Collider2D>(); // OS�κ� ��� �ݶ��̴�
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
        dir = dir.normalized;//���� ���ϱ�

        transform.position = playerPos + dir;
    }

    private void FixedUpdate()
    {
        if (isExplosion) // ���� ���� ���������� �̵�X
            return;
        if (!scanner.nearestTarget)
            return;

        target = scanner.nearestTarget;// OS ��ü�� scanner�� ���� �ִܰŸ� �� ã�ư�
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

        spriteRenderer.sprite = sprites[1];//�̹����� ���߷� ����

        isExplosion = true; // ������������ �ٲ�

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
