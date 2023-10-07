using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Scanner : MonoBehaviour
{
    public float scanRange;
    public LayerMask targetLayer; // 인스펙터에서 감지할 대상 레이어 설정
    public RaycastHit2D[] targets;
    public Transform nearestTarget;

    private void FixedUpdate()
    {
        //원형의 캐스트를 쏘고 모든 결과를 반환
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        nearestTarget = GetNearest();
    }

    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;

        foreach (RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float curDiff = Vector3.Distance(myPos, targetPos);

            if(curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }
        }

        return result;
    }
    
    public Transform GetNearTargetFromNotHitedEnemy(List<Transform> hitedTargets)
    {
        Transform result = null;

        Transform lastesthitedTarget = hitedTargets[^1];

        float diff = 100;

        foreach (RaycastHit2D target in targets)
        {
            if (hitedTargets.Contains(target.transform))
                continue;

            float curDiff = Vector3.Distance(lastesthitedTarget.position, target.transform.position);

            if (curDiff < 2.0f || curDiff > 6.0f)
                continue; //적당히 떨어져 있어야 좀 예쁠 것 같아서.

            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }

        }
        return result;
    }

    /*
    //아직 쓰지는 않는데 혹시나 단일 적한테서 가까운 적 구해야할 일 있으면 쓰려고 만든 코드.
    public Transform GetNearTargetFromEnemy(Transform hitedTarget)
    {
        Transform result = null;
        float diff = 100;

        foreach (RaycastHit2D target in targets)
        {

            float curDiff = Vector3.Distance(hitedTarget.position, target.transform.position);

            if (curDiff < 2.0f)
                continue; //적당히 떨어져는 있어야 좀 예쁠 것 같아서.

            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }

        }
        return result;
    }*/

    public Transform GetRandomTarget() // 랜덤한 타겟을 선택하는 함수
    {
        Transform result = null;
        if (targets.Length == 0)
            return null; // 인덱스 오버나는 경우 막음
        int randomIndex = Random.Range(0, targets.Length);
        
        result = targets[randomIndex].transform;
        
        return result;
    }
}
