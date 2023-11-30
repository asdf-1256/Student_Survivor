using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class Retry_Button : MonoBehaviour
{

    public DataManager data;

    private void Start()
    {
        UpdateButtonInteractable();
    }

    private void UpdateButtonInteractable()
    {
        if (DataManager.Instance.money <= 15)
        {
            GetComponent<Button>().interactable = false;
            Debug.Log("돈이 부족합니다.");
        }
        else
        {
            GetComponent<Button>().interactable = true;
            
        }
    }

    public void OnClick()
    {
        if (DataManager.Instance.CheckMoney(15))
        {
            DataManager.Instance.SubMoney(15);
            data.Save();
        }
        else
        {

        }
    }
}
