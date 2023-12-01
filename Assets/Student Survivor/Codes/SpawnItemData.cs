using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�ʿ� �����Ǵ� �������� ScriptableObject�� �����, Spawner�� itemDatas �迭�� �Ҵ���.
//���⿡ ����� �߰� ���

//�ش� ScriptableObject�� ������ �����ʹ� Data/SpawnItemData�� ����.


[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/SpawnItemData")]
public class SpawnItemData : ScriptableObject
{
    public enum ItemType{ Coin, EXP, Buff, Heal, EnemyCleaner } //������ ���� ����
    public enum BuffEffect { Power, Speed, Defense, Magnetic, Invincible, None } // ���� ������ ȿ�� ����

    [Header("# Main Info")]
    public ItemType itemType; // ������ ����
    public int itemId; // ������ Id - ��� �� �� �����ѵ� Ȥ�� ���� �־��
    public Sprite image; // �̹���-�ʿ� ǥ�õ� �̹���
    public bool magnetable; //�ڷ� ���� ����

    [Header("# value")]
    public int value; //�� (���ξ�, ����ġ��, ����, ���� ��ġ ���)
    public int dropRate; // �����


    [Header("# Buff Item Info")]
    public BuffData buff; //���� �����͸� ����ִ� ScriptableObject
    /*
    public BuffEffect buffType;//����ȿ�� ����
    public float duration;//���� ���ӽð� ����
    */
    [Header("# Prefab(GameObject)")]
    public GameObject prefab;//������ - Coin���� ������
    //Coin ���ο��� �ڼ�ȿ���� ���� Rigidbody2D�� Magnetable(�ڼ� ������ ����) ��ũ��Ʈ, �ڼ�ȿ���� ������ ������ �����ϴ� CircleCollider2D�� ������.
    //Tag�� Item���� �����Ǿ��ִ�.


    

}
