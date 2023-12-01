using UnityEngine;
using UnityEngine.SceneManagement;

public class Stop_Menu : MonoBehaviour
{

    [SerializeField] GameObject stopMenu; // UI Hierarchy�� �ִ�
    [SerializeField] GameObject joy;


    public void Stop()
    {
        stopMenu.SetActive(true);
        Time.timeScale = 0;
        joy.GetComponent<RectTransform>().localScale = Vector3.zero;
        Debug.Log("�Ͻ����� ��ư�� ���Ƚ��ϴ�.");
    }

    public void Home()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void Resume()
    {
        stopMenu.SetActive(false);
        Time.timeScale = 1;
        joy.GetComponent<RectTransform>().localScale = Vector3.one;
    }

}
