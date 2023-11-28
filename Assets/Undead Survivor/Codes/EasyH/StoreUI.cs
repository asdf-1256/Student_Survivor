using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreUI : MonoBehaviour
{
    public StoreData storeData;
    public bool isSelected;

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
    }
    
}
