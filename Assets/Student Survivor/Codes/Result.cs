using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    public GameObject[] titles;

    public Text gameResultScreenTitleText;
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
        titles[1].transform.parent.GetComponentInChildren<Button>().gameObject.SetActive(false);
        titles[1].transform.parent.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = "메인으로";
        gameResultScreenTitleText.text = "성적 우수";
    }
    private void OnDisable()
    {
        foreach (var title in titles)
        {
            title.SetActive(false);
        }
    }
}
