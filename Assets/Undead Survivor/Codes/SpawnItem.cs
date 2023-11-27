using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

//맵에 스폰되는 아이템 prefab에 들어가는 스크립트
//prefab은 Coin 하나만을 사용했음
public class SpawnItem : MonoBehaviour
{
    private Transform target;
    public SpawnItemData data;
    SpriteRenderer spriteRenderer; //이미지 변경을 위한 SpriteRenderer
    [SerializeField] private readonly float MagneticConst = 4f;//자력 상수
    private float magnetForce;
    private bool isMagneted = false;

    [SerializeField] private readonly float lowerbound = 2f;
    [SerializeField] private readonly float upperbound = 16f;

    float timer = 0f;
    float reinforceMagnetForceTime = 1f;
    float magnetCalc;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        target = GameManager.Instance.player.transform;
    }
    //Spawner에서 아이템을 스폰할 때, Init함수를 호출함.
    public void Init(SpawnItemData data)
    {
        this.data = data;//Spawner에 있는 itemDatas 배열에 있는 데이터(Inspector상에서 보면 아이템의 Scriptable Object 데이터가 들어가있음) 배열에서 데이터를 받아와 설정
        spriteRenderer.sprite = data.image;//아이템에 맞게 이미지 변경
        
    }
    private void OnEnable()
    {
        Clear();
    }
    private void OnDisable()
    {
        Clear();
    }
    private void Update()
    {
        if (!data.magnetable)
            return;
        if (!isMagneted)
            return;

        timer += Time.deltaTime;
        magnetForce += magnetCalc * Time.deltaTime * 0.75f;
        if(timer > reinforceMagnetForceTime)
        {
            timer = 0f;
            magnetCalc = magnetForce;
            Debug.Log("자석 힘 쎄짐");
        }

    }
    private void FixedUpdate()
    {
        if (!data.magnetable)
            return;

        if (!isMagneted)
            return;
        
        Vector2 directionVect = target.position - transform.position; // 코인 -> 플레이어 벡터 구하기
        float distance = directionVect.magnitude; //거리 구하기
        float force = MagneticConst * magnetForce / (distance * distance);
        force = Mathf.Clamp(force, lowerbound, upperbound);
        transform.Translate(directionVect.normalized * (force * Time.fixedDeltaTime));
    }
    private void Clear()
    {
        isMagneted = false;
        magnetForce = 1f;
        timer = 0f;
    }
    public void ActiveMagnet(float MagnetSize)
    {
        isMagneted = true;
        magnetForce = Mathf.Max(MagnetSize / 2, magnetForce);
        magnetCalc = magnetForce;
    }
    public void DeActiveMagnet()
    {
        Clear();
    }
}
