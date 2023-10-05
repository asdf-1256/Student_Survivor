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
        Debug.Log("addskill호출");
        skillList.Add(new Skill(skillDatas[index]));
    }

    //인공지능 스킬 어떻게 만들지...
    void RemoveAllSkill()
    {
        skillList.Clear();
    }
    private void Update()
    {
        foreach (Skill skill in skillList)
        {
            skill.remainTime -= Time.deltaTime;
            if (skill.remainTime <= 0f)
                skill.Activate();
        }
    }

    public void LevelUp(int skillId)
    {
        foreach (Skill skill in skillList)
        {
            if(skill.data.id == skillId)
            {
                skill.LevelUp();
            }
        }
    }


}

[System.Serializable]
class Skill {
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
    public void Activate()
    {
        //공격스킬이라면
        //GameManager.inst.pool.get(pool's index)
        GameManager.Instance.pool.Get(data.pool_index).GetComponent<SkillBase>().Init(level);

        //버프스킬이라면
        //스탯변화.... --방어막 추가, 일정 시간마다 체력회복

        //반영구적인 스탯의 변화는? - 공격력 증가 같은거?
        //Skill 클래스 멤버로 isActivated 같은거 둬야하나.

        this.remainTime = cooltime;
    }

    public void LevelUp()
    {
        Debug.Log(string.Format("레벨 : {0} 에서 레벨업 함수 호출", level));
        level++;
        this.cooltime = data.cooltimes[level];
    }
}

//