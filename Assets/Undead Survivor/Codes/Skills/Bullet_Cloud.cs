using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Cloud : MonoBehaviour
{

    public float damage;
    public float speed;
    public float lifeTime;
    public float dropCycleTime; // 구름이 전자기기를 떨어뜨리는 주기
    public float dropTimeOfLaptop; // 랩탑 자체가 떨어지는 시간.

    Rigidbody2D rigid;
    Transform target;
    GameObject laptop;
    Collider2D coll;

    float timer;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponentInChildren<Collider2D>();
        coll.enabled = false;
        laptop = transform.GetChild(2).gameObject; // 랩탑 오브젝트 가져오기
    }

    public void Init(float damage, float speed, float lifeTime, float dropCycleTime)
    {
        this.damage = damage;
        this.speed = speed;
        this.lifeTime = lifeTime;
        this.dropCycleTime = dropCycleTime;
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
        

        Vector3 dirVec = target.position - transform.position;
        Vector3 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(transform.position + nextVec);
        rigid.velocity = Vector2.zero;
    }

    private void OnEnable()
    {
        StartCoroutine(DropRoutine());
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

            yield return new WaitForSeconds(dropCycleTime);
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

}
