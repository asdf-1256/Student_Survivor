using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] GameObject[] shields;

    public int ShieldCount
    {
        get
        {
            int count = 0;
            foreach (var shield in shields)
                if (shield.activeSelf == true)
                    count++;
            return count;
        }
    }

    public void AddShield()
    {
        foreach (var shield in shields)
            if (shield.activeSelf == false)
            {
                shield.SetActive(true);
                return;
            }
    }
    public void RemoveShield()
    {
        for (int i = shields.Length - 1; i >= 0; i--)
            if (shields[i].activeSelf == true)
            {
                shields[i].SetActive(false);
                return;
            }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow)) {
            AddShield();
        }
    }
}
