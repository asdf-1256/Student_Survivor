using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_OS : BulletBase
{

    Rigidbody2D rigid;
    float timer;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    public override void Init(bool isAI, SkillData skillData, int level)
    {
        base.Init(isAI, skillData, level);


        Vector2 randomCircle = Random.insideUnitCircle; // 원 내의 한 점
        Vector3 randomPosition = new Vector3(randomCircle.x, randomCircle.y, 0);
        randomPosition = randomPosition.normalized; // 원 위의 한 점

        transform.position = playerTransform.position;
        Vector3 dir = randomPosition;
        rigid.velocity = dir * speed;

        StartCoroutine(MoveRoutine()); // 해당 오브젝트가 On 될 때마다 실행
    }

    IEnumerator MoveRoutine()
    {
        while (timer < flightTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        timer = 0f;
        StartCoroutine(SaBangPalBang());
    }

    IEnumerator SaBangPalBang()
    {
        Fire(1, 0);
        Fire(1, 1);
        Fire(0, 1);
        Fire(-1, 0);
        Fire(-1, -1);
        Fire(0, -1);
        Fire(1, -1);
        Fire(-1, 1);
        yield return null;
    }
    void Fire(int x, int y)
    {

        Vector3 dir = new Vector3(x, y, 0);
        dir = dir.normalized;

        Transform bullet = GameManager.Instance.pool.Get(2).transform; // Bullet 1의 총알 그대로 일단 씀

        bullet.position = transform.position;//위치결정
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);//회전결정
        bullet.GetComponent<Bullet>().Init(damage, 1, dir);
    }

}
