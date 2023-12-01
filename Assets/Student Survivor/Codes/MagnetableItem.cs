using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class MagnetableItem : MonoBehaviour
{

    Transform target;//�÷��̾��� ��ġ ��, Magnet�� �ݰ濡 ���Դ��� Ȯ����
    private readonly float MagneticConst = 4f;//�ڷ� ���
    

    private void Awake()
    {
        target = null;
    }
    
    //�������� �Ծ� ��Ȱ��ȭ �� ��, Ÿ���� ���ش�
    private void OnDisable()
    {
        target = null;
    }

    private void FixedUpdate()
    {
        if (target == null)//Ÿ���� ���� ��� - �ڷ� ������ ������ ���� ���.
            return;

        Vector2 directionVect = target.position - transform.position; // ���� -> �÷��̾� ���� ���ϱ�
        float distance = directionVect.magnitude; //�Ÿ� ���ϱ�
        Vector2 MagneticForce = directionVect.normalized * MagneticConst / distance; // �ڱ�� ���ϱ�
        if (MagneticForce.magnitude < 4f) //�ڱ���� �ʹ� �۴ٸ�(= �ӵ��� �ʹ� �����ٸ�) ���� ����(4)���� ����
            MagneticForce = MagneticForce.normalized * 4;
        transform.Translate(MagneticForce * Time.fixedDeltaTime); //������ ��ǥ�� ���� * ������ ��ŭ �̵���Ű��
    }

    //Player �ڽ� ������Ʈ�� Magnet�� Collider�� ����, ����ġ ���� �ڷ��� ������ �޴� �������� ���� ��� �� �Լ��� ȣ��Ǿ� target�� �ڷ��� Ȱ��ȭ�ǰ�
    //���� Player������ �����̴ٰ� Player�� �ڽ� ������Ʈ�� Collector�� Collider���� ������ �����ϵ��� ó��.
    public void ActiveMagnet()
    {
        target = GameManager.Instance.player.transform;
    }
}
