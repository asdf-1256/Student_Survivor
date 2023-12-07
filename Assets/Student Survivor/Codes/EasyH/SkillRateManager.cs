using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRateManager : MonoBehaviour
{
    public static SkillRateManager instance;
    public LevelUpSkill LevelUpObject;


    public SkillData[] SkillDatas;

    [Header("# Skill Rates")]
    public float[] ratesOnSubject; // 교양
    public float[] ratesOnGrade1; // 전공 1학년
    public float[] ratesOnGrade2; // 전공 2학년
    public float[] ratesOnGrade3; // 전공 3학년
    public float[] ratesOnGrade4; // 전공 4학년


    private void Awake()
    {
        instance = this;
    }

    public void updateSkillRate(int phase)
    {
        // 현재 페이즈(0~7)에 맞게 스킬 등장 비율 재구성
        for (int i=0; i<SkillDatas.Length; i++)
        {
            
            int grade = SkillDatas[i].grade;
            if (grade == 0)
                LevelUpObject.setRate(i, ratesOnSubject[Mathf.Min(phase, ratesOnSubject.Length - 1)]);
            else if (grade == 1)
                LevelUpObject.setRate(i, ratesOnGrade1[Mathf.Min(phase, ratesOnGrade1.Length - 1)]);
            else if (grade == 2)
                LevelUpObject.setRate(i, ratesOnGrade2[Mathf.Min(phase, ratesOnGrade2.Length - 1)]);
            else if (grade == 3)
                LevelUpObject.setRate(i, ratesOnGrade3[Mathf.Min(phase, ratesOnGrade3.Length - 1)]);
            else if (grade == 4)
                LevelUpObject.setRate(i, ratesOnGrade4[Mathf.Min(phase, ratesOnGrade4.Length - 1)]);
            else
                Debug.LogError("학년초과에러!!");
        } 
    }

}
