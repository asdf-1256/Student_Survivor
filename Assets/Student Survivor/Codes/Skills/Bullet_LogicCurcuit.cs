using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_LogicCurcuit : BulletBase
{

    [SerializeField]
    Sprite[] sprites; // 0:Ʈ�������� �̹���, 1: ���� �̹���


    Collider2D collExplosion, collCurcuit; // �ݶ��̴���
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        collExplosion = GetComponent<Collider2D>(); // ���߸�� �ݶ��̴�
        collCurcuit = GetComponentInChildren<Collider2D>(); // OS�κ� ��� �ݶ��̴�
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public override void Init(bool isAI, SkillData skillData, int level)
    {
        base.Init(isAI, skillData, level);

        transform.position = playerTransform.position;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
            return;
        Debug.Log("���� ������!");
        spriteRenderer.sprite = sprites[1];//�̹����� ���߷� ����

        StartCoroutine(ExplosionRoutine(() => { gameObject.SetActive(false); }));

    }
    private void OnDisable()
    {
        collExplosion.enabled = false;
        collCurcuit.enabled = true;

        spriteRenderer.sprite = sprites[0];
        transform.position = new Vector3(0, 0, 0);
        // transform.rotation = Quaternion.identity;
    }
    IEnumerator ExplosionRoutine(System.Action done)
    {
        collCurcuit.enabled = false;
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
