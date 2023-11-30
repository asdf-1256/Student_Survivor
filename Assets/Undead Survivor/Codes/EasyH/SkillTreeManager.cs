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
            return _skillName + "(" + levelToGrade(_skillLevel - 1) + ") ";
        }
        string levelToGrade(int level)
        {
            switch (level)
            {
                case 0:
                    return "C+";
                case 1:
                    return "B0";
                case 2:
                    return "B+";
                case 3:
                    return "A0";
                case 4:
                    return "A+";
                default:
                    return "error!!";
            }
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
        string result = "스킬트리 : ";
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
