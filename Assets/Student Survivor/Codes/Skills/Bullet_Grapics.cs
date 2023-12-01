using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Grapics : BulletBase
{
    public float spawnDistance = 10;

    GameObject selectedPolygon;

    private void Awake()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);
    }

    public override void Init(bool isAI, SkillData skillData, int level)
    {
        base.Init(isAI, skillData, level);

        Vector2 randomCircle = Random.insideUnitCircle; // �� ���� �� ��
        Vector3 spawnPosition = new Vector3(randomCircle.x, randomCircle.y, 0);


        transform.position = playerTransform.position + spawnPosition * spawnDistance; // ĳ���� �߽����� ������ 10�� �� ���� �� ��
        // transform.rotation = Random.rotation; // ���� ȸ��

        int selectedChildNum = Random.Range(0, 4); // 0~4 �� �ڽ� �� �� ������
        selectedPolygon = transform.GetChild(selectedChildNum).gameObject;
        selectedPolygon.SetActive(true); // ������Ʈ �ϳ��� Ȱ��ȭ

        StartCoroutine(TimerRoutine()); // Ÿ�̸� �ڷ�ƾ ����
    }

    IEnumerator TimerRoutine() // lifeTime �Ŀ� ��Ȱ��ȭ�ϴ� �ڷ�ƾ
    {
        float timer = 0f;
        while (timer < lifeTime)
        {
            timer += Time.deltaTime;

            yield return null;
        }
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        selectedPolygon.SetActive(false); // ��ų ����Ǹ� �ٽ� ��Ȱ��ȭ
    }
}
