using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRateManager : MonoBehaviour
{
    public static SkillRateManager instance;
    public LevelUpSkill LevelUpObject;


    public SkillData[] SkillDatas;

    [Header("# Skill Rates")]
    public float[] ratesOnSubject; // ����
    public float[] ratesOnGrade1; // ���� 1�г�
    public float[] ratesOnGrade2; // ���� 2�г�
    public float[] ratesOnGrade3; // ���� 3�г�
    public float[] ratesOnGrade4; // ���� 4�г�


    private void Awake()
    {
        instance = this;
    }

    public void updateSkillRate(int phase)
    {
        // ���� ������(0~7)�� �°� ��ų ���� ���� �籸��
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
                Debug.LogError("�г��ʰ�����!!");
        } 
    }

}
