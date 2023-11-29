using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinStoreUI : MonoBehaviour
{
    public SkinData skinData;
    public bool isBought;

    Image image;

    [SerializeField] Color Locked;
    [SerializeField] Color Unlocked;

    private void Awake()
    {
        image = GetComponent<Image>();
        isBought = StoreManager.instance.isBought(skinData.SkinID);
        DecisionSkinLock();
    }
    
    public void OnClick()
    {
        DecisionSkinLock();
        if(isBought)
            StoreManager.instance.ApplySkin(skinData.SkinID);
        StoreManager.instance.ShowSkinUI(skinData, isBought);
    }
    public void DecisionSkinLock()
    {
        if (isBought)
            image.color = Unlocked;
        else
            image.color = Locked;
    }
    public void UnLockSkin()
    {
        isBought = true;
        image.color = Unlocked;
        OnClick();
    }
}
