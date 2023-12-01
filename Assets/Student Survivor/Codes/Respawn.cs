using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public GameObject reviveUI; // ��� â(��Ȱ ��ư ���� �� ����)
    public GameObject gameoverUI; // ��� â
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


