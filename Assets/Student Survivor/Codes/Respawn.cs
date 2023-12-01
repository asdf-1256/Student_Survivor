using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public GameObject reviveUI; // 사망 창(부활 버튼 누를 수 있음)
    public GameObject gameoverUI; // 결과 창
    public GameObject player;

    private bool revived;

    public void YouWantToRevive()
    {
        if(revived)
        {
            GameOver();
        }
        else
        {
            reviveUI.SetActive(true);
        }
    }
    
    public void RevivePlayer() { 
        revived = true;
        player.transform.position = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        player.SetActive(true);
    }

    public void GameOver()
    {
        reviveUI.SetActive(false); 
        gameoverUI.SetActive(true);
    }

    public void GetARevive()
    {
        reviveUI.SetActive(false);
        RevivePlayer();
    }
}


