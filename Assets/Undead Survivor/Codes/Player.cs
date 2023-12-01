using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public Scanner scanner;
    public Hand[] hands;

    public Transform SpriteTransform;
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    Rigidbody2D rigid;
    public RuntimeAnimatorController[] animCon;

    //���� ��ġ = �տ���
    [SerializeField]
    public float attackRate; //���ݷ� ����
    public float speedRate; //�ӵ� ����
    public float defenseRate; //���� ����
    //public float magneticRate; // �ڼ� ���� - ����� �������� �̰ͻ��̶� �̰͸� �׽�Ʈ �Ǿ�������, �ٸ� �����۵� �߰��Ͽ� ������ ������ ȹ������ ���� �׽�Ʈ�� �����ؾ���
    public bool isInvincible; // ����

    private List<BuffData> buffs; //Ȱ��ȭ�� ���� ���
    private WaitForSeconds wait; //���� �ð� ���� WaitForSeconds ��ü (0.1��)

    public SkillManager skillManager;


    public float spawnSkillCoolDownRate = 1f;
    public float attackSkillCoolDownRate = 1f;

    [SerializeField] private float totalDistance = 0f;

    [SerializeField] private Shield _shield;
    public Shield shield
    {
        get { return _shield; }
    }
    [SerializeField] private BuffData[] buffDatas;

    private float debuffSpeedRate = 1f;

    public float TotalDistance
    {
        get { return totalDistance; }
    }
    public Vector3 Scale
    {
        set
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (i == 0)
                    continue;
                Transform child = transform.GetChild(i);
                child.localScale = new Vector3(child.localScale.x / value.x, child.localScale.y / value.y, child.localScale.z / value.z);
            }
            transform.localScale = value;
            
        }
    }
    private CapsuleCollider2D playerCollider;

    private ParticleSystem buffEffectParticle;

    private readonly Color attackBuffColor = Color.red;
    private readonly Color speedBuffColor = Color.blue;
    private readonly Color defenseBuffColor = Color.yellow;
    private readonly Color magneticBuffColor = Color.black;
    private readonly Color invincibleBuffColor = Color.white;

    //private readonly float alpha = 0.25f;
    private readonly float alpha = 1f;

    Gradient buffEffectGradient;

    //빨주노초파
    //오방색?

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true); // ��Ȱ��ȭ�� ������Ʈ�� �����Ͽ� �����´�.
        
        attackRate = 1f;
        speedRate = 1f;
        defenseRate = 1f;
        //magneticRate = 1f;
        isInvincible = false;
        buffs = new List<BuffData>();
        wait = new WaitForSeconds(0.1f);

        playerCollider = GetComponent<CapsuleCollider2D>();


        buffEffectParticle = GetComponent<ParticleSystem>();
        buffEffectGradient = new()
        {
            mode = GradientMode.Fixed
        };
    }
    private void OnEnable()
    {
        speed *= Character.Speed;
        animator.runtimeAnimatorController = animCon[DataManager.Instance.selectedCharacterId];
    }
    private void FixedUpdate()
    {
        if (!GameManager.Instance.isLive)
            return;
        
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime * speedRate * debuffSpeedRate;
        totalDistance += nextVec.magnitude;
        GameManager.Instance.AddManBogi(nextVec.magnitude);
        rigid.MovePosition(rigid.position + nextVec);
    }
    private void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }
    //�������� ���� �Ǳ� �� ����Ǵ� �Լ�
    private void LateUpdate()
    {
        if (!GameManager.Instance.isLive)
            return;
        animator.SetFloat("Speed", inputVec.magnitude);
        //������ ũ��

        if (inputVec.x != 0)
        {
            spriteRenderer.flipX = inputVec.x < 0;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.Instance.isLive)
            return;
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Machine"))
            Physics2D.IgnoreCollision(collision.collider, playerCollider, true);

        if (isInvincible)
            return;

        if (_shield.ShieldCount > 0)
        {
            _shield.RemoveShield();
            foreach (var buffdata in buffDatas)
                if (buffdata.effect == BuffData.BuffEffect.무적)
                    ActivateBuff(buffdata);
        }

        GameManager.Instance.health -= Time.deltaTime * 10 / defenseRate;

        if (GameManager.Instance.health < 0)
        {
            //shadow�� area�� ����ΰ� ������ ��Ȱ��ȭ
            for (int index = 2; index < transform.childCount; index++)
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }
            animator.SetTrigger("Dead");
            GameManager.Instance.GameOver();
        }
    }
    

    //Collector �Լ����� �ش� �Լ��� ȣ���Ͽ� ���� ����
    public void ActivateBuff(BuffData buff)
    {
        buff.ResetTime();
        if (!buffs.Contains(buff))
        {
            //Debug.Log("���� ����");
            switch (buff.effect) //���� ȿ�� ������ ���� ������ ���ݿ� ������ ���Ѵ�.
            {
                case BuffData.BuffEffect.공격력:
                    attackRate *= buff.value;
                    break;
                case BuffData.BuffEffect.이동속도:
                    speedRate *= buff.value;
                    break;
                case BuffData.BuffEffect.방어력:
                    defenseRate *= buff.value;          
                    break;
                case BuffData.BuffEffect.자기력:
                    GetComponentInChildren<Magnet>().MagneticRate *= buff.value;
                    break;
                case BuffData.BuffEffect.무적:
                    isInvincible = true;
                    break;
            }
            buffs.Add(buff); //���� ����Ʈ�� ������ �߰��Ѵ�
            SetBuffParticle();
            UIManager.Instance.Notice(string.Format("{0} 버프를 획득했습니다.", buff.effect.ToString()));
            StartCoroutine(BuffRoutine(buff, () =>  //���� ���ӽð��� ����ϴ� �ڷ�ƾ�� �����Ѵ�. ������ ������ ���ٽ� �Լ��� ȣ��ȴ�.
            {
                //Debug.Log("���� ���ӽð� ��");
                switch (buff.effect) //���� ������ ���� ����Ǵ� ������ ������ ��ŭ ���� ���ҽ�Ų��
                {
                    case BuffData.BuffEffect.공격력:
                        attackRate /= buff.value;                       
                        break;
                    case BuffData.BuffEffect.이동속도:
                        speedRate /= buff.value;
                        break;
                    case BuffData.BuffEffect.방어력:
                        defenseRate /= buff.value;
                        break;
                    case BuffData.BuffEffect.자기력:
                        GetComponentInChildren<Magnet>().MagneticRate /= buff.value;
                        break;
                    case BuffData.BuffEffect.무적:
                        isInvincible = false;
                        break;
                }
                buffs.Remove(buff); //���� ��Ͽ��� �����Ѵ�.
                //UIManager.Instance.Notice(string.Format("{0} 버프의 지속시간이 만료되었습니다.", buff.effect.ToString()));
                SetBuffParticle();
                buff.ResetTime(); //������ �ٽ� ���� �������� �԰� �� ��� ���ӽð��� ���������� ����ǵ��� �ʱ�ȭ�����ش�.
            }));
        }
    }
    private IEnumerator BuffRoutine(BuffData buff, System.Action done)
    {//���� ������ ��ü���� ���� ���ӽð��� �귯�� �ð���ŭ ��� ���ҽ�Ű�ٰ�
        //���ӽð��� 0�� �� ��� ������ ������.
        float remainTime;
        while ((remainTime = buff.GetRemainTime()) >= 0f)
        {
            remainTime -= 0.1f;
            buff.SetRemainTime(remainTime);
            yield return wait;
        }
        done.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("SleepGas"))
            return;
        debuffSpeedRate = 0f;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(!collision.CompareTag("SleepGas"))
            return;
        debuffSpeedRate = 1f;
    }
    private void SetBuffParticle()
    {
        var main = buffEffectParticle.main;

        int count = buffs.Count;
        if (count == 0) {
            buffEffectParticle.Stop();
            return;
        }
        else if (count == 1)
        {
            Color color = Color.white;

            switch (buffs[0].effect)
            {
                case BuffData.BuffEffect.공격력:
                    color = new(attackBuffColor.r, attackBuffColor.g, attackBuffColor.b, alpha);
                    break;
                case BuffData.BuffEffect.이동속도:
                    color = new(speedBuffColor.r, speedBuffColor.g, speedBuffColor.b, alpha);
                    break;
                case BuffData.BuffEffect.방어력:
                    color = new(defenseBuffColor.r, defenseBuffColor.g, defenseBuffColor.b, alpha);
                    break;
                case BuffData.BuffEffect.자기력:
                    color = new(magneticBuffColor.r, magneticBuffColor.g, magneticBuffColor.b, alpha);
                    break;
                case BuffData.BuffEffect.무적:
                    color = new(invincibleBuffColor.r, invincibleBuffColor.g, invincibleBuffColor.b, alpha);
                    break;
            }
            main.startColor = color;
            buffEffectParticle.Play();
            return;
        }


        var colors = new List<GradientColorKey>();
        var alphas = new GradientAlphaKey[2] { new GradientAlphaKey(alpha, 0), new GradientAlphaKey(alpha, 1) };

        for (int i = 0; i < count; i++)
        {
            float min = ((float) i) / count;
            float max = ((float) (i + 1)) / count - 0.001f;
            switch (buffs[i].effect)
            {
                case BuffData.BuffEffect.공격력:
                    //colors.Add(new GradientColorKey(attackBuffColor, min));
                    colors.Add(new GradientColorKey(attackBuffColor, max));
                    break;
                case BuffData.BuffEffect.이동속도:
                    //colors.Add(new GradientColorKey(speedBuffColor, min));
                    colors.Add(new GradientColorKey(speedBuffColor, max));
                    break;
                case BuffData.BuffEffect.방어력:
                    //colors.Add(new GradientColorKey(defenseBuffColor, min));
                    colors.Add(new GradientColorKey(defenseBuffColor, max));
                    break;
                case BuffData.BuffEffect.자기력:
                    //colors.Add(new GradientColorKey(magneticBuffColor, min));
                    colors.Add(new GradientColorKey(magneticBuffColor, max));
                    break;
                case BuffData.BuffEffect.무적:
                    //colors.Add(new GradientColorKey(invincibleBuffColor, min));
                    colors.Add(new GradientColorKey(invincibleBuffColor, max));
                    break;
            }

            UnityEngine.Debug.Log(string.Format("버프 종류 : {0} | min : {1} | max = {2}", buffs[i].effect.ToSafeString(), min, max));
        }

        buffEffectGradient.SetKeys(colors.ToArray(), alphas);
        main.startColor = buffEffectGradient;
        //main.startColor = new ParticleSystem.MinMaxGradient(buffEffectGradient) { mode = ParticleSystemGradientMode.RandomColor };
        buffEffectParticle.Play();
    }
    public void ChangeColliderSize(float size)
    {
        playerCollider.size *= size;
    }
}



/*
public class Player : MonoBehaviour
{
    //public���� ���� �� Add Component ��� �� Input Vec�� ����
    public Vector2 inputVec;
    public float speed;

    Rigidbody2D rigid;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }



    void Update()
    {
        //Input Ŭ���� = ����Ƽ���� �޴� ��� �Է��� �����ϴ� Ŭ����
        //Project Settint-Input Manager���� Ȯ��
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
        //�׳� GetAxis���� ������ ��. 
    }

    //������ ���ؼ��� FixedUpdate
    private void FixedUpdate()
    {
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        //normalized = �밢������ �̵��� �� �ӵ��� �����ϴ� ���� ����
        //fixed delta time = ���� ������ �ϳ��� �Һ��� �ð�
        //delta time = update���� ���

        //�̵����
        //1.���� �ش�
        //rigid.AddForce(inputVec);

        //2.�ӵ� ����
        //rigid.velocity = inputVec;

        //3.��ġ �̵�
        //�Ű������� ������� ��ġ�� �ޱ⶧���� �����־���Ѵ�.
        rigid.MovePosition(rigid.position + nextVec);

        //������ 1. �ʹ� ����, 2.�����ӿ� ���� �̵��ӵ� �ٸ� �� ����
    }
}
*/