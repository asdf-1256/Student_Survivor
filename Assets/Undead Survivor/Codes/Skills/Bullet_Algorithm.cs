using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Algorithm : MonoBehaviour
{
    public float rotateSpeed = 100f;
    public float lifeTime = 3f;
    public float damage = 10f;


    Collider2D coll;
    float timer;


    private void Awake()
    {
        coll = GetComponent<Collider2D>();
    }


    IEnumerator RotateRoutine()
    {
        Quaternion targetRotation = Quaternion.Euler(0, 0, 90f);
        float rotationThreshold = 0.01f;
        // 타켓회전각도와의 차이가 0.01 이하일 때까지 반복
        while (Quaternion.Angle(transform.rotation, targetRotation) > rotationThreshold)
        {
            // deltaTime을 곱해 프레임이 달라도 같은 속도 유지
            float step = rotateSpeed * Time.deltaTime;
            // 현재 각도에서 타겟회전각까지 step만큼 회전
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);
            yield return null; // 다음 프레임까지 양보
        }
        yield return StartCoroutine(MainRoutine(() =>
        {
            gameObject.SetActive(false);
        } ));
    }

    IEnumerator MainRoutine(System.Action done) // LifeTime동안 그냥 타이머 재는 코루틴
    {
        coll.enabled = true; // collider 활성화
        while (timer < lifeTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        timer = 0f;
        done.Invoke(); // 이후 비활성화 함수 호출
    }

    private void OnEnable()
    {
        StartCoroutine(RotateRoutine()); // 해당 오브젝트가 On 될 때마다 실행
    }
    private void OnDisable()
    {
        coll.enabled = false;
        transform.rotation = Quaternion.identity;
        StopAllCoroutines();
    }

}
