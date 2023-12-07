using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_JAVA : BulletBase
{
    //[SerializeField]
    //GameObject cup_prefab;
    [SerializeField]
    Sprite[] sprites; // 0:�� �̹���, 1:Ŀ�� �̹���


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
    }

    public override void Init(bool isAI, SkillData skillData, int level)
    {
        base.Init(isAI, skillData, level);

        StartCoroutine(CurveRoutine()); //PoolManager���� Get�ؿ��� �ٷ� ���� ���ư������Ѵ�.
    }
    IEnumerator CurveRoutine() //child = ù��° �ڽ��� �ڹ����� �߷¹����� ���� �ö󰡴� �Լ�
    {
        float time = 0.0f;
        // target = GameManager.Instance.player.scanner.nearestTarget;
        target = playerTransform.GetComponent<Scanner>().GetRandomTarget(); // ������ ���� Ÿ������

        Vector3 start = playerTransform.position;
        Vector3 end = target.transform.position;

        transform.position = start;
        CupObject.transform.position = transform.position;

        Vector3 dirVec = end - start;
        dirVec = dirVec * 1 / flightTime;

        float upperForceToCup = flightTime * 4.9f; // ���� ���� ������ ���� ���ư��� �Ÿ��� ���
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
        while (timer < lifeTime)
        { //n�� ���� ���
            timer += Time.deltaTime;
            yield return null;
        }
        done.Invoke();//���� ��Ȱ��ȭ �Լ� ȣ��
    }
}
