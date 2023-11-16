using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    [SerializeField]
    private float radius; //아직은 사용되지 않음
    private float magneticRate;
    public float Radius
    {
        get { return radius; }
        set { radius = value; coll.radius = radius * magneticRate; }
    }
    public float MagneticRate
    {
        get { return magneticRate; }
        set
        {
            magneticRate = value;
            //coll.radius = radius * magneticRate;
            transform.localScale = Vector3.one * magneticRate;
            spriteRenderer.enabled = true;
        }
    }

    CircleCollider2D coll; // 자력 범위를 설정할 Collider
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        coll = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        radius = coll.radius;
        magneticRate = 1f;
        spriteRenderer.enabled = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Item"))//Item만을 끌어당긴다.
            return;


        MagnetableItem magnet;//현재 충돌한 아이템의 스크립트를 불러와
        if (!collision.TryGetComponent<MagnetableItem>(out magnet))
        {
            Debug.LogWarning("TryGetComponent문 실패했을 경우 실행됨");
            return;
        }
            magnet.ActiveMagnet(coll.radius);//자석을 활성화 한다.



    }

    IEnumerator CreateMagRoutine() // 10초 동안 반지름 10 증가
    {
        Debug.Log("자석자석");

        coll.radius += 10;
        yield return new WaitForSeconds(10);
        coll.radius -= 10;
        Debug.Log("자석 효과 끝");
    }

    private void LevelUpColliderRadius()
    {
        coll.radius *= 2f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space Key Input 발생");
            LevelUpColliderRadius();
        }
    }
}
