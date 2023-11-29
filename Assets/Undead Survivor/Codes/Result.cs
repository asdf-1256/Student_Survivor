using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour
{
    public GameObject[] titles;
    /*
    private void OnEnable()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }*/
    public void Lose()
    {
        titles[0].SetActive(true);
    }
    public void Win()
    {
        titles[1].SetActive(true);
    }
    private void OnDisable()
    {
        foreach (var title in titles)
        {
            title.SetActive(false);
        }
    }
}
