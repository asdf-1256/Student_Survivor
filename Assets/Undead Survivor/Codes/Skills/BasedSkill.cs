using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    bool isAI;
    private void Awake()
    {
        player = GameManager.Instance.player;
        timer = 999;
    }
    void Update()
    {
        if (!GameManager.Instance.isLive)
            return;

        timer += Time.deltaTime;

        if (timer > coolTime)
        {
            timer = 0f;
            if(skillData.skillType == SkillData.SkillType.전공)
                Fire();
            else if (skillData.getype == SkillData.GEType.Recovery)
                GameManager.Instance.GetHealth(Convert.ToInt32(skillData.damages[level]));
        }
    }

    public void LevelUp()
    {
        level++;

        coolTime = skillData.cooltimes[level];

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
        player.BroadcastMessage("ApplyCooldown", SendMessageOptions.DontRequireReceiver);
    }
    public void Init(bool isAI, SkillData skillData)
    {
        this.isAI = isAI;
        this.skillData = skillData;
        // skillData.level = 0; // 레벨 초기화
        name = "SKILL " + skillData.skillName; // 오브젝트 name을 설정하는거임
        if (isAI)
        {
            transform.parent = GameManager.Instance.ai_Player.transform;
        }
        else
        {
            transform.parent = player.transform;
        }
        transform.localPosition = Vector3.zero;
        coolTime = skillData.cooltimes[0];

        if (skillData.skillType == SkillData.SkillType.전공)
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
        player.BroadcastMessage("ApplyCooldown", SendMessageOptions.DontRequireReceiver);
    }

    void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;
        GameObject bullet = GameManager.Instance.pool.Get(prefabId);
        bullet.GetComponent<BulletBase>().Init(isAI, skillData, level); // 새로 호출되거나 레벨업 시에만 유의미함
    }

    void ApplyCooldown()
    {
        int[] spawnSkillId = { 3, 6, 7, 8 };
        int[] attackSkillId = { 0, 1, 2, 5, 10, 11, 12, 13 };//음... 스킬 데이터에 enum 한 칸 또 넣고싶어지는 코드다

        if (spawnSkillId.Contains(skillData.skillID))
            coolTime = skillData.cooltimes[level] * GameManager.Instance.player.spawnSkillCoolDownRate;

        if (attackSkillId.Contains(skillData.skillID))
            coolTime = skillData.cooltimes[level] * GameManager.Instance.player.attackSkillCoolDownRate;

    }
}
