using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    int level;
    float timer;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        //�ڱ� �ڽ��� ������ �ڽĵ� component �ϴ� ������
    }
    void Update()
    {
        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.Instance.gameTime / 10f), spawnData.Length - 1);
        //������ �Ҽ��� ����

        if (timer > spawnData[level].spawnTime)
        {
            timer = 0f;
            Spwan();
        }
  
    }
    void Spwan()
    {
        GameObject enemy = GameManager.Instance.pool.Get(0);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        //1�� ������ �ڱ� �ڽ� ����
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}

//�Ӽ�-����ȭ�� �־��ָ� unity������ �� �� ����
[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}