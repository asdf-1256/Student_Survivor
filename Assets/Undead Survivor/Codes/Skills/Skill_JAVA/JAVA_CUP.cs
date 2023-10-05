using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class JAVA_CUP : SkillBase
{
    [SerializeField]
    AnimationCurve curve;
    [SerializeField]
    float flightSpeed = 1.0f; //1초 후 컵이 커피로 변함
    //[SerializeField]
    //GameObject cup_prefab;
    [SerializeField]
    Sprite[] sprites; // 0:컵 이미지, 1:커피 이미지

    public float duration;
    //public float damage;

    SpriteRenderer spriteRenderer;

    Transform target;
    Collider2D coll;

    //Rigidbody2D rigid;
    private void Awake()
    {
        coll = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        duration = 2f;
        //damage = 10f;
    }
    
    IEnumerator CurveRoutine() //포물선으로 날아가게하는 코루틴
    {
        float duration = flightSpeed;
        float time = 0.0f;
        target = GameManager.Instance.player.scanner.nearestTarget;


        while (time < duration)
        {
            Vector3 start = GameManager.Instance.player.transform.position;
            Vector3 end = target.transform.position;
            time += Time.deltaTime;
            float linearT = time / duration;
            float heightT = curve.Evaluate(linearT);

            float height = Mathf.Lerp(0.0f, target.transform.position.y, heightT);
            transform.position = Vector2.Lerp(start, end, linearT) + new Vector2(0.0f, height);
            yield return null;
        }

        yield return StartCoroutine(CoffeeLavaRoutine(() => { //컵을 커피로 바꾸는 코루틴. 이후 비활성화
            gameObject.SetActive(false); 
        } ));   
    }
    
    IEnumerator CoffeeLavaRoutine(System.Action done)
    {
        spriteRenderer.sprite = sprites[1];//이미지를 커피로 변경
        coll.enabled = true;//collider 활성화
        float timer = 0f;
        while (timer < duration) { //n초 동안 대기
            timer += Time.deltaTime; 
            yield return null; 
        }
        done.Invoke();//이후 비활성화 함수 호출
    }
    private void OnEnable()
    {
        if (GameManager.Instance.player.scanner.nearestTarget == null)
            return;
        StartCoroutine(CurveRoutine()); //PoolManager에서 Get해오면 바로 컵이 날아가도록한다.
    }
    private void OnDisable() //비활성화할 때 초기상태로 되돌리기
    {
        spriteRenderer.sprite = sprites[0];
        coll.enabled = false;
        transform.position = new Vector3(0, 0, 0);
        transform.rotation = Quaternion.identity;
        StopAllCoroutines();
    }
}
