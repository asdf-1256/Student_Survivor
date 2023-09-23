using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//맵에 스폰되는 아이템 prefab에 들어가는 스크립트
//prefab은 Coin 하나만을 사용했음
public class SpawnItem : MonoBehaviour
{
    public SpawnItemData data;
    SpriteRenderer spriteRenderer; //이미지 변경을 위한 SpriteRenderer
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    //Spawner에서 아이템을 스폰할 때, Init함수를 호출함.
    public void Init(SpawnItemData data)
    {
        this.data = data;//Spawner에 있는 itemDatas 배열에 있는 데이터(Inspector상에서 보면 아이템의 Scriptable Object 데이터가 들어가있음) 배열에서 데이터를 받아와 설정
        spriteRenderer.sprite = data.image;//아이템에 맞게 이미지 변경
        GetComponent<MagnetableItem>().enabled = data.magnetable;//자석의 영향을 받지않는 버프 아이템들은
        //자석같은 움직임을 담당하는 MagnetableItem 스크립트를 비활성화 시켜 움직이지 않도록 함.
    }
    private void OnDisable()
    {
        GetComponent<MagnetableItem>().enabled = false;
    }
}
