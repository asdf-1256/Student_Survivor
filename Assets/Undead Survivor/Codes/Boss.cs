using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] int bossId;

    [SerializeField] private SpawnItemData[] itemDatas;
    [SerializeField] private int spawnExpCount = 100;

    [SerializeField] private GameObject bossHealthHUD;

    private void OnEnable()
    {
        bossHealthHUD.SetActive(true);
    }

    private void OnDisable()
    {
        bossHealthHUD.SetActive(false);
        GameManager.Instance.SpawnedBoss = null;

        if (bossId == 0)
        {
            Spawner spawner = GetComponentInChildren<Spawner>();

            for(int i = 0;i < spawnExpCount; i++)
            {
                spawner.SpawnItem(itemDatas[Random.Range(0, itemDatas.Length)]);
            }
        }
        else if (bossId == 1)
            GameManager.Instance.GameVictory();
    }

}
