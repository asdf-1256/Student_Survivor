using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    //������ ������ ���
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Item"))
            return;

        SpawnItemData data = collision.GetComponent<SpawnItem>().data;//�浹�� �������� �����͸� �޾ƿ´�.

        //�������� itemType�� ���� �з��ϰ�, �ش��ϴ� ȿ���� �����Ѵ�.(value��ŭ �����Ѵ�.)
        switch (data.itemType)
        {
            case SpawnItemData.ItemType.Coin:
                //Debug.Log(string.Format("���� {0}��ŭ ����", data.value));
                //DataManager.Instance.AddMoney(data.value);
                DataManager.Instance.AddMoney(Random.Range(1,data.value));
                break;
            case SpawnItemData.ItemType.EXP:
                //Debug.Log(string.Format("����ġ {0}��ŭ ����", data.value));
                GameManager.Instance.GetExp(data.value);
                break;
            case SpawnItemData.ItemType.Buff:
                GameManager.Instance.player.ActivateBuff(data.buff);
                //Debug.Log("���� ������ ����");
                break;
            case SpawnItemData.ItemType.Heal:
                //Debug.Log("�� ������ ����");
                GameManager.Instance.GetHealth(data.value);
                break;
            case SpawnItemData.ItemType.EnemyCleaner:
                GameManager.Instance.ActiveEnemyCleaner();
                break;
        }

        collision.gameObject.SetActive(false);//���� �������� ���ش�.
    }
}
