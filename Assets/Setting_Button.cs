using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting_Button : MonoBehaviour
{
 public GameObject settingObject;  // Setting 오브젝트를 저장할 변수

    public void OnButtonClick()
    {
        // Setting 오브젝트가 존재하는지 확인
        if (settingObject != null)
        {
            // Setting 오브젝트의 활성화 여부를 반전시킴
            settingObject.SetActive(!settingObject.activeSelf);
        }
    }
}
