using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_LogicCurcuit : BulletBase
{

    [SerializeField]
    Sprite[] sprites; // 0:트랜지스터 이미지, 1: 폭발 이미지


    Collider2D collExplosion, collCurcuit; // 콜라이더들
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        collExplosion = GetComponent<Collider2D>(); // 폭발모드 콜라이더
        collCurcuit = GetComponentInChildren<Collider2D>(); // OS로봇 모드 콜라이더
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public override void Init(bool isAI, SkillData skillData, int level)
    {
        base.Init(isAI, skillData, level);

        transform.position = playerTransform.position;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
            return;
        Debug.Log("적과 접촉함!");
        spriteRenderer.sprite = sprites[1];//이미지를 폭발로 변경

        StartCoroutine(ExplosionRoutine(() => { gameObject.SetActive(false); }));

    }
    private void OnDisable()
    {
        collExplosion.enabled = false;
        collCurcuit.enabled = true;

        spriteRenderer.sprite = sprites[0];
        transform.position = new Vector3(0, 0, 0);
        // transform.rotation = Quaternion.identity;
    }
    IEnumerator ExplosionRoutine(System.Action done)
    {
        collCurcuit.enabled = false;
        collExplosion.enabled = true;

        float timer = 0f;
        while (timer <= lifeTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        done.Invoke();
    }
}
