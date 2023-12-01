using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Python : BulletBase
{
    public float high;
    public GameObject PythonPrefab;
    public int prefabId;


    private void Awake()
    {
        /*
        for (int index = 0; index < GameManager.Instance.pool.prefabs.Length; index++)
        {
            if (PythonPrefab == GameManager.Instance.pool.prefabs[index])
            {
                prefabId = index;
                break;
            }
        }*/

        prefabId = GameManager.Instance.pool.GetPoolIndex(PythonPrefab);
    }
    private void OnEnable()
    {
        transform.parent = GameManager.Instance.player.transform; // �θ� pool���� player�� �ٲٱ�
        transform.localPosition = Vector3.zero;

        Arrange();
    }

    private void Update()
    {
        transform.Rotate(Vector3.back * speed * Time.deltaTime);
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
                //�� �θ�:Ǯ �Ŵ��� -> �÷��̾�� �ٲ��� �� �ֵ��� Ʈ������ �����
                bullet.parent = transform;
            }

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * high, Space.World);

            bullet.GetComponent<BulletBase>().damage = damage; // �������� �������ֱ�
        }
    }

    private void OnDisable()
    {
        transform.parent = GameManager.Instance.pool.transform; // ����� �� �ٽ� �θ� pool�� �ٲٱ�
    }
}
