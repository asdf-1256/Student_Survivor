using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Grapics : MonoBehaviour
{
    public float lifeTime;

    GameObject selectedPolygon;

    public void Init(float lifeTime)
    {
        this.lifeTime = lifeTime;
    }
    private void OnEnable()
    {
        int selectedChildNum = Random.Range(0, 4); // 0~4 중 자식 한 명 선택함
        selectedPolygon = transform.GetChild(selectedChildNum).gameObject;
        selectedPolygon.SetActive(true); // 오브젝트 하나만 활성화

        StartCoroutine(TimerRoutine()); // 타이머 코루틴 실행
    }

    IEnumerator TimerRoutine() // lifeTime 후에 비활성화하는 코루틴
    {
        float timer = 0f;
        while (timer < lifeTime)
        {
            timer += Time.deltaTime;

            yield return null;
        }
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        selectedPolygon.SetActive(false); // 스킬 종료되면 다시 비활성화
    }
}
