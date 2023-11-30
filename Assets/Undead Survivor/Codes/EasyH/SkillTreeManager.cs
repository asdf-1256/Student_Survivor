using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{
    public static SkillTreeManager instance;

    class SkillLevelPair
    {
        string _skillName;
        int _skillLevel;
        public SkillLevelPair(string skillName)
        {
            _skillName = skillName;
            _skillLevel = 1;
        }
        public bool isMatch(string skillName)
        {
            return (_skillName == skillName);
        }
        public void SkillLevelUp()
        {
            _skillLevel++;
        }
        public string getSkillInfo()
        {
            return _skillName + "(" + _skillLevel.ToString() + ") ";
        }
    }

    List<SkillLevelPair> _skillLevelPairs;

    private void Awake()
    {
        instance = this;
        _skillLevelPairs = new List<SkillLevelPair>();

    }

    public void AddSkillLevelPair(string skillName)
    {
        foreach (var skill in _skillLevelPairs)
        {
            if (skill.isMatch(skillName))
            {
                skill.SkillLevelUp();
                return;
            }
        }
        _skillLevelPairs.Add(new SkillLevelPair(skillName));
    }

    public string tellMeSkillTree()
    {
        string result = "��ųƮ�� : ";
        foreach (var skill in _skillLevelPairs)
        {
            result += skill.getSkillInfo();
        }
        return result;
    }

    bool containSkill(string skillName)
    {
        foreach (var skill in _skillLevelPairs)
        {
            if (skill.isMatch(skillName))
                return true;
        }
        return false;
    }

    List<SkillLevelPair> getSkillTree()
    {
        return _skillLevelPairs;
    }
}