using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;

    bool isLive;

    Rigidbody2D rigid;
    Collider2D coll;
    Animator anim;
    SpriteRenderer spriteRenderer;
    WaitForFixedUpdate wait;

    //좀비가 죽을 때 드롭할 경험치 데이터를 Inspector상에서 연결함.(Spawner에 있는 것과 동일한 파일)
    public SpawnItemData expData;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!GameManager.Instance.isLive)
            return;

        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;

        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }
    private void LateUpdate()
    {
        if (!GameManager.Instance.isLive)
            return;

        if (!isLive)
            return;

        spriteRenderer.flipX = target.position.x < rigid.position.x;
    }
    private void OnEnable()
    {
        target = GameManager.Instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        spriteRenderer.sortingOrder = 2;
        anim.SetBool("Dead", false);
        health = maxHealth;
    }

    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());

        if (health > 0) {
            //.. 살았고 피격판정
            //애니메이션, 넉백
            anim.SetTrigger("Hit");
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.Hit);
        }
        else
        {
            //.. 죽음
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriteRenderer.sortingOrder = 1;
            anim.SetBool("Dead", true);
            GameManager.Instance.kill++;
            GameManager.Instance.GetExp();
            DropExp();

            if (GameManager.Instance.isLive)
                AudioManager.Instance.PlaySfx(AudioManager.Sfx.Dead);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)//장판위에 있을 때 호출되도록 할 예정. 웹프 기능인 속도 바꾸는 기능도 여기서 처리해야할듯.
    {

        //.. 장판 공격은 공통 태그 Lava로 둘까 했는데 collision에서 어떤 컴퍼넌트인지 비교하는 법을 모르겠따
        if (!isLive) //일단 장판을 Tag:Lava로 구분하지만, 더 좋은 단어가 없을지 생각
            return;

        float damage = 0;
        if (collision.CompareTag("Lava"))
        {
            SkillBase skillBase = collision.GetComponent<SkillBase>();
            damage = skillBase.data.damages[skillBase.GetLevel()];
        }
        else if (collision.CompareTag("RB_Tree"))
        {
            SkillBase skillBase = collision.GetComponent<SkillBase>();
            damage = skillBase.data.damages[skillBase.GetLevel()];
        }
            //damage = collision.GetComponent<Bullet_Algorithm>().damage;
        else if (collision.CompareTag("OS_Explosion"))
            damage = collision.GetComponentInParent<Bullet_OS>().damage; // 컬리전 대상은 Bullet_OS의 자식 오브젝트임
        else
            return;

        Debug.Log("damage는 " + damage);
        health -= damage * Time.deltaTime;//데미지를 받아와서 프레임마다 데미지 계산.
        //피격 효과를 제거하고 죽었는지 아닌지만 확인.
        if (health <= 0)
        {
            //.. 죽음
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriteRenderer.sortingOrder = 1;
            anim.SetBool("Dead", true);
            GameManager.Instance.kill++;
            GameManager.Instance.GetExp();
            DropExp();

            if (GameManager.Instance.isLive)
                AudioManager.Instance.PlaySfx(AudioManager.Sfx.Dead);
        }
    }

    void DropExp()
    {
        int tmp = Random.Range(0, 100);
        if (tmp > 20) // 확률적으로 Exp 드롭
        {
            return;
        }
        GameObject Exp = GameManager.Instance.pool.Get(3);
        Exp.GetComponent<SpawnItem>().Init(expData);
        Exp.transform.position = new Vector2(transform.position.x, transform.position.y);
        //Debug.Log("@경험치 드랍됨");

    }

    //코루틴 - 비동기
    IEnumerator KnockBack()
    {

        //yield - 코루틴 반환
        //yield return null; // 1프레임 쉬기
        
        //yield return new WaitForSeconds(2f);//2초 쉬기 - new 계속하면 성능문제

        //하나의 물리 프레임을 딜레이할 것
        yield return wait;

        Vector3 playerPos = GameManager.Instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
        //순간적인 힘 - Impulse


    }
    void Dead()
    {
        gameObject.SetActive(false);
    }
}