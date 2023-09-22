using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    CircleCollider2D coll;

    private void Awake()
    {
        coll = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            GameManager.Instance.money++;

            Debug.Log("코인과 충돌 발생");

            collision.gameObject.SetActive(false);
        }
        else if (collision.CompareTag("Exp 0")) {
            GameManager.Instance.GetExp();

            Debug.Log("경험치와 충돌 발생");

            collision.gameObject.SetActive(false);
        }
        else if (collision.CompareTag("Exp 1"))
        {
            GameManager.Instance.GetExp(10);

            Debug.Log("메가경험치 충동");

            collision.gameObject.SetActive(false);
        }
        else if (collision.CompareTag("Health"))
        {
            GameManager.Instance.GetHealth(10); // 10만큼 체력 회복

            Debug.Log("체력 충동");

            collision.gameObject.SetActive(false);
        }
        else if (collision.CompareTag("Mag")) // 일단은 영구 증가?
        {

            StartCoroutine(CreateMagRoutine());


            collision.gameObject.SetActive(false);
        }

    }

    IEnumerator CreateMagRoutine() // 10초 동안 반지름 10 증가
    {
        Debug.Log("자석자석");

        coll.radius += 10;
        yield return new WaitForSeconds(10);
        coll.radius -= 10;
        Debug.Log("자석 효과 끝");
    }

    private void LevelUpColliderRadius()
    {
        coll.radius *= 2f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space Key Input 발생");
            LevelUpColliderRadius();
        }
    }
}
