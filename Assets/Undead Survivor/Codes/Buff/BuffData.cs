using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffData", menuName = "Scriptable Object/BuffData")]
public class BuffData : ScriptableObject
{
    public enum BuffEffect { Power, Speed, Defense, Magnetic, Invincible } //버프효과 목록

    public BuffEffect effect;
    public Sprite image;
    public int id;
    public float value;
    public float duration;//버프의 지속시간 정보

    private float remainTime; //남은 지속시간 계산용

    private void Awake()
    {
        remainTime = duration;
    }

    public void ResetTime()//버프 중복 획득 시 시간 갱신하는 용도
    {
        remainTime = duration;
    }
    public float GetRemainTime()//남은 지속시간 계산용 - getter
    {
        return remainTime;
    }
    public void SetRemainTime(float remainTime)//남은 지속시간 계산용 - getter
    {
        this.remainTime = remainTime;
    }


    //id만으로 동일 객체 여부 확인
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