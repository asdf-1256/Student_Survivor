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
    float flightSpeed = 1.0f; //1�� �� ���� Ŀ�Ƿ� ����
    //[SerializeField]
    //GameObject cup_prefab;
    [SerializeField]
    Sprite[] sprites; // 0:�� �̹���, 1:Ŀ�� �̹���

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
    
    IEnumerator CurveRoutine() //���������� ���ư����ϴ� �ڷ�ƾ
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

        yield return StartCoroutine(CoffeeLavaRoutine(() => { //���� Ŀ�Ƿ� �ٲٴ� �ڷ�ƾ. ���� ��Ȱ��ȭ
            gameObject.SetActive(false); 
        } ));   
    }
    
    IEnumerator CoffeeLavaRoutine(System.Action done)
    {
        spriteRenderer.sprite = sprites[1];//�̹����� Ŀ�Ƿ� ����
        coll.enabled = true;//collider Ȱ��ȭ
        float timer = 0f;
        while (timer < duration) { //n�� ���� ���
            timer += Time.deltaTime; 
            yield return null; 
        }
        done.Invoke();//���� ��Ȱ��ȭ �Լ� ȣ��
    }
    private void OnEnable()
    {
        if (GameManager.Instance.player.scanner.nearestTarget == null)
            return;
        StartCoroutine(CurveRoutine()); //PoolManager���� Get�ؿ��� �ٷ� ���� ���ư������Ѵ�.
    }
    private void OnDisable() //��Ȱ��ȭ�� �� �ʱ���·� �ǵ�����
    {
        spriteRenderer.sprite = sprites[0];
        coll.enabled = false;
        transform.position = new Vector3(0, 0, 0);
        transform.rotation = Quaternion.identity;
        StopAllCoroutines();
    }
}
