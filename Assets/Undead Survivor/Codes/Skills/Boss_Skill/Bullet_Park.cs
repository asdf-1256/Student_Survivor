using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Park : MonoBehaviour
{
    [SerializeField] Transform bossTransform;
    [SerializeField] GameObject warningObject;
    [SerializeField] GameObject bombObject;
    [SerializeField] float flightTime = 2;
    [SerializeField] float lifeTime = 1;
    [SerializeField] float rotateSpeed = 200;

    [SerializeField] float warningTime = 1;
    [SerializeField] float fadeOutTime = 1;

    private Vector3 targetPosition = Vector3.zero;

    private SpriteRenderer warningSpriter;
    private GameObject glassObject;
    private GameObject shadowObject;
    private Transform target;
    private Rigidbody2D bombParentRigid;
    private Rigidbody2D glassRigid;
    private SpriteRenderer explosionSpriter;
    private Collider2D explosionCollider;
    private void Awake()
    {
        warningSpriter = warningObject.GetComponent<SpriteRenderer>();
        glassObject = bombObject.transform.GetChild(1).gameObject;
        shadowObject = bombObject.transform.GetChild(0).gameObject;
        bombParentRigid = bombObject.GetComponent<Rigidbody2D>();
        glassRigid = glassObject.GetComponent<Rigidbody2D>();
        explosionSpriter = bombObject.GetComponent<SpriteRenderer>();
        explosionCollider = bombObject.GetComponent<Collider2D>();
        target = GameManager.Instance.player.transform;
    }
    private void OnEnable()
    {
        targetPosition = target.position;

        StartCoroutine(WarningFadeOutRoutine());
    }
    
    IEnumerator WarningFadeOutRoutine()
    {
        warningObject.SetActive(true);

        float alpha = 1f;
        float timer = 0f;
        Color color = new(warningSpriter.color.r, warningSpriter.color.g, warningSpriter.color.b, alpha);

        warningSpriter.color = color;
        warningObject.transform.position = targetPosition;
        warningObject.transform.SetParent(null);
        while (timer < warningTime)
        {
            timer += Time.deltaTime;

            yield return null;

        }
        timer = 0f;
        while (timer < fadeOutTime)
        {
            timer += Time.deltaTime;

            alpha -= Time.deltaTime / fadeOutTime;
            color.a = alpha;
            warningSpriter.color = color;

            yield return null;
        }
        
        warningObject.SetActive(false);
        warningObject.transform.SetParent(transform);

        warningObject.transform.position = Vector3.zero;

        yield return StartCoroutine(CurveRoutine());


    }
    IEnumerator CurveRoutine() //child = 첫번째 자식인 자바컵이 중력받으며 위로 올라가는 함수
    {
        bombObject.SetActive(true);
        glassObject.SetActive(true);
        shadowObject.SetActive(true);

        float time = 0.0f;

        Vector3 start = bossTransform.position;
        Vector3 end = targetPosition;

        //transform.position = start;
        bombObject.transform.position = start;
        glassObject.transform.position = bombObject.transform.position;

        Vector3 dirVec = end - start;
        dirVec = dirVec * 1 / flightTime;

        float upperForceToCup = flightTime * 4.9f; // 컵을 위로 던지는 힘은 날아가는 거리에 비례
        glassRigid.velocity = dirVec + new Vector3(0, upperForceToCup, 0);
        bombParentRigid.velocity = dirVec;


        float rotateCup = 0;
        Vector3 rotate = new(0, 0, 0);
        while (time < flightTime)
        {
            rotateCup += rotateSpeed * Time.deltaTime;
            rotate.Set(0, 0, rotateCup);
            glassObject.transform.eulerAngles = rotate;
            time += Time.deltaTime;

            yield return null;
        }

        bombParentRigid.velocity = Vector2.zero;
        glassRigid.velocity = Vector2.zero;

        glassObject.SetActive(false);
        shadowObject.SetActive(false);
        yield return StartCoroutine(BombRoutine());
    }
    IEnumerator BombRoutine()
    {
        float timer = 0f;

        explosionCollider.enabled = true;
        explosionSpriter.enabled = true;

        while (timer < lifeTime)
        { //n초 동안 대기
            timer += Time.deltaTime;
            yield return null;
        }

        explosionCollider.enabled = false;
        explosionSpriter.enabled = false;

        bombObject.SetActive(false);

        StopAllCoroutines();

        gameObject.SetActive(false);
    }
    /*
    private void OnDisable() //비활성화할 때 초기상태로 되돌리기
    {
        bombObject.SetActive(false);
        shadowObject.SetActive(false);
        warningObject.SetActive(false);
        glassObject.SetActive(false);
    }*/
}
