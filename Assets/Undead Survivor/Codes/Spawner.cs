using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    public float levelTime;
    public float ItemsRandomSpawnArea; //.. 플레이어 주위로 아이템 스폰되는 써클의 반지름

    int level;
    float timer;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        //자기 자신을 포함한 자식들 component 싹다 갖고옴

        levelTime = GameManager.Instance.maxGameTime / spawnData.Length;

        ItemsRandomSpawnArea = GameManager.Instance.ItemsRandomSpawnArea;

        StartCoroutine(CreateCoinRoutine());
    }
    void Update()
    {
        if (!GameManager.Instance.isLive)
            return;

        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.Instance.gameTime / levelTime), spawnData.Length - 1);
        //나눠서 소수점 버림

        if (timer > spawnData[level].spawnTime)
        {
            timer = 0f;
            SpwanEnemy();
        }
 
    }

    IEnumerator CreateCoinRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            int randomItemNum = SelectRandomItem();
            SpawnItem(randomItemNum);
        }
    }
    void SpwanEnemy()
    {
        GameObject enemy = GameManager.Instance.pool.Get(0);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        //1인 이유는 자기 자신 제외
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }

    int SelectRandomItem() //.. 게임매니저의 아이템 별 degree수치에 따라 확률적으로 아이템 번호를 선택하는 함수
    {
        int resultItem;
        float coin_spd = GameManager.Instance.coin_spd;
        float exp0_spd = GameManager.Instance.exp0_spd;
        float exp1_spd = GameManager.Instance.exp1_spd;

        float total_spd = coin_spd + exp0_spd + exp1_spd;
        float select_spd = Random.Range(0f, total_spd);

        if (select_spd < coin_spd)
            resultItem = 3;
        else if (select_spd < coin_spd + exp0_spd)
            resultItem = 4;
        else
            resultItem = 5;
        return resultItem;
    }

    void SpawnItem(int itemNum) // 3 : 코인, 4 : Exp 0, 5 : Exp 1
    {
        GameObject item = GameManager.Instance.pool.Get(itemNum);
        item.transform.position = new Vector2(transform.position.x, transform.position.y) + Random.insideUnitCircle * ItemsRandomSpawnArea;
        
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
