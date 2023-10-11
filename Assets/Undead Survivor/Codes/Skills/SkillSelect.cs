using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelect : MonoBehaviour
{
    public SkillData skillData;
    public int level;
    public Weapon weapon;
    public Gear gear;

    Image icon;
    Text textLevel;
    Text textName;
    Text textDesc;

    private void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = skillData.skillIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];
        textName = texts[1];
        textDesc = texts[2];
        //get components의 순서는 hierarchy에 따른다.

        textName.text = skillData.skillName;
    }

    private void OnEnable()
    {
        textLevel.text = "Lv." + (level + 1);

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
/*
    public void OnClick()
    {
        switch (skillData.skillType)
        {
            case SkillData.SkillType.전공:
                if (level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(skillData);
                }
                else
                {
                    float nextDamage = skillData.baseDamage;
                    int nextCount = 0;

                    nextDamage += skillData.baseDamage * skillData.damages[level];
                    nextCount += skillData.counts[level];

                    weapon.LevelUp(nextDamage, nextCount);
                }
                level++;
                break;
            case SkillData.SkillType.교양:
                if (level == 0)
                {
                    GameObject newGear = new GameObject();
                    gear = newGear.AddComponent<Gear>();
                    gear.Init(skillData);
                }
                else
                {
                    float nextRate = skillData.damages[level];
                    gear.LevelUp(nextRate);

                }
                level++;
                break;
        }
        if (level == skillData.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }*/
}