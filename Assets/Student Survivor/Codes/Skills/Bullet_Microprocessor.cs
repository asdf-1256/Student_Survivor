using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Microprocessor : BulletBase
{
    Transform lineTracer;
    //Transform lineTracerCollider;

    
    Vector3[] positions;
    WaitForFixedUpdate wait = new WaitForFixedUpdate();
    private void Awake()
    {
        lineTracer = transform.GetChild(0).GetComponent<Transform>();
        //lineTracerCollider = transform.GetChild(1).GetComponent<Transform>();


        positions = new Vector3[6];
        positions[0] = new Vector3 (0, -1, 0) * 2;
        positions[1] = new Vector3(1, -1, 0) * 2;
        positions[2] = new Vector3(1, 1, 0) * 2;
        positions[3] = new Vector3(-1, 1, 0) * 2;
        positions[4] = new Vector3(-1, -1, 0) * 2;
        positions[5] = new Vector3(0, -1, 0) * 2;
    }

    public override void Init(bool isAI, SkillData skillData, int level)
    {
        base.Init(isAI, skillData, level);

        //lineTracerCollider.GetComponent<Bullet>().putDamage(damage); // �ڽ� �Ѿ��� ������ ����
        transform.parent = playerTransform;
        transform.localPosition = Vector3.zero;
        lineTracer.rotation = Quaternion.identity;
        lineTracer.GetComponent<Bullet>().putDamage(damage);
        StartCoroutine(LineTracerRoutine(() => { gameObject.SetActive(false); }));
    }

    IEnumerator LineTracerRoutine(System.Action done)
    {
        for(int i = 1; i < positions.Length; i++)
        {
            Vector3 nowPos = positions[i - 1];
            Vector3 nextPos = positions[i];
            Vector3 dir = (nextPos - nowPos).normalized;

            lineTracer.Rotate(Vector3.forward, 90f, Space.World);

            //Debug.Log("dir�� ��" + dir);

            while (Vector3.Distance(lineTracer.localPosition, nextPos) > 0.1f) {
                lineTracer.localPosition += dir * speed * Time.fixedDeltaTime;
                //lineTracerCollider.localPosition += dir * speed * Time.fixedDeltaTime;
                yield return wait;
            }

        }

        done.Invoke();
    }


}
