using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DongAriStoreUI : MonoBehaviour
{
    public DongAriData dongAriData;
    public bool isSelected;
    public Image Icon;
    public Text Name;
    public Text Desc;
    public Text Cost;

    Image image;

    [SerializeField] Color pressed;
    [SerializeField] Color notPressed;

    private void Awake()
    {
        image = GetComponent<Image>();
        Icon.sprite = dongAriData.Icon;
        Name.text = dongAriData.Name;
        Desc.text = dongAriData.Descript;
        Cost.text = dongAriData.price.ToString();

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
            UIManager.Instance.Notice("�� ������� ����!");
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
