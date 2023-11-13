using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Python : BulletBase
{
    public float high;
    public GameObject PythonPrefab;
    public int prefabId;

    float timer;
    private void Awake()
    {
        for (int index = 0; index < GameManager.Instance.pool.prefabs.Length; index++)
        {
            if (PythonPrefab == GameManager.Instance.pool.prefabs[index])
            {
                prefabId = index;
                break;
            }
        }
    }
    public override void Init(bool isAI, SkillData skillData, int level)
    {
        base.Init(isAI, skillData, level);

        transform.parent = playerTransform; // 부모를 pool에서 player로 바꾸기
        transform.localPosition = Vector3.zero;

        Arrange();

    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > lifeTime)
        {
            timer = 0f;
            transform.parent = GameManager.Instance.pool.transform; // 사라질 땐 다시 부모를 pool로 바꾸기
            gameObject.SetActive(false);
        }
        transform.Rotate(Vector3.back * speed * Time.deltaTime); // 회전주기
    }
    void Arrange()
    {
        for (int index = 0; index < count; index++)
        {
            Transform bullet;
            if (index < transform.childCount)
            {
                bullet = transform.GetChild(index);
            }
            else
            {
                bullet = GameManager.Instance.pool.Get(prefabId).transform;
                //현 부모:풀 매니저 -> 플레이어로 바꿔줄 수 있도록 트랜스폼 갖고옴
                bullet.parent = transform;
            }

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * high, Space.World);

            bullet.GetComponent<BulletBase>().putDamage(damage); // 데미지만 전달해주기
        }
    }
}

