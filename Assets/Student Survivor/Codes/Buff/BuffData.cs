using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffData", menuName = "Scriptable Object/BuffData")]
public class BuffData : ScriptableObject
{
    public enum BuffEffect { ���ݷ�, �̵��ӵ�, ����, �ڱ��, ���� } //����ȿ�� ���

    public BuffEffect effect;
    public Sprite image;
    public int id;
    public float value;
    public float duration;//������ ���ӽð� ����

    private float remainTime; //���� ���ӽð� ����

    private void Awake()
    {
        remainTime = duration;
    }

    public void ResetTime()//���� �ߺ� ȹ�� �� �ð� �����ϴ� �뵵
    {
        remainTime = duration;
    }
    public float GetRemainTime()//���� ���ӽð� ���� - getter
    {
        return remainTime;
    }
    public void SetRemainTime(float remainTime)//���� ���ӽð� ���� - getter
    {
        this.remainTime = remainTime;
    }


    //id������ ���� ��ü ���� Ȯ��
    public override bool Equals(object other)
    {
        if (other == null || GetType() != other.GetType())
            return false;
        BuffData otherBuffData = other as BuffData;
        return this.id == otherBuffData.id;
    }
    public override int GetHashCode()
    {
        return id.GetHashCode();
    }
}