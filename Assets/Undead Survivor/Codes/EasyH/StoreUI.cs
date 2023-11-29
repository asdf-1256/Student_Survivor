using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreUI : MonoBehaviour
{
    public StoreData storeData;
    public bool isSelected;
    public bool isBought;

    Image image;

    [SerializeField] Color pressed;
    [SerializeField] Color notPressed;
    [SerializeField] Color locked;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
    public void OnClickDongAri()
    {
        if (isSelected)
        {
            StoreManager.instance.CancelDongAri(storeData);
            isSelected = false;
        }
        else if (DataManager.Instance.CheckMoney(storeData.price))
        {
            StoreManager.instance.SelectDongAri(storeData);
            isSelected = true;
        }
        else
        {
            UIManager.Instance.Notice("돈 충분하지 못함!");
        }
        ChangeButtonColor();
    }
    void ChangeButtonColor()
    {
        if (isSelected)
            image.color = pressed;
        else
            image.color = notPressed;
    }
    
    public void OnClickSkin()
    {
        if(isBought)
        {
            StoreManager.instance.ApplySkin(storeData.degree);
        }
    }
}
