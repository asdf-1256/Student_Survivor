using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_OS : MonoBehaviour
{

    [SerializeField]
    Sprite[] sprites; // 0:로봇 이미지, 1: 일단 커피 이미지

    public float duration;
    public float damage;

    Collider2D[] colls; // 자식 오브젝트에 있는 콜라이더들
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;

    private void Awake()
    {
        colls = GetComponentsInChildren<Collider2D>(); // 0:OS모드, 1:폭발모드
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, Vector3 dir)
    {
        this.damage = damage;
        
        rigid.velocity = dir * 1f; // 속도 일단 1로 둬. 하드코딩
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
            return;
        rigid.velocity = Vector2.zero;

        spriteRenderer.sprite = sprites[1];//이미지를 일단 커피로 변경 -> 추후 폭발로 바꿔야 함


        StartCoroutine(ExplosionRoutine(() => { gameObject.SetActive(false); }));
        
    }
    /*private void OnTriggerExit2D(Collider2D collision) // 이거 없으니까 되네
    {
        if (!collision.CompareTag("Area"))
            return;
        Debug.Log("OS 로봇 밖으로 나감");
        gameObject.SetActive(false);
    }*/
    private void OnDisable()
    {

        spriteRenderer.sprite = sprites[0];
        transform.position = new Vector3(0, 0, 0);
        // transform.rotation = Quaternion.identity;
    }
    IEnumerator ExplosionRoutine(System.Action done)
    {
        colls[1].enabled = true;
        colls[0].enabled = false;

        float timer = 0f;
        while (timer <= duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        colls[0].enabled = true;
        colls[1].enabled = false;

        done.Invoke();
    }
}
