using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelect : MonoBehaviour
{
    // �� ���� item ��Ʈ��Ʈ
    public SkillData skillData;
    public int level;
    public BasedSkill skill, AIskill; // �� ���� weapon
    public Gear gear;
    public QuestData questData;
    public QuestManager quest;

    Image icon;
    Text textLevel;
    Text textName;
    Text textDesc;

    private void Awake()
    {
        level = 0;

        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = skillData.skillIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];
        textName = texts[1];
        textDesc = texts[2];
        //get components�� ������ hierarchy�� ������.

        textName.text = skillData.skillName;
    }

    private void OnEnable()
    {
        textLevel.text = "Lv." + (level);

        switch (skillData.skillType)
        {
            case SkillData.SkillType.전공:
                textDesc.text = string.Format(skillData.skillDesc);
                break;
            case SkillData.SkillType.교양:
                textDesc.text = string.Format(skillData.skillDesc);
                break;
        }


    }

    public void OnClick()
    {

        switch (skillData.skillType)
        {
            case SkillData.SkillType.전공:
                if (!quest)
                {
                    GameObject newQuest = new GameObject();
                    quest = newQuest.AddComponent<QuestManager>();
                }
                quest.SetQuest(questData, skillData);
                
                if (skillData.skillID == 15) // �ΰ����� ��ų�̶��
                {
                    GameManager.Instance.ai_Player.gameObject.SetActive(true);
                }
                level++;
                /*
                else if (level == 0)
                {
                    GameObject newSkill = new GameObject();
                    GameObject newAISkill = new GameObject();
                    skill = newSkill.AddComponent<BasedSkill>();
                    AIskill = newAISkill.AddComponent<BasedSkill>();

                    skill.Init(false, skillData);
                    AIskill.Init(true, skillData);
                    level++;
                }
                else
                {
                    skill.LevelUp();
                    AIskill.LevelUp();
                    level++;
                }
*/
                break;
            case SkillData.SkillType.교양:
                GEActive();
                break;
        }
        if (level == skillData.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }

    private void GEActive()
    {
        switch (skillData.getype)
        {
            case SkillData.GEType.Speed:
                GameManager.Instance.player.speedRate *= skillData.damages[level];
                break;
            case SkillData.GEType.Size:
                //GameManager.Instance.player.transform.localScale = Vector3.one * skillData.damages[level];
                GameManager.Instance.player.Scale = Vector3.one * skillData.damages[level];
                break;
            case SkillData.GEType.MagnetSize:
                GameManager.Instance.player.GetComponentInChildren<Magnet>().MagneticRate *= skillData.damages[level];
                break;
            case SkillData.GEType.Attack:
                GameManager.Instance.player.attackRate *= skillData.damages[level];
                break;
            case SkillData.GEType.Defense:
                GameManager.Instance.player.defenseRate *= skillData.damages[level];
                break;
            case SkillData.GEType.EXP:
                GameManager.Instance.expRate *= skillData.damages[level];
                break;
            case SkillData.GEType.MaxHealth:
                GameManager.Instance.maxHealth += skillData.damages[level];
                GameManager.Instance.health += Convert.ToInt32(skillData.damages[level]);
                break;
            case SkillData.GEType.Recovery:
                if (level == 0)
                {
                    GameObject newSkill = new GameObject();
                    skill = newSkill.AddComponent<BasedSkill>();
                    skill.Init(false, skillData);
                }
                else
                {
                    skill.LevelUp();
                }
                break;
            case SkillData.GEType.AttackCoolDownReduction:
                GameManager.Instance.player.attackSkillCoolDownRate = skillData.damages[level];
                GameManager.Instance.player.BroadcastMessage("ApplyCooldown", SendMessageOptions.DontRequireReceiver);
                break;
            case SkillData.GEType.SpawnCoolDownReduction:
                GameManager.Instance.player.spawnSkillCoolDownRate = skillData.damages[level];
                GameManager.Instance.player.BroadcastMessage("ApplyCooldown", SendMessageOptions.DontRequireReceiver);
                break;
        }
        level++;
    }
}