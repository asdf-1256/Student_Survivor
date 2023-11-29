using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DongAriStoreUI : MonoBehaviour
{
    public DongAriData dongAriData;
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
            StoreManager.instance.CancelDongAri(dongAriData);
            isSelected = false;
        }
        else if (DataManager.Instance.CheckMoney(dongAriData.price))
        {
            StoreManager.instance.SelectDongAri(dongAriData);
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
