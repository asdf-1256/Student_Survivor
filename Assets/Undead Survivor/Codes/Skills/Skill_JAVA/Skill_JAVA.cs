using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_JAVA : MonoBehaviour
{
    [SerializeField]
    public int bulletPrefabID;
    public float coolTime;
    public float flightTime; // 체공시간. 컵이 커피로 변하는 시간.
    public float rotateSpeed; // 컵이 회전하는 속도
    public float lifeTime; // 용암으로 존재하는 시간
    public float damage;

    public GameObject Bullet; // 총알이 어떤 프리팹인지 보여주기만 하는 용도


    float timer;
    private void Awake()
    {
        A_Skill_Data skillData = GetComponentInParent<A_Skill_Data>();
        bulletPrefabID = skillData.bulletPrefabID;
        coolTime = skillData.coolTime;
        flightTime = skillData.flightTime;
        rotateSpeed = skillData.rotateSpeed;
        lifeTime = skillData.lifeTime;
        damage = skillData.damage;
    }

    private void Update()
    {

        if (!GameManager.Instance.isLive)
            return;

        timer += Time.deltaTime;

        if (timer > coolTime)
        {
            timer = 0f;
            Fire();
        }
        /*if (!GameManager.Instance.isLive)
            return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(string.Format("space바 {0}번째 눌림", space_count));
            switch (space_count)
            {
                case 0:
                    GameManager.Instance.player.skillManager.AddSkill(0);
                    break;
                case 1:
                    GameManager.Instance.player.skillManager.LevelUp(0);
                    break;
                case 2:
                    GameManager.Instance.player.skillManager.LevelUp(0);
                    break;
                case 3:
                    break;
                case 4:
                    GameManager.Instance.player.skillManager.AddSkill(1);
                    break;
                case 5:
                    GameManager.Instance.player.skillManager.LevelUp(1);
                    break;
                case 6:
                    GameManager.Instance.player.skillManager.LevelUp(1);
                    break;
                case 7:
                    break;
            }
            space_count++;

        }*/


    }
    void Fire()
    {
        if (!GameManager.Instance.player.scanner.nearestTarget)
            return;
        // GameManager.Instance.pool.Get(5);
        Transform bullet = GameManager.Instance.pool.Get(bulletPrefabID).transform;
        bullet.GetComponent<Bullet_JAVA>().Init(flightTime, rotateSpeed, lifeTime, damage);
    }
    IEnumerator ThrowCupRoutine()
    {
        yield return null;
    }
    IEnumerator CoffeeLava()
    {
        yield return null;
    }

}
//커피랑 컵 던지는 거 소스 통합.
//던질 때 컵 sprite
//시간되면 고정 시키고 커피 sprite 로 변경 이후 collider enable; 후 시간 지나면 끄고 장판도 없애기

//Pool Manager에 추가.

//스킬 쿨타임 연산을 위한 매니저 클래스 추가가 필요. 
