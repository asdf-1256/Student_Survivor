using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupBuff : MonoBehaviour
{
    public BuffData buff;
    public DataManager data;


    [SerializeField]
    public float attackRate; //���ݷ� ����
    public float speedRate; //�ӵ� ����
    public float defenseRate;
    public bool isInvincible;

    Image icon;

    private void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = buff.image;

        attackRate = 1f;
        speedRate = 1f;
        defenseRate = 1f;
        //magneticRate = 1f;
        isInvincible = false;

    }

    private void Start()
    {
        UpdateButtonInteractable();
    }

    private void UpdateButtonInteractable()
    {
        if (DataManager.Instance.money <= 100)
        {
            GetComponent<Button>().interactable = false;
        }
        else
        {
            GetComponent<Button>().interactable = true;
        }
    }

    public void OnClick()
    {
        switch (buff.effect)
        {
            case BuffData.BuffEffect.�ڱ��:
                GetComponentInChildren<Magnet>().MagneticRate *= buff.value;
                //GetComponent<Button>().interactable = false;
                break;
            case BuffData.BuffEffect.�̵��ӵ�:
                speedRate *= buff.value;
                break;
            case BuffData.BuffEffect.����:
                isInvincible = true;
                break;
        }
    }





}
