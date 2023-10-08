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
        //if (!collision.CompareTag("Bullet")||!isLive)
        //  return;
        if (!isLive) return;

        if (collision.CompareTag("Bullet"))
        {
            health -= collision.GetComponent<Bullet>().damage;
            if (health > 0)
            {
                //.. 살았고 피격판정
                //애니메이션, 넉백
                anim.SetTrigger("Hit");
                AudioManager.Instance.PlaySfx(AudioManager.Sfx.Hit);
                StartCoroutine(KnockBack());
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

        else if (collision.CompareTag("Laptop"))
        {
            health -= collision.GetComponentInParent<Bullet_Cloud>().damage;
            if (health > 0)
            {
                //.. 살았고 피격판정
                //애니메이션, 넉백
                anim.SetTrigger("Hit");
                AudioManager.Instance.PlaySfx(AudioManager.Sfx.Hit);
                StartCoroutine(KnockBack());
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


        else if (collision.CompareTag("Lava"))
            StartCoroutine(LavaRoutine(collision.GetComponent<SkillBase>()));
        else if (collision.CompareTag("Web"))
            speed /= 3f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isLive) return;

        if (collision.gameObject.CompareTag("Machine"))
            StartCoroutine(MomBBangRoutine(collision.gameObject));
    }
    IEnumerator MomBBangRoutine(GameObject targetObject)
    {
        while (true)
        {
            health -= targetObject.GetComponent<Bullet_MachhineLearning>().damage;
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
                break;
            }
            else
            {
                //.. 살았고 피격판정
                //애니메이션, 넉백
                anim.SetTrigger("Hit");
                AudioManager.Instance.PlaySfx(AudioManager.Sfx.Hit);
            }
            yield return new WaitForSeconds(1f);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        StopCoroutine("MomBBangRoutine");
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Lava"))
            StopAllCoroutines();
        else if (collision.CompareTag("Web"))
            speed *= 3f;
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
    IEnumerator LavaRoutine(SkillBase skillBase)
    {
        while (true)
        {
            health -= skillBase.data.damages[skillBase.GetLevel()] * 0.5f;
            Debug.Log(string.Format("데미지 {0} 루틴 발동", skillBase.data.damages[skillBase.GetLevel()]));
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
                break;
            }
            else
            {
                //.. 살았고 피격판정
                //애니메이션, 넉백
                anim.SetTrigger("Hit");
                AudioManager.Instance.PlaySfx(AudioManager.Sfx.Hit);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
<<<<<<< HEAD
    public IEnumerator LockRoutine(float duration, System.Action done)
    {
        HEAD
        float tempSpeed = speed;

        speed = 0;

        rigid.bodyType = RigidbodyType2D.Kinematic;
        while (duration > 0)
        {
            duration -= Time.deltaTime;
            yield return null;

        }

        rigid.bodyType = RigidbodyType2D.Dynamic;
        speed = tempSpeed;

        done.Invoke();

    }
=======
>>>>>>> parent of 82e7fcf (시스템프로그래밍및보안 기능 구현)
    private void OnDisable()
    {
        if (transform.childCount > 0)
        {
            Transform SystemProgLockObj = transform.GetChild(0);
            SystemProgLockObj.parent = GameManager.Instance.pool.transform;
            SystemProgLockObj.gameObject.SetActive(false);
        }

        StopAllCoroutines();
    }
}