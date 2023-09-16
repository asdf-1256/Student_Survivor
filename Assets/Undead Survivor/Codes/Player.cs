using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator animator;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }
    private void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }
    //프레임이 종료 되기 전 실행되는 함수
    private void LateUpdate()
    {
        animator.SetFloat("Speed", inputVec.magnitude);
        //벡터의 크기

        if (inputVec.x != 0)
        {
            spriteRenderer.flipX = inputVec.x < 0;
        }
    }
}



/*
public class Player : MonoBehaviour
{
    //public으로 변경 후 Add Component 등록 시 Input Vec이 보임
    public Vector2 inputVec;
    public float speed;

    Rigidbody2D rigid;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }



    void Update()
    {
        //Input 클래스 = 유니티에서 받는 모든 입력을 관리하는 클래스
        //Project Settint-Input Manager에서 확인
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
        //그냥 GetAxis에는 보정이 들어감. 
    }

    //물리에 관해서는 FixedUpdate
    private void FixedUpdate()
    {
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        //normalized = 대각선으로 이동할 때 속도가 증가하는 일을 방지
        //fixed delta time = 물리 프레임 하나가 소비한 시간
        //delta time = update에서 사용

        //이동방법
        //1.힘을 준다
        //rigid.AddForce(inputVec);

        //2.속도 제어
        //rigid.velocity = inputVec;

        //3.위치 이동
        //매개변수로 월드맵의 위치를 받기때문에 더해주어야한다.
        rigid.MovePosition(rigid.position + nextVec);

        //문제점 1. 너무 빠름, 2.프레임에 따라 이동속도 다를 수 있음
    }
}
*/