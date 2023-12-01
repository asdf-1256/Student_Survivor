using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class MagnetableItem : MonoBehaviour
{

    Transform target;//플레이어의 위치 및, Magnet의 반경에 들어왔는지 확인함
    private readonly float MagneticConst = 4f;//자력 상수
    

    private void Awake()
    {
        target = null;
    }
    
    //아이템을 먹어 비활성화 될 때, 타겟을 없앤다
    private void OnDisable()
    {
        target = null;
    }

    private void FixedUpdate()
    {
        if (target == null)//타겟이 없는 경우 - 자력 범위에 들어오지 않은 경우.
            return;

        Vector2 directionVect = target.position - transform.position; // 코인 -> 플레이어 벡터 구하기
        float distance = directionVect.magnitude; //거리 구하기
        Vector2 MagneticForce = directionVect.normalized * MagneticConst / distance; // 자기력 구하기
        if (MagneticForce.magnitude < 4f) //자기력이 너무 작다면(= 속도가 너무 느리다면) 일정 수준(4)으로 보정
            MagneticForce = MagneticForce.normalized * 4;
        transform.Translate(MagneticForce * Time.fixedDeltaTime); //코인의 좌표를 벡터 * 프레임 만큼 이동시키기
    }

    //Player 자식 오브젝트인 Magnet의 Collider에 코인, 경험치 등의 자력의 영향을 받는 아이템이 들어올 경우 이 함수가 호출되어 target과 자력이 활성화되고
    //이후 Player쪽으로 움직이다가 Player의 자식 오브젝트인 Collector의 Collider에서 물건을 습득하도록 처리.
    public void ActiveMagnet()
    {
        target = GameManager.Instance.player.transform;
    }
}
