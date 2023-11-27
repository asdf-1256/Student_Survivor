using UnityEngine;
using UnityEngine.SceneManagement;

public class Stop_Menu : MonoBehaviour
{

    [SerializeField] GameObject stopMenu; // UI Hierarchy에 있는
    [SerializeField] GameObject joy;


    public void Stop()
    {
        stopMenu.SetActive(true);
        Time.timeScale = 0;
        joy.GetComponent<RectTransform>().localScale = Vector3.zero;
        Debug.Log("일시정지 버튼이 눌렸습니다.");
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
