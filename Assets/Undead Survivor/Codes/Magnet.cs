using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    [SerializeField]
    private float radius; //������ ������ ����
    private float magneticRate;
    public float Radius
    {
        get { return radius; }
        set { radius = value; coll.radius = radius * magneticRate; }
    }
    public float MagneticRate
    {
        get { return magneticRate; }
        set { magneticRate = value; coll.radius = radius * magneticRate; }
    }

    CircleCollider2D coll; // �ڷ� ������ ������ Collider

    private void Awake()
    {
        coll = GetComponent<CircleCollider2D>();
        radius = coll.radius;
        magneticRate = 1f;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Item"))//Item���� �������.
            return;


        MagnetableItem magnet;//���� �浹�� �������� ��ũ��Ʈ�� �ҷ���
        if (!collision.TryGetComponent<MagnetableItem>(out magnet))
        {
            Debug.LogWarning("TryGetComponent�� �������� ��� �����");
            return;
        }
            magnet.ActiveMagnet(coll.radius);//�ڼ��� Ȱ��ȭ �Ѵ�.



    }

    IEnumerator CreateMagRoutine() // 10�� ���� ������ 10 ����
    {
        Debug.Log("�ڼ��ڼ�");

        coll.radius += 10;
        yield return new WaitForSeconds(10);
        coll.radius -= 10;
        Debug.Log("�ڼ� ȿ�� ��");
    }

    private void LevelUpColliderRadius()
    {
        coll.radius *= 2f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space Key Input �߻�");
            LevelUpColliderRadius();
        }
    }
}