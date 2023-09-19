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
        if (!collision.CompareTag("Coin"))
            return;

        GameManager.Instance.money++;

        Debug.Log("코인과 충돌 발생");

        collision.gameObject.SetActive(false);
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
