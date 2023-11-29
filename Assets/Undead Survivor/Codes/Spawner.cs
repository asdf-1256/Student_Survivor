using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    //public float levelTime;
    public float ItemsRandomSpawnArea; //.. �÷��̾� ������ ������ �����Ǵ� ��Ŭ�� ������
    public float ItemSpawnTime;
    [SerializeField] private readonly float[] spawnTimes = { 2, 1, 0.2f, 2, 1, 0.2f, 2, 2 };

    private WaitForSeconds WaitSpawnTime; //������- �������� �����Ǵµ� �ɸ��� �ð�. default:1��

    //int level;
    [SerializeField] float timer;

    public SpawnItemData[] itemDatas;
    public int[] dropRates; //SpawnItemData ���ο� �ִ� ����� �κ��� �о�� ������� �����ϴ� �迭
    private int totalDropRate; // dropRates �迭�� ���� �� ���� ����

    public bool isPlayer;

    [SerializeField] GameObject enemyPrefab;
    private int enemyPoolIndex;

    [SerializeField] GameObject spawnItemPrefab;
    private int itemPoolIndex;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        //�ڱ� �ڽ��� ������ �ڽĵ� component �ϴ� ������
        enemyPoolIndex = GameManager.Instance.pool.GetPoolIndex(enemyPrefab);
        itemPoolIndex = GameManager.Instance.pool.GetPoolIndex(spawnItemPrefab);
    }
    private void OnEnable()
    {
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
    private void OnDisable()
    {
        StopCoroutine(CreateCoinRoutine());
    }

    private void OnValidate()//����Ƽ Inspector���� ���� ����� ��� ȣ��Ǵ� �Լ�.
    {
        totalDropRate = dropRates.Sum();//����� ���� �ٽ� ���
        WaitSpawnTime = new WaitForSeconds(ItemSpawnTime);//������ ���� �ð� ��ü �ٽ� ����
    }
    void Update()
    {
        if (!GameManager.Instance.isLive)
            return;

        timer += Time.deltaTime * ((!isPlayer) ? 5f:1f);

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
            yield return WaitSpawnTime; //new�� ���ֱ� ���� awake�ܰ迡�� WaitForSeconds�� ����
            int randomItemNum = SelectRandomItem();
            SpawnItem(randomItemNum);
        }
    }
    void SpwanEnemy()
    {
        GameObject enemy = GameManager.Instance.pool.Get(enemyPoolIndex);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        //1�� ������ �ڱ� �ڽ� ����
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

    void SpawnItem(int itemNum) //Item : 3��, ������� ���� ���õ� �������� �޾�, �ش� �������� index�� �ϴ� �������� �������� ����.
    {
        GameObject item = GameManager.Instance.pool.Get(itemPoolIndex);
        item.GetComponent<SpawnItem>().Init(itemDatas[itemNum]);
        item.transform.position = new Vector2(transform.position.x, transform.position.y) + Random.insideUnitCircle * ItemsRandomSpawnArea;
    }
    public GameObject SpawnItem(SpawnItemData itemData)
    {
        GameObject item = GameManager.Instance.pool.Get(itemPoolIndex);
        item.GetComponent<SpawnItem>().Init(itemData);
        item.transform.position = new Vector2(transform.position.x, transform.position.y) + Random.insideUnitCircle * ItemsRandomSpawnArea;
        return item;
    }

}

//�Ӽ�-����ȭ�� �־��ָ� unity������ �� �� ����
[System.Serializable]
public class SpawnData
{
    //public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}
