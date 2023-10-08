using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_IoT : MonoBehaviour
{
    public float damage;
    public float speed;
    public float lifeTime;
    public float attackCoolTime;
    public float movePosTime; // 다음 포지션으로 변경하는 시간

    Rigidbody2D rigid;
    Scanner scanner;

    float lifetimer;
    float attackTimer;
    Vector3 nextPos;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        scanner = GetComponent<Scanner>();
    }

    public void Init(float damage, float speed, float lifeTime, float attackCoolTime)
    {
        this.damage = damage;
        this.speed = speed;
        this.lifeTime = lifeTime;
        this.attackCoolTime = attackCoolTime;
        GetComponentInParent<A_Skill_Data>().damage = damage; // 외부에서 참조하기 쉽게 따로 데미지 표시
    }
    private void Update() // 타이머 기능
    {
        if (!GameManager.Instance.isLive)
            return;

        lifetimer += Time.deltaTime;
        attackTimer += Time.deltaTime;

        if (lifetimer > lifeTime)
        {
            lifetimer = 0f;
            gameObject.SetActive(false); // lifeTime 다 되면 비활성화
        }
        if (attackTimer > attackCoolTime)
        {
            attackTimer = 0f;
            FireDDabal();
        }
    }

    private void FixedUpdate() // 항상 nextPos로 이동
    {
        if (Vector3.Distance(transform.position, nextPos) < 0.1f)
            return; // nextPos와 근접했으면 멈추기

        Vector3 dirVec = nextPos - transform.position;
        Vector3 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(transform.position + nextVec);
        rigid.velocity = Vector2.zero;
    }


    private void OnEnable()
    {
        // 계속 플레이어 주위를 엄호하기?
        StartCoroutine(setNextPos());
    }

    IEnumerator setNextPos()
    {
        // n초마다 플레이어 주위의 한 포지션을 설정하는 코루틴
        while (true)
        {
            Vector2 randomCircle = Random.insideUnitCircle; // 원 내의 한 점
            Vector3 randomPos = new Vector3(randomCircle.x, randomCircle.y, 0);
            nextPos = GameManager.Instance.player.transform.position + randomPos * 3;

            yield return new WaitForSeconds(movePosTime);
        }
    }

    void FireDDabal()
    {
        if (!scanner.nearestTarget)
            return;

        Vector2 randomCircle = Random.insideUnitCircle * 2; // r=2인 원 내에 랜덤한 위치
        Vector3 ddabalRate = new Vector3(randomCircle.x, randomCircle.y, 0); // vector3로 변환

        Vector3 targetPos = scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position + ddabalRate; // 플레이어->적 벡터에 따발률 첨가
        dir = dir.normalized;//방향 구하기

        Transform bullet = GameManager.Instance.pool.Get(2).transform; // Bullet 1의 총알 그대로 일단 씀

        bullet.position = transform.position;//위치결정
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);//회전결정
        bullet.GetComponent<Bullet>().Init(damage, 1, dir);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
