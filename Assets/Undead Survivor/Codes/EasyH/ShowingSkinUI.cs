using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShowingSkinUI : MonoBehaviour
{
    public SkinData showingSkinData;

    public Image Icon;
    public Text Name;
    public Text Price;
    public Text Desc;
    public GameObject BuyButtonObject;

    public void UpdateSkin(SkinData skinData, bool isBought)
    {
        if (isBought)
            BuyButtonObject.SetActive(false);
        else
            BuyButtonObject.SetActive(true);
        showingSkinData = skinData;
        Icon.sprite = skinData.Icon;
        Name.text = skinData.Name;
        Price.text = skinData.price.ToString();
        Desc.text = skinData.Descript;
    }
    public void OnClickBuy()
    {
        StoreManager.instance.BuySkin(showingSkinData);
        StoreManager.instance.DecisionAllSkinUI(showingSkinData.SkinID);
    }
}
