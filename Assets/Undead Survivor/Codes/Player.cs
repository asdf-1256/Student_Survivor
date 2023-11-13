using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public Scanner scanner;
    public Hand[] hands;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator animator;
    public RuntimeAnimatorController[] animCon;

    //스텟 수치 = 합연산
    [SerializeField]
    public float attackRate; //공격력 버프
    public float speedRate; //속도 버프
    public float defenseRate; //방어력 버프
    //public float magneticRate; // 자석 버프 - 현재는 아이템이 이것뿐이라 이것만 테스트 되어있지만, 다른 아이템도 추가하여 버프를 여러개 획득했을 시의 테스트를 진행해야함
    public bool isInvincible; // 무적

    private List<BuffData> buffs; //버프 목록
    private WaitForSeconds wait; //남은 시간 계산용 WaitForSeconds 객체 (0.1초)

    public SkillManager skillManager;


    public float spawnSkillCoolDownRate = 1f;
    public float attackSkillCoolDownRate = 1f;

    public Vector3 Scale
    {
        set
        {
            transform.localScale = value;
            for(int i = 0; i < transform.childCount; i++)
            {
                if (i == 0)
                    continue;
                Transform child = transform.GetChild(i);
                child.localScale = new Vector3(1 / value.x, 1 / value.y, 1 / value.z);
            }
        }
    }

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true); // 비활성화된 오브젝트를 포함하여 가져온다.
        
        attackRate = 1f;
        speedRate = 1f;
        defenseRate = 1f;
        //magneticRate = 1f;
        isInvincible = false;
        buffs = new List<BuffData>();
        wait = new WaitForSeconds(0.1f);
    }
    private void OnEnable()
    {
        speed *= Character.Speed;
        animator.runtimeAnimatorController = animCon[GameManager.Instance.playerId];
    }
    private void FixedUpdate()
    {
        if (!GameManager.Instance.isLive)
            return;
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime * speedRate;
        rigid.MovePosition(rigid.position + nextVec);
    }
    private void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }
    //프레임이 종료 되기 전 실행되는 함수
    private void LateUpdate()
    {
        if (!GameManager.Instance.isLive)
            return;
        animator.SetFloat("Speed", inputVec.magnitude);
        //벡터의 크기

        if (inputVec.x != 0)
        {
            spriteRenderer.flipX = inputVec.x < 0;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.Instance.isLive)
            return;

        if (isInvincible)
            return;

        GameManager.Instance.health -= Time.deltaTime * 10 / defenseRate;

        if (GameManager.Instance.health < 0)
        {
            //shadow와 area는 살려두고 나머지 비활성화
            for (int index = 2; index < transform.childCount; index++)
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }
            animator.SetTrigger("Dead");
            GameManager.Instance.GameOver();
        }
    }
    

    //Collector 함수에서 해당 함수를 호출하여 버프 적용
    public void ActivateBuff(BuffData buff)
    {
        if (buffs.Contains(buff))
        {
            buff.ResetTime();
            //Debug.Log("버프 시간 리셋");
        }

        else
        {
            //Debug.Log("버프 적용");
            switch (buff.effect) //버프 효과 종류에 따라 각각의 스텟에 비율을 곱한다.
            {
                case BuffData.BuffEffect.Attack:
                    attackRate *= buff.value;
                    break;
                case BuffData.BuffEffect.Speed:
                    speedRate *= buff.value;
                    break;
                case BuffData.BuffEffect.Defense:
                    defenseRate *= buff.value;          
                    break;
                case BuffData.BuffEffect.Magnetic:
                    GetComponentInChildren<Magnet>().MagneticRate *= buff.value;
                    break;
                case BuffData.BuffEffect.Invincible:
                    isInvincible = true;
                    break;
            }
            buffs.Add(buff); //버프 리스트에 버프를 추가한다
            StartCoroutine(BuffRoutine(buff, () =>  //버프 지속시간을 계산하는 코루틴을 실행한다. 버프가 끝나면 람다식 함수가 호출된다.
            {
                //Debug.Log("버프 지속시간 끝");
                switch (buff.effect) //버프 종류에 따라 종료되는 버프로 증가된 만큼 값을 감소시킨다
                {
                    case BuffData.BuffEffect.Attack:
                        attackRate /= buff.value;                       
                        break;
                    case BuffData.BuffEffect.Speed:
                        speedRate /= buff.value;
                        break;
                    case BuffData.BuffEffect.Defense:
                        defenseRate /= buff.value;
                        break;
                    case BuffData.BuffEffect.Magnetic:
                        GetComponentInChildren<Magnet>().MagneticRate /= buff.value;
                        break;
                    case BuffData.BuffEffect.Invincible:
                        isInvincible = false;
                        break;
                }
                buffs.Remove(buff); //버프 목록에서 제거한다.
                buff.ResetTime(); //다음에 다시 같은 아이템을 먹게 될 경우 지속시간이 정상적으로 적용되도록 초기화시켜준다.
            }));
        }
    }
    private IEnumerator BuffRoutine(BuffData buff, System.Action done)
    {//버프 데이터 객체에서 남은 지속시간을 흘러간 시간만큼 계속 감소시키다가
        //지속시간이 0이 될 경우 버프를 끝낸다.
        float remainTime;
        while ((remainTime = buff.GetRemainTime()) >= 0f)
        {
            remainTime -= 0.1f;
            buff.SetRemainTime(remainTime);
            yield return wait;
        }
        done.Invoke();
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