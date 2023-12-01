using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_DataBase : BulletBase
{
    GameObject blackhole; //�ڽ� ������Ʈ
    Rigidbody2D rigid; //���� Rigidbody2D

    float timer = 0;
    private void Awake()
    {
        blackhole = transform.GetChild(0).gameObject;
        rigid = GetComponent<Rigidbody2D>();
    }

    public override void Init(bool isAI, SkillData skillData, int level)//OnEnable������ ��ġ�� �����ϴϱ� ������Ʈ�� ó�� ������ ���� Rigidbody2D�� velocity�� (0,0)�� �Ǵ� ������ �־
    {
        base.Init(isAI, skillData, level);

        transform.position = GameManager.Instance.player.transform.position;
        Vector3 targetPos = GameManager.Instance.player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - GameManager.Instance.player.transform.position;
        dir = dir.normalized;//���� ���ϱ�
        transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        rigid.velocity = Vector3.zero;
        rigid.velocity = dir * speed;
    }
    private void Update() //Ÿ�̸�
    {
        timer += Time.deltaTime;

        if (timer > lifeTime)
        {
            timer = 0;
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) //���� �浹 ���� - ��Ȧ Ȱ��ȭ
    {
        if (!collision.CompareTag("Enemy"))
            return;
        rigid.velocity = Vector3.zero;
        transform.rotation = Quaternion.identity;
        blackhole.SetActive(true);
    }
    private void OnTriggerExit2D(Collider2D collision) //�÷��̾� �ֺ����� �־��� ��� �Ѿ��� ��Ȱ��ȭ, ��Ȧ�� Ȱ��ȭ�� ���¶�� �� �۾��� ����
    {
        if (!collision.CompareTag("Area"))
            return;

        if (blackhole.activeSelf)
            return;
        gameObject.SetActive(false);
    }
    private void OnEnable() //�߻� ����
    {
        transform.position = GameManager.Instance.player.transform.position;
        Vector3 targetPos = GameManager.Instance.player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - GameManager.Instance.player.transform.position;
        dir = dir.normalized;//���� ���ϱ�
        transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        rigid.velocity = Vector3.zero;
        rigid.velocity = dir * speed;
    }
    private void OnDisable() //��Ȧ ����
    {
        blackhole.SetActive(false);
    }
}
