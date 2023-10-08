using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_OS : MonoBehaviour
{

    [SerializeField]
    Sprite[] sprites; // 0:로봇 이미지, 1: 일단 커피 이미지

    public float duration;
    public float damage;
    public float speed;

    Collider2D[] colls; // 자식 오브젝트에 있는 콜라이더들
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;
    Scanner scanner;
    Transform target;

    bool isExplosion;

    private void Awake()
    {
        colls = GetComponentsInChildren<Collider2D>(); // 0:OS모드, 1:폭발모드
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        scanner = GetComponent<Scanner>();
        isExplosion = false;
    }

    public void Init(float damage, float speed)
    {
        this.damage = damage;
        this.speed = speed;
        
    }


    private void FixedUpdate()
    {
        if (isExplosion) // 적과 만나 폭발했으면 이동X
            return;
        if (!scanner.nearestTarget)
            return;

        target = scanner.nearestTarget;// OS 객체의 scanner를 통해 최단거리 적 찾아감
        Vector3 dirVec = target.position - transform.position;
        Vector3 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(transform.position + nextVec);
        rigid.velocity = Vector2.zero;
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
            return;
        rigid.velocity = Vector2.zero;

        spriteRenderer.sprite = sprites[1];//이미지를 일단 커피로 변경 -> 추후 폭발로 바꿔야 함
        
        isExplosion = true; // 폭발했음으로 바꿈

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
        isExplosion = false;
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
