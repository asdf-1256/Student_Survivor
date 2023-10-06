using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_JAVA : MonoBehaviour
{
    [SerializeField]
    float flightTime; // 체공시간. 컵이 커피로 변하는 시간.

    public float rotateSpeed; // 컵이 회전하는 속도
    //[SerializeField]
    //GameObject cup_prefab;
    [SerializeField]
    Sprite[] sprites; // 0:컵 이미지, 1:커피 이미지

    public float duration;
    public float damage;

    SpriteRenderer spriteRenderer;

    Transform target;
    Collider2D coll;
    GameObject CupObject;

    Rigidbody2D rigid;
    private void Awake()
    {
        coll = GetComponent<Collider2D>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        CupObject = transform.GetChild(1).gameObject; // 1번째 자식 갖고 옴 = 자바컵
        duration = 2f;
        damage = 10f;
    }

    private void OnEnable()
    {

        StartCoroutine(CurveRoutine()); //PoolManager에서 Get해오면 바로 컵이 날아가도록한다.
    }
    IEnumerator CurveRoutine() //child = 첫번째 자식인 자바컵이 중력받으며 위로 올라가는 함수
    {
        float time = 0.0f;
        // target = GameManager.Instance.player.scanner.nearestTarget;
        target = GameManager.Instance.player.scanner.GetRandomTarget(); // 랜덤한 적을 타겟으로

        Vector3 start = GameManager.Instance.player.transform.position;
        Vector3 end = target.transform.position;

        transform.position = start;
        CupObject.transform.position = transform.position;

        Vector3 dirVec = end - start;
        dirVec = dirVec * 1 / flightTime;

        float upperForceToCup = flightTime * 4.9f; // 컵을 위로 던지는 힘은 날아가는 거리에 비례
        CupObject.GetComponent<Rigidbody2D>().velocity = dirVec + new Vector3(0, upperForceToCup, 0);
        rigid.velocity = dirVec;


        float rotateCup = 0;
        while (time < flightTime)
        {
            rotateCup += rotateSpeed * Time.deltaTime;
            CupObject.transform.eulerAngles = new Vector3(0, 0, rotateCup);
            time += Time.deltaTime;

            yield return null;
        }
        rigid.velocity = Vector2.zero;
        yield return StartCoroutine(CoffeeLavaRoutine(() => { //컵을 커피로 바꾸는 코루틴. 이후 비활성화
            gameObject.SetActive(false);
        }));
    }
    private void OnDisable() //비활성화할 때 초기상태로 되돌리기
    {
        CupObject.SetActive(true);
        spriteRenderer.sprite = sprites[0];
        coll.enabled = false;
        transform.position = new Vector3(0, 0, 0);
        transform.rotation = Quaternion.identity;
        StopAllCoroutines();
    }

    IEnumerator CoffeeLavaRoutine(System.Action done)
    {
        CupObject.SetActive(false);
        spriteRenderer.sprite = sprites[1];//이미지를 커피로 변경
        coll.enabled = true;//collider 활성화
        float timer = 0f;
        while (timer < duration)
        { //n초 동안 대기
            timer += Time.deltaTime;
            yield return null;
        }
        done.Invoke();//이후 비활성화 함수 호출
    }
}
