using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//맵에 스폰되는 아이템을 ScriptableObject로 만들어, Spawner의 itemDatas 배열에 할당함.
//여기에 드롭율 추가 고려

//해당 ScriptableObject로 생성된 데이터는 Data/SpawnItemData에 있음.


[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/SpawnItemData")]
public class SpawnItemData : ScriptableObject
{
    public enum ItemType{ Coin, EXP, Buff, Heal } //아이템 종류 구분
    public enum BuffEffect { Power, Speed, Defense, Magnetic, Invincible, None } // 버프 아이템 효과 구분

    [Header("# Main Info")]
    public ItemType itemType; // 아이템 종류
    public int itemId; // 아이템 Id - 없어도 될 것 같긴한데 혹시 몰라서 넣어둠
    public Sprite image; // 이미지-맵에 표시될 이미지
    public bool magnetable; //자력 영향 여부

    [Header("# value")]
    public int value; //값 (코인양, 경험치량, 힐양, 버프 수치 등등)
    public int dropRate; // 드롭율

    [Header("# Buff Item Info")]
    public BuffEffect buffType;//버프효과 설정
    public float duration;//버프 지속시간 설정

    [Header("# Prefab(GameObject)")]
    public GameObject prefab;//프리팹 - Coin으로 동일함
    //Coin 내부에는 자석효과를 위한 Rigidbody2D와 Magnetable(자석 움직임 구현) 스크립트, 자석효과와 아이템 습득을 구현하는 CircleCollider2D가 있으며.
    //Tag는 Item으로 설정되어있다.


    

}
