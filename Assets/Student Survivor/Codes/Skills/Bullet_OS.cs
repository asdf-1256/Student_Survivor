using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_OS : BulletBase
{

    Rigidbody2D rigid;
    float timer;

    [SerializeField] private GameObject fragmentPrefab;

    private int fragmentIndex;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        fragmentIndex = GameManager.Instance.pool.GetPoolIndex(fragmentPrefab);
    }
    public override void Init(bool isAI, SkillData skillData, int level)
    {
        base.Init(isAI, skillData, level);


        Vector2 randomCircle = Random.insideUnitCircle; // �� ���� �� ��
        Vector3 randomPosition = new Vector3(randomCircle.x, randomCircle.y, 0);
        randomPosition = randomPosition.normalized; // �� ���� �� ��

        transform.position = playerTransform.position;
        Vector3 dir = randomPosition;
        rigid.velocity = dir * speed;

        StartCoroutine(MoveRoutine()); // �ش� ������Ʈ�� On �� ������ ����
    }

    IEnumerator MoveRoutine()
    {
        while (timer < flightTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        timer = 0f;
        StartCoroutine(SaBangPalBang());
    }

    IEnumerator SaBangPalBang()
    {
        Fire(1, 0);
        Fire(1, 1);
        Fire(0, 1);
        Fire(-1, 0);
        Fire(-1, -1);
        Fire(0, -1);
        Fire(1, -1);
        Fire(-1, 1);
        gameObject.SetActive(false);
        yield return null;
    }
    void Fire(int x, int y)
    {

        Vector3 dir = new Vector3(x, y, 0);
        dir = dir.normalized;

        Transform bullet = GameManager.Instance.pool.Get(fragmentIndex).transform; // Bullet 1�� �Ѿ� �״�� �ϴ� ��

        bullet.position = transform.position;//��ġ����
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);//ȸ������
        bullet.GetComponent<Bullet>().Init(damage, 1, dir);
    }

}


