using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    CircleCollider2D circleCollider;

    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
    }

    //Stay로 변경
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Coin"))
            return;

        //거리계산
        if(( GameManager.Instance.player.transform.position - collision.gameObject.transform.position ).magnitude < 0.5f){
            GameManager.Instance.money++;

            Debug.Log("코인과 충돌 발생");

            collision.gameObject.SetActive(false);
        }
        else
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce((GameManager.Instance.player.transform.position - collision.gameObject.transform.position).normalized * 2f, ForceMode2D.Force);
        }
    }

    private void LevelUpColliderRadius()
    {
        circleCollider.radius *= 2f;
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
