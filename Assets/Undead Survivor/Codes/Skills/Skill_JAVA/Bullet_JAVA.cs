using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_JAVA : MonoBehaviour
{
    [SerializeField]
    float flightSpeed = 2.0f; //1�� �� ���� Ŀ�Ƿ� ����
    //[SerializeField]
    //GameObject cup_prefab;
    [SerializeField]
    Sprite[] sprites; // 0:�� �̹���, 1:Ŀ�� �̹���

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
        CupObject = transform.GetChild(1).gameObject; // 1��° �ڽ� ���� �� = �ڹ���
        duration = 2f;
        damage = 10f;
    }

    private void OnEnable()
    {

        StartCoroutine(CurveRoutine()); //PoolManager���� Get�ؿ��� �ٷ� ���� ���ư������Ѵ�.
    }
    IEnumerator CurveRoutine() //child = ù��° �ڽ��� �ڹ����� �߷¹����� ���� �ö󰡴� �Լ�
    {
        float time = 0.0f;
        target = GameManager.Instance.player.scanner.nearestTarget;
        Vector3 start = GameManager.Instance.player.transform.position;
        Vector3 end = target.transform.position;

        transform.position = start;
        CupObject.transform.position = transform.position;
        CupObject.GetComponent<Rigidbody2D>().velocity = end - start + new Vector3(0, 9.8f, 0);
        rigid.velocity = end - start;

        while (time < flightSpeed)
        {
            time += Time.deltaTime;

            yield return null;
        }
        rigid.velocity = Vector2.zero;
        yield return StartCoroutine(CoffeeLavaRoutine(() => { //���� Ŀ�Ƿ� �ٲٴ� �ڷ�ƾ. ���� ��Ȱ��ȭ
            gameObject.SetActive(false);
        }));
    }
    private void OnDisable() //��Ȱ��ȭ�� �� �ʱ���·� �ǵ�����
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
        spriteRenderer.sprite = sprites[1];//�̹����� Ŀ�Ƿ� ����
        coll.enabled = true;//collider Ȱ��ȭ
        float timer = 0f;
        while (timer < duration)
        { //n�� ���� ���
            timer += Time.deltaTime;
            yield return null;
        }
        done.Invoke();//���� ��Ȱ��ȭ �Լ� ȣ��
    }
}