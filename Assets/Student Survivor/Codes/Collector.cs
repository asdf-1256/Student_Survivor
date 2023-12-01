using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    //아이템 습득을 담당
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Item"))
            return;

        SpawnItemData data = collision.GetComponent<SpawnItem>().data;//충돌한 아이템의 데이터를 받아온다.

        //아이템의 itemType에 따라 분류하고, 해당하는 효과를 발휘한다.(value만큼 증가한다.)
        switch (data.itemType)
        {
            case SpawnItemData.ItemType.Coin:
                //Debug.Log(string.Format("코인 {0}만큼 먹음", data.value));
                //DataManager.Instance.AddMoney(data.value);
                DataManager.Instance.AddMoney(Random.Range(1,data.value));
                break;
            case SpawnItemData.ItemType.EXP:
                //Debug.Log(string.Format("경험치 {0}만큼 먹음", data.value));
                GameManager.Instance.GetExp(data.value);
                break;
            case SpawnItemData.ItemType.Buff:
                GameManager.Instance.player.ActivateBuff(data.buff);
                //Debug.Log("버프 아이템 먹음");
                break;
            case SpawnItemData.ItemType.Heal:
                //Debug.Log("힐 아이템 먹음");
                GameManager.Instance.GetHealth(data.value);
                break;
            case SpawnItemData.ItemType.EnemyCleaner:
                GameManager.Instance.ActiveEnemyCleaner();
                break;
        }

        collision.gameObject.SetActive(false);//이후 아이템을 없앤다.
    }
}
