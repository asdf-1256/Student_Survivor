using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End_Button : MonoBehaviour
{
    public void End()
    {
        SceneManager.LoadScene(0);
    }
}
