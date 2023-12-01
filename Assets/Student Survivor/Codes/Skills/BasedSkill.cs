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
    Transform playerTransform;
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
            if(skillData.skillType == SkillData.SkillType.����)
                Fire();
            else if (skillData.getype == SkillData.GEType.Recovery)
                GameManager.Instance.GetHealth(Convert.ToInt32(skillData.damages[level]));
        }
    }

    public void LevelUp()
    {
        level++;

        coolTime = skillData.cooltimes[level];

        playerTransform.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
        playerTransform.BroadcastMessage("ApplyCooldown", SendMessageOptions.DontRequireReceiver);
    }
    public void Init(bool isAI, SkillData skillData)
    {
        this.isAI = isAI;
        this.skillData = skillData;
        // skillData.level = 0; // ���� �ʱ�ȭ
        name = "SKILL " + skillData.skillName; // ������Ʈ name�� �����ϴ°���
        if (isAI)
        {
            playerTransform = GameManager.Instance.ai_Player.transform;
            transform.parent = playerTransform;
        }
        else
        {
            playerTransform = GameManager.Instance.player.transform;
            transform.parent = playerTransform;
        }
        transform.localPosition = Vector3.zero;
        coolTime = skillData.cooltimes[0];

        if (skillData.skillType == SkillData.SkillType.����)
            if (skillData.bulletPrefab != null)
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

        //���߿� �߰��� ���⿡�� ������ ����ǵ���
        //�� �Լ��� �����ִ� �ֵ� �� �����ض� ���
        playerTransform.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
        playerTransform.BroadcastMessage("ApplyCooldown", SendMessageOptions.DontRequireReceiver);
    }

    void Fire()
    {
        if(skillData.skillID == 26)
        {
            GameManager.Instance.player.shield.AddShield();
            return;
        }
        if (!playerTransform.GetComponent<Scanner>().nearestTarget)
            return;
        GameObject bullet = GameManager.Instance.pool.Get(prefabId);
        bullet.GetComponent<BulletBase>().Init(isAI, skillData, level); // ���� ȣ��ǰų� ������ �ÿ��� ���ǹ���
    }

    void ApplyCooldown()
    {
        int[] spawnSkillId = { 3, 6, 7, 8 };
        int[] attackSkillId = { 0, 1, 2, 5, 10, 11, 12, 13 };//��... ��ų �����Ϳ� enum �� ĭ �� �ְ�;����� �ڵ��

        if (spawnSkillId.Contains(skillData.skillID))
            coolTime = skillData.cooltimes[level] * GameManager.Instance.player.spawnSkillCoolDownRate;

        if (attackSkillId.Contains(skillData.skillID))
            coolTime = skillData.cooltimes[level] * GameManager.Instance.player.attackSkillCoolDownRate;
    }
}
