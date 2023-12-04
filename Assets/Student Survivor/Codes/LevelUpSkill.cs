using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpSkill : MonoBehaviour
{
    public float[] SkillSelectRates;
    //UI�� rect transform
    RectTransform rect;
    SkillSelect[] skillSelects;

    public int skillCount = 3; // ���� ������ ��ų ����
    int[] selectedNums = new int[30]; // ���� �� �ִ� ��ų ���� 30����

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        skillSelects = GetComponentsInChildren<SkillSelect>(true);
    }

    public void Show()
    {
        Next();
        rect.localScale = Vector3.one;
        GameManager.Instance.Stop();

        AudioManager.Instance.PlaySfx(AudioManager.Sfx.LevelUp);
        AudioManager.Instance.EffectBgm(true);

        CheckSelectButtonActive();
    }
    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager.Instance.Resume();

        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Select);
        AudioManager.Instance.EffectBgm(false);
    }
    public void Select(int index)
    {
        skillSelects[index].OnClick();
    }

    void Next()
    {
        // 1. ��� ������ ��Ȱ��ȭ
        foreach (SkillSelect skill in skillSelects)
        {
            skill.gameObject.SetActive(false);
        }

        /*
        // 2. �� �߿��� ���� 3�� ������ Ȱ��ȭ
        while (true)
        {
            selectedNums[0] = Random.Range(0, skillSelects.Length);
            selectedNums[1] = Random.Range(0, skillSelects.Length);
            selectedNums[2] = Random.Range(0, skillSelects.Length);

            if (selectedNums[0] != selectedNums[1] && selectedNums[1] != selectedNums[2] && selectedNums[0] != selectedNums[2])
                break;
        }
*/
        int count = 0;
        for (int index = 0; count < skillCount && index < selectedNums.Length; index++)
        {
            selectedNums[index] = SelectRandomNum(index); // selectedNums �迭�� ������ ��ų��ȣ �ֱ�
            SkillSelect randomSkill = skillSelects[selectedNums[index]];
            Debug.Log(index + "��° ���� : " + selectedNums[index]);

            if (QuestManager.Instance.IsQuestDoing(randomSkill.skillData.skillName)) {
                Debug.Log(index + "��° : ���� ������");
            }
            // ���� ��ų�̶�� �ٽ� �̱�
            else if (randomSkill.level == randomSkill.skillData.damages.Length)
            {
                Debug.Log(index + "��° : �����̳�?");
            }
            else
            {
                randomSkill.gameObject.SetActive(true);
                count++;
            }
        }

    }
    int SelectRandomNum(int ignoreCount) // �̹� ���õ� ��ų ������ ignoreCount�� ����
    {
        int index;
        float AllSkillRange = 0;

        // 
        for (int i=0; i<SkillSelectRates.Length; i++)
        {
            AllSkillRange += SkillSelectRates[i];
        }
        for (int i=0; i<ignoreCount; i++)
        {
            AllSkillRange -= SkillSelectRates[selectedNums[i]];
        }

        float randomFloat = Random.Range(0f, AllSkillRange);

        for (index=0; index<SkillSelectRates.Length; index++)
        {
            bool isAlreadySelected = false;
            for (int i=0; i<ignoreCount; i++)
            {
                if (index == selectedNums[i])
                {
                    isAlreadySelected = true;
                }
            }
            if (isAlreadySelected)
            {
                continue;
            }
            randomFloat -= SkillSelectRates[index];
            if (randomFloat < 0)
            {
                return index;
            }
        }
        return index;
    }

    public void setRate(int index, float rate)
    {
        SkillSelectRates[index] = rate;
    }

    private void CheckSelectButtonActive()
    {
        foreach (SkillSelect skill in skillSelects)
        {
            if (skill.gameObject.activeSelf)
            {
                return;
            }
        }

        Hide();
        UIManager.Instance.Notice("�� �̻� ������ �� �ִ� ��ų�� �����ϴ�.");
        Debug.Log("������ �� �ִ� ��ų ���ٴ� �κ��� �����");
    }
}
