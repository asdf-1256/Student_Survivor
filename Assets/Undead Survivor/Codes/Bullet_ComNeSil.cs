using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_ComNeSil : MonoBehaviour
{
    public float scaleFactor = 0.5f;
    public float speed = 15;

    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        InvokeRepeating("ScaleUp", 0.1f, 0.1f);  // 0.1f초마다 함수 실행, 얘는 비활성화 상태에서도 계속 반복함
    }

    public void Init(Vector3 dir)
    {
        rigid.velocity = dir * speed;
    }

    /*
    private void Update() // 총알 하나하나에 타이머
    {
        timer += Time.deltaTime;

        if (timer > 5f)
        {
            timer = 0f;
            transform.localScale = new Vector3(2f, 2f, 0f);
            gameObject.SetActive(false);
        }
    }*/
    private void OnTriggerExit2D(Collider2D collision) // 거리보다 시간을 기준으로 비활성화가 나은 듯
    {
        if (!collision.CompareTag("Area"))
            return;
        transform.localScale = new Vector3(2f, 2f, 0f);
        gameObject.SetActive(false);
    }
    private void ScaleUp()
    {
        if (!gameObject.activeSelf)
            return;
        Vector3 newScale = transform.localScale + new Vector3(scaleFactor, scaleFactor, 0f);
        transform.localScale = newScale;
    }
}
