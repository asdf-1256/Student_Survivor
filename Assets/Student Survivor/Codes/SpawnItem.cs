using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

//�ʿ� �����Ǵ� ������ prefab�� ���� ��ũ��Ʈ
//prefab�� Coin �ϳ����� �������
public class SpawnItem : MonoBehaviour
{
    private Transform target;
    public SpawnItemData data;
    SpriteRenderer spriteRenderer; //�̹��� ������ ���� SpriteRenderer
    [SerializeField] private readonly float MagneticConst = 4f;//�ڷ� ���
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
    //Spawner���� �������� ������ ��, Init�Լ��� ȣ����.
    public void Init(SpawnItemData data)
    {
        this.data = data;//Spawner�� �ִ� itemDatas �迭�� �ִ� ������(Inspector�󿡼� ���� �������� Scriptable Object �����Ͱ� ������) �迭���� �����͸� �޾ƿ� ����
        spriteRenderer.sprite = data.image;//�����ۿ� �°� �̹��� ����
        
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
            Debug.Log("�ڼ� �� ����");
        }

    }
    private void FixedUpdate()
    {
        if (!data.magnetable)
            return;

        if (!isMagneted)
            return;
        
        Vector2 directionVect = target.position - transform.position; // ���� -> �÷��̾� ���� ���ϱ�
        float distance = directionVect.magnitude; //�Ÿ� ���ϱ�
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
