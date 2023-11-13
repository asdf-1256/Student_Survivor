using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Cloud : BulletBase
{
    public float spawnDistance = 5;
    public float dropTimeOfLaptop = 0.6f;

    Rigidbody2D rigid;
    Transform target;
    GameObject laptop;
    Collider2D coll;

    float timer;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        coll.enabled = false;
        laptop = transform.GetChild(2).gameObject; // 랩탑 오브젝트 가져오기
    }

    public override void Init(bool isAI, SkillData skillData, int level)
    {
        base.Init(isAI, skillData, level);

        Vector2 randomCircle = Random.insideUnitCircle.normalized; // 원 위의 한 점
        Vector3 spawnPosition = new Vector3(randomCircle.x, randomCircle.y, 0);

        transform.position = playerTransform.position + spawnPosition * spawnDistance; // 캐릭터 중심으로 반지름 5인 원 위의 한 점
        StartCoroutine(DropRoutine());
    }

    private void Update() // 타이머 기능
    {
        if (!GameManager.Instance.isLive)
            return;

        timer += Time.deltaTime;

        if (timer > lifeTime)
        {
            timer = 0f;
            gameObject.SetActive(false); // lifeTime 다 되면 비활성화
        }
    }

    private void FixedUpdate() // 타겟에게 이동
    {
        if (!target || Vector3.Distance(transform.position, target.position) < 0.1f) // 타겟이 비활성화거나 도착했을 때
            target = GameManager.Instance.player.scanner.GetRandomTarget(); // 랜덤한 타겟 선택
        if (!target)
            return; // sccaner가 null을 받아오는 경우도 있음

        Vector3 dirVec = target.position - transform.position;
        Vector3 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(transform.position + nextVec);
        rigid.velocity = Vector2.zero;
    }


    IEnumerator DropRoutine()
    {
        while (true)
        {
            laptop.transform.position = transform.position + new Vector3(0, 2, 0);
            laptop.SetActive(true);
            
            yield return new WaitForSeconds(dropTimeOfLaptop - 0.1f); // 랩탑 떨어질때까지 대기
            coll.enabled = true;
            yield return new WaitForSeconds(0.1f); // 랩탑 떨어질때까지 대기
            coll.enabled = false;

            laptop.SetActive(false);

            yield return new WaitForSeconds(attackCoolTime);
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

}
