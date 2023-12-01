using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_IoT : BulletBase
{
    public float spawnDistance = 5;
    public float movePosTime = 2f; // ���� ���������� �����ϴ� �ð�

    Rigidbody2D rigid;
    Scanner scanner;

    float lifetimer;
    float attackTimer;
    Vector3 nextPos;

    [SerializeField] private GameObject bulletPrefab;

    private int bulletIndex;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        scanner = GetComponent<Scanner>();
        bulletIndex = GameManager.Instance.pool.GetPoolIndex(bulletPrefab);
    }
    public override void Init(bool isAI, SkillData skillData, int level)
    {
        base.Init(isAI, skillData, level);

        Vector2 randomCircle = Random.insideUnitCircle.normalized; // �� ���� �� ��
        Vector3 spawnPosition = new Vector3(randomCircle.x, randomCircle.y, 0);

        transform.position = playerTransform.position + spawnPosition * spawnDistance; // ĳ���� �߽����� ������ 5�� �� ���� �� ��

        // ��� �÷��̾� ������ ��ȣ�ϱ�?
        StartCoroutine(setNextPos());
    }


    private void Update() // Ÿ�̸� ���
    {
        if (!GameManager.Instance.isLive)
            return;

        lifetimer += Time.deltaTime;
        attackTimer += Time.deltaTime;

        if (lifetimer > lifeTime)
        {
            lifetimer = 0f;
            gameObject.SetActive(false); // lifeTime �� �Ǹ� ��Ȱ��ȭ
        }
        if (attackTimer > attackCoolTime)
        {
            attackTimer = 0f;
            FireDDabal();
        }
    }

    private void FixedUpdate() // �׻� nextPos�� �̵�
    {
        if (Vector3.Distance(transform.position, nextPos) < 0.1f)
            return; // nextPos�� ���������� ���߱�

        Vector3 dirVec = nextPos - transform.position;
        Vector3 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(transform.position + nextVec);
        rigid.velocity = Vector2.zero;
    }

    IEnumerator setNextPos()
    {
        // n�ʸ��� �÷��̾� ������ �� �������� �����ϴ� �ڷ�ƾ
        while (true)
        {
            Vector2 randomCircle = Random.insideUnitCircle; // �� ���� �� ��
            Vector3 randomPos = new Vector3(randomCircle.x, randomCircle.y, 0);
            nextPos = GameManager.Instance.player.transform.position + randomPos * 3;

            yield return new WaitForSeconds(movePosTime);
        }
    }

    void FireDDabal()
    {
        if (!scanner.nearestTarget)
            return;

        Vector2 randomCircle = Random.insideUnitCircle * 2; // r=2�� �� ���� ������ ��ġ
        Vector3 ddabalRate = new Vector3(randomCircle.x, randomCircle.y, 0); // vector3�� ��ȯ

        Vector3 targetPos = scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position + ddabalRate; // �÷��̾�->�� ���Ϳ� ���߷� ÷��
        dir = dir.normalized;//���� ���ϱ�

        Transform bullet = GameManager.Instance.pool.Get(bulletIndex).transform; // Bullet 1�� �Ѿ� �״�� �ϴ� ��

        bullet.position = transform.position;//��ġ����
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);//ȸ������
        bullet.GetComponent<Bullet>().Init(damage, 1, dir);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
