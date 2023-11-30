using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public static StoreManager instance;
    public List<DongAriData> JoinedDongAris;
    public int PlayerSkinID;

    public ShowingSkinUI showingSkinUI;
    public SkinStoreUI[] skinStoreUIs;
    private void Awake()
    {
        instance = this;
        JoinedDongAris = new List<DongAriData>();
    }
    public void SelectDongAri(DongAriData dongAriData)
    {
        Debug.Log("돈 충분함!");
        JoinedDongAris.Add(dongAriData);
        DataManager.Instance.SubMoney(dongAriData.price);
    }
    public void CancelDongAri(DongAriData storeData)
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
    public void ApplyDongAri(DongAriData DongAri)
    {
        switch (DongAri.dongAriType)
        {
            case DongAriData.DongAriType.EXPERT:
                GameManager.Instance.player.shield.AddShield();
                Debug.Log("쉴드 추가");
                break;
            case DongAriData.DongAriType.SSOS:
                GameManager.Instance.player.GetComponentInChildren<Magnet>().MagneticRate *= DongAri.degree;
                Debug.Log("자석 범위 증가");
                break;
            case DongAriData.DongAriType.PNC:
                GameManager.Instance.maxHealth += DongAri.degree;
                GameManager.Instance.health += Convert.ToInt32(DongAri.degree);
                GameManager.Instance.HealthInHUD.GetComponent<RectTransform>().sizeDelta = new Vector2(GameManager.Instance.maxHealth / 10, 4);
                Debug.Log("최대 체력 증가");
                break;
        }
    }

    public bool isBought(int skinID)
    {
        return DataManager.Instance.isUnLockedSkins[skinID];
    }
    public void ApplySkin(int playerSkinID)
    {
        PlayerSkinID = playerSkinID;
        DataManager.Instance.SetSelectedSkinID(playerSkinID);
    }
    public void ShowSkinUI(SkinData skinData, bool isBought)
    {
        showingSkinUI.UpdateSkin(skinData, isBought);
    }
    public void BuySkin(SkinData skinData)
    {
        if (DataManager.Instance.CheckMoney(skinData.price))
        {
            Debug.Log(skinData.SkinID + "번 스킨 구매함");
            DataManager.Instance.SubMoney(skinData.price);
            DataManager.Instance.UnlockSkin(skinData.SkinID);


            ApplySkin(skinData.SkinID);
            UIManager.Instance.Notice(skinData.Name + " 스킨 구매함");
        }
        else
        {
            UIManager.Instance.Notice("돈 충분하지 못함!");
        }
    }
    public void DecisionAllSkinUI(int skinID)
    {
        foreach (SkinStoreUI skinStoreUI in skinStoreUIs)
        {
            if (skinStoreUI.skinData.SkinID == skinID)
            {
                skinStoreUI.UnLockSkin();
            }
        }
    }
}
