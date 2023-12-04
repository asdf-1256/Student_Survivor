using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillLevelUpReward: QuestReward {
    int level = 0;

    BasedSkill skill, AIskill;
    internal SkillData skillData;

    public void Reward()
    {
        if (skillData.skillID == 26) //블록 체인이면
        {
            if (level == 0)
            {
                GameObject blockChainSKillObject = new GameObject();
                skill = blockChainSKillObject.AddComponent<BasedSkill>();
                skill.Init(false, skillData);
            }
            else
            {
                skill.LevelUp();
                level++;
            }
            return;
        }
        if (skillData.skillID == 15) // 인공지능 스킬이면 AI 오브젝트 활성화
        {
            GameManager.Instance.ai_Player.gameObject.SetActive(true);
            level++;
            return;
        }
        else if (level > 0)
        {
            skill.LevelUp();
            AIskill.LevelUp();
            level++;
            return;
        }

        GameObject newSkill = new GameObject();
        GameObject newAISkill = new GameObject();
        skill = newSkill.AddComponent<BasedSkill>();
        AIskill = newAISkill.AddComponent<BasedSkill>();

        skill.Init(false, skillData);
        AIskill.Init(true, skillData);
        level++;
    }
}

public class SkillSelect : MonoBehaviour
{
    // �� ���� item ��Ʈ��Ʈ
    public SkillData skillData;
    SkillLevelUpReward _questReward;
    public int level;
    public BasedSkill skill, AIskill; // �� ���� weapon
    //public Gear gear;
    public QuestData questData;

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
        textLevel.text = "현재 학점 : " + levelToGrade(level);

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
    string levelToGrade(int level) {
        switch (level)
        {
            case 0:
                return "F ";
            case 1:
                return "C+";
            case 2:
                return "B ";
            case 3:
                return "B+";
            case 4:
                return "A ";
            default:
                return "error!!";
        }
    }

    private void Start()
    {
        _questReward = new SkillLevelUpReward();
        _questReward.skillData = skillData;
    }

    public void OnClick()
    {

        switch (skillData.skillType)
        {
            case SkillData.SkillType.전공:
                if (!GameManager.Instance.CanAddQuest())
                {
                    UIManager.Instance.Notice(string.Format("풀강입니다! 공강이 없습니다?"));
                    Debug.Log("퀘스트가 꽉 차있습니다.");
                    break;
                }
                QuestManager.Instance.AddQuest(skillData.skillName, level, questData, _questReward);
                level++;
                break;
            case SkillData.SkillType.교양:
                SkillTreeManager.instance.AddSkillLevelPair(skillData.skillName);
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
                GameManager.Instance.player.SpriteTransform.localScale = Vector3.one * skillData.damages[level];
                GameManager.Instance.player.ChangeColliderSize(skillData.damages[level]);
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
                GameManager.Instance.HealthInHUD.GetComponent<RectTransform>().sizeDelta = new Vector2(GameManager.Instance.maxHealth / 10, 4);
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
                GameManager.Instance.player.attackSkillCoolDownRate *= skillData.damages[level];
                GameManager.Instance.player.BroadcastMessage("ApplyCooldown", SendMessageOptions.DontRequireReceiver);
                break;
            case SkillData.GEType.SpawnCoolDownReduction:
                GameManager.Instance.player.spawnSkillCoolDownRate *= skillData.damages[level];
                GameManager.Instance.player.BroadcastMessage("ApplyCooldown", SendMessageOptions.DontRequireReceiver);
                break;
        }
        level++;
    }
}