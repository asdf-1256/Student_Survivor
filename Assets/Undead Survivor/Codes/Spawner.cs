using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    //public float levelTime;
    public float ItemsRandomSpawnArea; //.. 플레이어 주위로 아이템 스폰되는 써클의 반지름
    public float ItemSpawnTime;
    [SerializeField] private readonly float[] spawnTimes = { 2, 1, 0.2f, 2, 1, 0.2f, 2, 2 };

    private WaitForSeconds WaitSpawnTime; //디버깅용- 아이템이 스폰되는데 걸리는 시간. default:1초

    //int level;
    [SerializeField] float timer;

    public SpawnItemData[] itemDatas;
    public int[] dropRates; //SpawnItemData 내부에 있는 드롭율 부분을 읽어와 순서대로 저장하는 배열
    private int totalDropRate; // dropRates 배열의 값을 다 더한 숫자

    public bool isPlayer;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        //자기 자신을 포함한 자식들 component 싹다 갖고옴

        if (isPlayer)
        {

            dropRates = new int[itemDatas.Length];

            for (int i = 0; i < dropRates.Length; i++)
                dropRates[i] = itemDatas[i].dropRate;

            totalDropRate = dropRates.Sum();

            ItemSpawnTime = 1f;
            WaitSpawnTime = new WaitForSeconds(ItemSpawnTime);


            StartCoroutine(CreateCoinRoutine());
        }
    }

    private void OnValidate()//유니티 Inspector에서 값이 변경될 경우 호출되는 함수.
    {
        totalDropRate = dropRates.Sum();//드롭율 총합 다시 계산
        WaitSpawnTime = new WaitForSeconds(ItemSpawnTime);//아이템 스폰 시간 객체 다시 만듬
    }
    void Update()
    {
        if (!GameManager.Instance.isLive)
            return;

        timer += Time.deltaTime;

        if (timer > spawnTimes[GameManager.Instance.currentPhase])
        {
            timer = 0f;
            SpwanEnemy();
        }

    }

    IEnumerator CreateCoinRoutine()
    {
        while (true)
        {
            yield return WaitSpawnTime; //new를 없애기 위해 awake단계에서 WaitForSeconds를 만듬
            int randomItemNum = SelectRandomItem();
            SpawnItem(randomItemNum);
        }
    }
    void SpwanEnemy()
    {
        GameObject enemy = GameManager.Instance.pool.Get(0);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        //1인 이유는 자기 자신 제외
        enemy.GetComponent<Enemy>().Init(spawnData[Random.Range(0, spawnData.Length)]);
    }

    int SelectRandomItem()
    {
        int resultItem = 0;
        int select = Random.Range(0, totalDropRate);

        for (int i = 0; i < dropRates.Length; i++)
        {
            select -= dropRates[i];

            if (select <= 0)
            {
                resultItem = i;
                break;
            }
        }

        return resultItem;
    }

    void SpawnItem(int itemNum) //Item : 3번, 드랍율에 따라 선택된 정수값을 받아, 해당 정수값을 index로 하는 데이터의 아이템을 스폰.
    {
        GameObject item = GameManager.Instance.pool.Get(3);
        item.GetComponent<SpawnItem>().Init(itemDatas[itemNum]);
        item.transform.position = new Vector2(transform.position.x, transform.position.y) + Random.insideUnitCircle * ItemsRandomSpawnArea;
    }
    public void SpawnItem(SpawnItemData itemData)
    {
        GameObject item = GameManager.Instance.pool.Get(3);
        item.GetComponent<SpawnItem>().Init(itemData);
        item.transform.position = new Vector2(transform.position.x, transform.position.y) + Random.insideUnitCircle * 10;
    }
}

//속성-직렬화를 넣어주면 unity에서도 볼 수 있음
[System.Serializable]
public class SpawnData
{
    //public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}
