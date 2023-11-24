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

    public void updateSkillRate(int pahse)
    {
        // 현재 페이즈(0~7)에 맞게 스킬 등장 비율 재구성

    }

}
