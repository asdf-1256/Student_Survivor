using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Skill_Data : MonoBehaviour
{
    public int bulletPrefabID; // Bullet�� ������ ID
    public int level; // 0~2����
    public float coolTime;
    public float lifeTime;
    public float damage;
    public float speed;
    public float scaleFactor; // �ĳ׽� : ���� �ĵ��� ũ�� ���� �ӵ�
    public float flightTime; // �ڹ� : ü�� �ð� = ���� ���ư� Ŀ�Ƿ� ���ϱ� ������ �ð�
    public float rotateSpeed; // �ڹ� : ȸ���ϴ� �ӵ�
    public float attackCoolTime; // Ŭ���� : ��ȯü�� �����ϴ� ��Ÿ��
}
