using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasedSkill : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float coolTime;

    public int level;

    public SkillData skillData;

    Player player;
    float timer;
    private void Awake()
    {
        player = GameManager.Instance.player;
    }
    void Update()
    {
        if (!GameManager.Instance.isLive)
            return;

        timer += Time.deltaTime;

        if (timer > skillData.cooltimes[level])
        {
            timer = 0f;
            Fire();
        }
    }

    public void LevelUp()
    {
        level++;

        if (id == 0)
        {
            // Arrange();
        }

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }
    public void Init(SkillData skillData)
    {
        this.skillData = skillData;
        // skillData.level = 0; // 레벨 초기화
        Debug.Log("여기는 BasedSkill의 Init함수 내부입니다");
        name = "SKILL " + skillData.skillName; // 오브젝트 name을 설정하는거임
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        for (int index = 0; index < GameManager.Instance.pool.prefabs.Length; index++)
        {
            if (skillData.bulletPrefab == GameManager.Instance.pool.prefabs[index])
            {
                prefabId = index;
                break;
            }
        }

        /*switch (id)
        {
            case 0:
                speed = 150 * Character.WeaponSpeed;
                // Arrange();
                break;
            default:
                speed = 0.5f * Character.WeaponRate;
                break;
        }
*/
        // Hand Set
        /*Hand hand = player.hands[(int)skillData.itemType];
        hand.spriteRenderer.sprite = skillData.hand;
        hand.gameObject.SetActive(true);*/

        //나중에 추가된 무기에도 버프가 적용되도록
        //이 함수를 갖고있는 애들 다 실행해라 방송
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;
        GameObject bullet = GameManager.Instance.pool.Get(prefabId);
        bullet.GetComponent<BulletBase>().Init(skillData, level); // 새로 호출되거나 레벨업 시에만 유의미함
    }
}
