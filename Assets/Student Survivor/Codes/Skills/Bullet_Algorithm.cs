using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Algorithm : BulletBase
{
    public float spawnDistance = 3;
    Collider2D coll;

    float timer;
        private void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    public override void Init(bool isAI, SkillData skillData, int level)
    {
        base.Init(isAI, skillData, level);


        Vector2 randomCircle = Random.insideUnitCircle; // �� ���� �� ��
        Vector3 spawnPosition = new Vector3(randomCircle.x, randomCircle.y, 0);
        spawnPosition = spawnPosition.normalized; // �� ���� �� ��

        transform.position = playerTransform.position + spawnPosition * spawnDistance;

        StartCoroutine(RotateRoutine()); // �ش� ������Ʈ�� On �� ������ ����
    }

    IEnumerator RotateRoutine()
    {
        Quaternion targetRotation = Quaternion.Euler(0, 0, 90f);
        float rotationThreshold = 0.01f;
        // Ÿ��ȸ���������� ���̰� 0.01 ������ ������ �ݺ�
        while (Quaternion.Angle(transform.rotation, targetRotation) > rotationThreshold)
        {
            // deltaTime�� ���� �������� �޶� ���� �ӵ� ����
            float step = rotateSpeed * Time.deltaTime;
            // ���� �������� Ÿ��ȸ�������� step��ŭ ȸ��
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);
            yield return null; // ���� �����ӱ��� �纸
        }
        yield return StartCoroutine(MainRoutine(() =>
        {
            gameObject.SetActive(false);
        } ));
    }

    IEnumerator MainRoutine(System.Action done) // LifeTime���� �׳� Ÿ�̸� ��� �ڷ�ƾ
    {
        coll.enabled = true; // collider Ȱ��ȭ
        while (timer < lifeTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        timer = 0f;
        done.Invoke(); // ���� ��Ȱ��ȭ �Լ� ȣ��
    }

    private void OnDisable()
    {
        coll.enabled = false;
        transform.rotation = Quaternion.identity;
        StopAllCoroutines();
    }

}
