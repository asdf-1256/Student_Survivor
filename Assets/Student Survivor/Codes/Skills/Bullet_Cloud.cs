using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Cloud : BulletBase
{
    public float spawnDistance = 5;
    public float dropTimeOfLaptop = 0.6f;

    Rigidbody2D rigid;
    Transform target;
    GameObject laptop;
    Collider2D coll;

    float timer;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        coll.enabled = false;
        laptop = transform.GetChild(2).gameObject; // ��ž ������Ʈ ��������
    }

    public override void Init(bool isAI, SkillData skillData, int level)
    {
        base.Init(isAI, skillData, level);

        Vector2 randomCircle = Random.insideUnitCircle.normalized; // �� ���� �� ��
        Vector3 spawnPosition = new Vector3(randomCircle.x, randomCircle.y, 0);

        transform.position = playerTransform.position + spawnPosition * spawnDistance; // ĳ���� �߽����� ������ 5�� �� ���� �� ��
        StartCoroutine(DropRoutine());
    }

    private void Update() // Ÿ�̸� ���
    {
        if (!GameManager.Instance.isLive)
            return;

        timer += Time.deltaTime;

        if (timer > lifeTime)
        {
            timer = 0f;
            gameObject.SetActive(false); // lifeTime �� �Ǹ� ��Ȱ��ȭ
        }
    }

    private void FixedUpdate() // Ÿ�ٿ��� �̵�
    {
        if (!target || Vector3.Distance(transform.position, target.position) < 0.1f) // Ÿ���� ��Ȱ��ȭ�ų� �������� ��
            target = GameManager.Instance.player.scanner.GetRandomTarget(); // ������ Ÿ�� ����
        if (!target)
            return; // sccaner�� null�� �޾ƿ��� ��쵵 ����

        Vector3 dirVec = target.position - transform.position;
        Vector3 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(transform.position + nextVec);
        rigid.velocity = Vector2.zero;
    }


    IEnumerator DropRoutine()
    {
        while (true)
        {
            laptop.transform.position = transform.position + new Vector3(0, 2, 0);
            laptop.SetActive(true);
            
            yield return new WaitForSeconds(dropTimeOfLaptop - 0.1f); // ��ž ������������ ���
            coll.enabled = true;
            yield return new WaitForSeconds(0.1f); // ��ž ������������ ���
            coll.enabled = false;

            laptop.SetActive(false);

            yield return new WaitForSeconds(attackCoolTime);
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

}
