using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Player : MonoBehaviour
{
    public float speed;
    public float playerDistance;
    Rigidbody2D rigid;
    Rigidbody2D playerRigid;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        playerRigid = GameManager.Instance.player.GetComponent<Rigidbody2D>();
        transform.position = GameManager.Instance.player.transform.position;
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.isLive)
            return;
        if (Mathf.Abs(rigid.position.x - playerRigid.position.x) > playerDistance
            || Mathf.Abs(rigid.position.y - playerRigid.position.y) > playerDistance)
        {
            Vector2 dirVec = playerRigid.position - rigid.position;
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + nextVec);
            rigid.velocity = Vector2.zero;
        }
    }

    private void LateUpdate()
    {

        spriteRenderer.flipX = playerRigid.position.x < rigid.position.x;
    }
}
