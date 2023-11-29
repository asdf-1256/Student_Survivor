using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreUI : MonoBehaviour
{
    public StoreData storeData;
    public bool isSelected;

    Image image;

    [SerializeField] Color pressed;
    [SerializeField] Color notPressed;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
    public void OnClick()
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
    
}
