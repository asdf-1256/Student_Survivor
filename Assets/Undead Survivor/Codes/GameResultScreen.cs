using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameResultScreen : MonoBehaviour
{
    public void Home()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
}
