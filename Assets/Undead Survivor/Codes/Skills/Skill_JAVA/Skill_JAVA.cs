using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_JAVA : MonoBehaviour
{
    [SerializeField]
    public int id = 2;
    public float cooltime = 5f;
    public float timer;

    
    public GameObject cup;

    //쿨타임 계산해서 호출하는 간단한 함수.

    private void Update()
    {
        if (!GameManager.Instance.isLive)
            return;

        timer += Time.deltaTime;

        if (timer > cooltime)
        {
            timer = 0f;
            Fire();
        }
    }
    void Fire()
    {
        if (!GameManager.Instance.player.scanner.nearestTarget)
            return;
        // GameManager.Instance.pool.Get(5);
        GameManager.Instance.pool.Get(8);
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
