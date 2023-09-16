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
        //자기 자신을 포함한 자식들 component 싹다 갖고옴
    }
    void Update()
    {
        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.Instance.gameTime / 10f), spawnData.Length - 1);
        //나눠서 소수점 버림

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
        //1인 이유는 자기 자신 제외
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}

//속성-직렬화를 넣어주면 unity에서도 볼 수 있음
[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}
