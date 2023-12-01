using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField]
    SkillData[] skillDatas;
    [SerializeField]
    List<Skill> skillList;

    private void Awake()
    {
        skillList = new List<Skill>();
    }

    public void AddSkill(int index)
    {
        Debug.Log("addskillȣ��");
        skillList.Add(new Skill(skillDatas[index]));
    }

    //�ΰ����� ��ų ��� ������...
    void RemoveAllSkill()
    {
        skillList.Clear();
    }

    public void LevelUp(int skillId)
    {
        foreach (Skill skill in skillList)
        {
            if(skill.data.skillID == skillId)
            {
                skill.LevelUp();
            }
        }
    }


}

[System.Serializable]
public class Skill {
    public SkillData data;
    public int level;
    public float remainTime;
    public float cooltime;
    public Skill(SkillData data)
    {
        this.level = 0;
        this.data = data;
        this.cooltime = data.cooltimes[level];
        this.remainTime = data.cooltimes[level];
    }

    public void LevelUp()
    {
        Debug.Log(string.Format("���� : {0} ���� ������ �Լ� ȣ��", level));
        level++;
        this.cooltime = data.cooltimes[level];
    }
}

//