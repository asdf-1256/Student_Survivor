using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    //List<SkillData> skillDatas;
    List<Skill> skillList;

    private void Awake()
    {
        skillList = new List<Skill>();
        //skillDatas = new List<SkillData>();
    }

    void AddSkill(Skill skill)
    {
        skillList.Add(skill);
    }
    void RemoveAllSkill()
    {
        skillList.Clear();
    }
    private void Update()
    {
        foreach (Skill skill in skillList)
        {
            skill.cooltime -= Time.deltaTime;
            if (skill.cooltime <= 0f)
                skill.Activate();
        }
    }



}
