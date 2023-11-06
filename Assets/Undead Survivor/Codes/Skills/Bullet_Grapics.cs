using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Grapics : BulletBase
{
    public float spawnDistance = 10;

    GameObject selectedPolygon;

    private void Awake()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);
    }
    private void OnEnable()
    {

        Vector2 randomCircle = Random.insideUnitCircle; // 원 내의 한 점
        Vector3 spawnPosition = new Vector3(randomCircle.x, randomCircle.y, 0);


        transform.position = GameManager.Instance.player.transform.position + spawnPosition * spawnDistance; // 캐릭터 중심으로 반지름 10인 원 내의 한 점
        // transform.rotation = Random.rotation; // 랜덤 회전

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
