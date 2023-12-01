using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Python : BulletBase
{
    public float high;
    public GameObject PythonPrefab;
    public int prefabId;

    public Sprite[] pythonImage;

    float timer;
    private void Awake()
    {
        prefabId = GameManager.Instance.pool.GetPoolIndex(PythonPrefab);
    }
    public override void Init(bool isAI, SkillData skillData, int level)
    {
        base.Init(isAI, skillData, level);

        transform.parent = playerTransform; // �θ� pool���� player�� �ٲٱ�
        transform.localPosition = Vector3.zero;

        Arrange();

    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > lifeTime)
        {
            timer = 0f;
            transform.parent = GameManager.Instance.pool.transform; // ����� �� �ٽ� �θ� pool�� �ٲٱ�
            gameObject.SetActive(false);
        }
        transform.Rotate(Vector3.back * (speed * Time.deltaTime)); // ȸ���ֱ�
    }
    void Arrange()
    {
        float countTwice = count * 2;
        for (int index = 0; index < countTwice; index++)
        {
            Transform bullet;
            if (index < transform.childCount)
            {
                bullet = transform.GetChild(index);
            }
            else
            {
                bullet = GameManager.Instance.pool.Get(prefabId).transform;
                //�� �θ�:Ǯ �Ŵ��� -> �÷��̾�� �ٲ��� �� �ֵ��� Ʈ������ �����
                bullet.parent = transform;
            }
            if (index % 2 == 0)
                bullet.GetComponent<SpriteRenderer>().sprite = pythonImage[0];
            else
                bullet.GetComponent<SpriteRenderer>().sprite = pythonImage[1];

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            

            Vector3 rotVec = Vector3.forward * (360 * index / countTwice);
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * high, Space.World);

            bullet.GetComponent<BulletBase>().putDamage(damage); // �������� �������ֱ�
        }
    }
}

