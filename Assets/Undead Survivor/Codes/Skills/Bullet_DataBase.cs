using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_DataBase : BulletBase
{
    GameObject blackhole; //자식 오브젝트
    Rigidbody2D rigid; //본인 Rigidbody2D

    float timer = 0;
    private void Awake()
    {
        blackhole = transform.GetChild(0).gameObject;
        rigid = GetComponent<Rigidbody2D>();
    }
    private void Start() //OnEnable에서만 위치를 조정하니까 오브젝트가 처음 생성될 때만 Rigidbody2D의 velocity가 (0,0)이 되는 오류가 있어서 Start함수도 호출
    {
        transform.position = GameManager.Instance.player.transform.position;
        Vector3 targetPos = GameManager.Instance.player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - GameManager.Instance.player.transform.position;
        dir = dir.normalized;//방향 구하기
        transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        rigid.velocity = Vector3.zero;
        rigid.velocity = dir * speed;
    }
    private void Update() //타이머
    {
        timer += Time.deltaTime;

        if (timer > lifeTime)
        {
            timer = 0;
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) //적과 충돌 감지 - 블랙홀 활성화
    {
        if (!collision.CompareTag("Enemy"))
            return;
        rigid.velocity = Vector3.zero;
        transform.rotation = Quaternion.identity;
        blackhole.SetActive(true);
    }
    private void OnTriggerExit2D(Collider2D collision) //플레이어 주변에서 멀어질 경우 총알을 비활성화, 블랙홀이 활성화된 상태라면 이 작업은 무시
    {
        if (!collision.CompareTag("Area"))
            return;

        if (blackhole.activeSelf)
            return;
        gameObject.SetActive(false);
    }
    private void OnEnable() //발사 로직
    {
        transform.position = GameManager.Instance.player.transform.position;
        Vector3 targetPos = GameManager.Instance.player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - GameManager.Instance.player.transform.position;
        dir = dir.normalized;//방향 구하기
        transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        rigid.velocity = Vector3.zero;
        rigid.velocity = dir * speed;
    }
    private void OnDisable() //블랙홀 끄기
    {
        blackhole.SetActive(false);
    }
}
