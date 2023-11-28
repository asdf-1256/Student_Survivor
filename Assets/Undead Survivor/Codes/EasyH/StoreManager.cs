using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public static StoreManager instance;
    public List<StoreData> JoinedDongAris;

    private void Awake()
    {
        instance = this;
        JoinedDongAris = new List<StoreData>();
    }
    public void SelectDongAri(StoreData storeData)
    {
        Debug.Log("돈 충분함!");
        JoinedDongAris.Add(storeData);
        DataManager.Instance.SubMoney(storeData.price);
    }
    public void CancelDongAri(StoreData storeData)
    {
        Debug.Log("돈 롤백함");
        DataManager.Instance.AddMoney(storeData.price);
        JoinedDongAris.Remove(storeData);
    }

    public void ApplyAllDongAri()
    {
        foreach (var DongAri in JoinedDongAris)
        {
            ApplyDongAri(DongAri);
        }
    }
    public void ApplyDongAri(StoreData DongAri)
    {
        switch (DongAri.Type)
        {
            case StoreData.DongAriType.EXPERT:
                GameManager.Instance.player.shield.AddShield();
                Debug.Log("쉴드 추가");
                break;
            case StoreData.DongAriType.SSOS:
                GameManager.Instance.player.GetComponentInChildren<Magnet>().MagneticRate *= DongAri.degree;
                Debug.Log("자석 범위 증가");
                break;
            case StoreData.DongAriType.PNC:
                GameManager.Instance.maxHealth += DongAri.degree;
                GameManager.Instance.health += Convert.ToInt32(DongAri.degree);
                GameManager.Instance.HealthInHUD.GetComponent<RectTransform>().sizeDelta = new Vector2(GameManager.Instance.maxHealth / 10, 4);
                Debug.Log("최대 체력 증가");
                break;
        }
    }
}
