using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    Animator anim;
    SpriteRenderer spriteRenderer;
    WaitForFixedUpdate wait;

    //좀비가 죽을 때 드롭할 경험치 데이터를 Inspector상에서 연결함.(Spawner에 있는 것과 동일한 파일)
    public SpawnItemData expData;

    Coroutine lockCoroutine = null;

    [SerializeField] private Transform damageTransform;

    [SerializeField] private GameObject DamageTextObjectPrefab;
    private int damagePoolIndex;

    private int currentSpriteType;

    // Start is called before the first frame update

    private Collider2D[] colliders;
    private Collider2D bossCollider;

    [SerializeField] private bool isBoss;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();

        if (isBoss)
            bossCollider = GetComponent<Collider2D>();
        else
        {
            colliders = new Collider2D[3];
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i] = transform.GetChild(2 + i).GetComponent<Collider2D>();
            }
        }

        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
        damagePoolIndex = GameManager.Instance.pool.GetPoolIndex(DamageTextObjectPrefab);
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
        if(isBoss)
            bossCollider.enabled = true;
        rigid.simulated = true;
        spriteRenderer.sortingOrder = 2;
        anim.SetBool("Dead", false);
        health = maxHealth;
    }

    public void Init(SpawnData data)
    {
        float difficulty = GameManager.Instance.semesterDifficulty[GameManager.Instance.currentPhase];
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health * difficulty;
        health = data.health * difficulty;
        currentSpriteType = data.spriteType;

        if(!isBoss)
            colliders[currentSpriteType / 3].enabled = true;

        //Debug.Log(string.Format("현재 적 체력: {0} * {1} = 최종적으로 {2}", data.health, difficulty, health));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (!collision.CompareTag("Bullet")||!isLive)
        //  return;
        if (!isLive) return;

        if (collision.CompareTag("Bullet")) // 일단은 기본무기 + 운체, 자바, 클라우드, 알고리즘, IoT를 담당
        {
            BulletBase bullet = collision.GetComponent<BulletBase>();
            if (bullet.damage == 1e+07)
                if (gameObject.name.Contains("Boss"))
                    return;
            health -= bullet.damage * GameManager.Instance.player.attackRate; // 이건 왜 이거야

            PrintDamage(bullet.damage * GameManager.Instance.player.attackRate);

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
                if(!isBoss)
                    colliders[currentSpriteType / 3].enabled = false;
                else
                    bossCollider.enabled = false;
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
        else if (collision.CompareTag("Web"))
            speed /= collision.GetComponent<BulletBase>().damage;
        else if (collision.CompareTag("SysProg"))
        {
            Bullet_SystemProgramming bullet = collision.GetComponent<Bullet_SystemProgramming>();
            if (lockCoroutine == null)
                lockCoroutine = StartCoroutine(LockRoutine(collision.GetComponent<Bullet_SystemProgramming>().lifeTime, () =>
                {
                    lockCoroutine = null;
                    bullet.transform.parent = GameManager.Instance.pool.transform;
                    bullet.gameObject.SetActive(false);
                }));
            else
            {
                bullet.gameObject.SetActive(false);
            }
        }

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
            Bullet_MachhineLearning bullet = targetObject.GetComponent<Bullet_MachhineLearning>();
            health -= bullet.damage * GameManager.Instance.player.attackRate;

            PrintDamage(bullet.damage * GameManager.Instance.player.attackRate);

            if (health <= 0)
            {
                //.. 죽음
                isLive = false;
                if(!isBoss)
                    colliders[currentSpriteType / 3].enabled = false;
                else
                    bossCollider.enabled = false;
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
        if (collision.CompareTag("Web"))
            speed *= collision.GetComponent<BulletBase>().damage;
    }
    void DropExp()
    {
        int tmp = Random.Range(0, 100);
        if (tmp > 20) // 확률적으로 Exp 드롭
        {
            return;
        }
        GameObject Exp = GameManager.Instance.player.GetComponentInChildren<Spawner>().SpawnItem(expData);
        Exp.transform.position = transform.position;
        //Debug.Log("@경험치 드랍됨");

    }

    //코루틴 - 비동기
    IEnumerator KnockBack()
    {
        yield return wait;

        Vector3 playerPos = GameManager.Instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
        //순간적인 힘 - Impulse


    }
    void Dead()
    {
        if (transform.childCount > 1)
        {
            rigid.bodyType = RigidbodyType2D.Dynamic;
            Bullet_SystemProgramming bullet = GetComponentInChildren<Bullet_SystemProgramming>();
            if (bullet != null)
            {
                bullet.transform.SetParent(GameManager.Instance.pool.transform);
                bullet.gameObject.SetActive(false);
            }
        }
        lockCoroutine = null;

        if (!isBoss)
            if (GameManager.Instance.killByType.ContainsKey(currentSpriteType / 3))
            {
                GameManager.Instance.killByType[currentSpriteType / 3] += 1;
            }
            else
            {
                GameManager.Instance.killByType.Add(currentSpriteType / 3, 1);
            }
        Debug.Log(string.Format("과제 : {0} | 퀴즈 : {1} | 시험 : {2}", GameManager.Instance.killByType[0], GameManager.Instance.killByType[1], GameManager.Instance.killByType[2]));

        gameObject.SetActive(false);
    }
 
    public IEnumerator LockRoutine(float duration, System.Action done)
    {
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
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private void PrintDamage(float damage)
    {
        GameObject damageText = GameManager.Instance.pool.Get(damagePoolIndex);
        damageText.GetComponent<DamageTextMesh>().Init(damageTransform, damage);
    }
}