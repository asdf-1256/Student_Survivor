using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Skill_WebProg : BulletBase
{
    const float ShootingSize = 0.8f;
    int[] sizes = { 7, 9, 11 };

    private bool isWebed;


    Rigidbody2D rigid;

    Vector3 scale;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        scale = Vector3.one;
    }
    public override void Init(bool isAI, SkillData skillData, int level)
    {
        base.Init(isAI, skillData, level);
        isWebed = false;
        RefactorScale(ShootingSize);
        transform.localScale = scale;
        transform.position = playerTransform.position;
        Vector3 dir = (GameManager.Instance.player.scanner.nearestTarget.transform.position - GameManager.Instance.player.transform.position).normalized;

        rigid.velocity = dir * 5;
    }
    IEnumerator WebRoutine(System.Action done)
    {
        rigid.velocity = Vector3.zero;
        RefactorScale(count);
        transform.localScale = scale;
        float timer = 0f;
        while (timer < lifeTime) { //n초 동안 대기
            timer += Time.deltaTime; 
            yield return null; 
        }
        done.Invoke();//이후 비활성화 함수 호출
    }
    private void RefactorScale(float size)
    {
        scale.x = size;
        scale.y = size;
        scale.z = size;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
            return;
        if (isWebed)
            return;
        StartCoroutine(WebRoutine(() => { gameObject.SetActive(false); }));
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isWebed)
            return;
        if (!collision.CompareTag("Area"))
            return;
        gameObject.SetActive(false);

    }
    private void OnDisable()
    {
        transform.rotation = Quaternion.identity;
    }
}
