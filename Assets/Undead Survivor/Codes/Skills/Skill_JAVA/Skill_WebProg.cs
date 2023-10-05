using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Skill_WebProg : SkillBase
{
    const float ShootingSize = 0.8f;
    int[] sizes = { 7, 9, 11 };

    public float duration;
    private bool isWebed;

    Collider2D coll;

    Rigidbody2D rigid;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
        duration = 3f;
        rigid = GetComponent<Rigidbody2D>();
    }
    IEnumerator WebRoutine(System.Action done)
    {
        rigid.velocity = Vector3.zero;
        transform.localScale = new Vector3(sizes[base.GetLevel()], sizes[base.GetLevel()], sizes[base.GetLevel()]);
        float timer = 0f;
        while (timer < duration) { //n초 동안 대기
            timer += Time.deltaTime; 
            yield return null; 
        }
        done.Invoke();//이후 비활성화 함수 호출
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

    private void OnEnable()
    {
        if (GameManager.Instance.player.scanner.nearestTarget == null)
            return;
        isWebed = false;
        transform.localScale = new Vector3(ShootingSize, ShootingSize, ShootingSize);
        transform.position = GameManager.Instance.player.transform.position;
        Vector3 dir = (GameManager.Instance.player.scanner.nearestTarget.transform.position - GameManager.Instance.player.transform.position).normalized;

        rigid.velocity = dir * 5;
    }
    private void OnDisable() //비활성화할 때 초기상태로 되돌리기
    {
        transform.rotation = Quaternion.identity;
    }
}
