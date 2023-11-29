using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupBuff : MonoBehaviour
{
    public BuffData buff;
    public DataManager data;


    [SerializeField]
    public float attackRate; //공격력 버프
    public float speedRate; //속도 버프
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
            case BuffData.BuffEffect.Magnetic:
                GetComponentInChildren<Magnet>().MagneticRate *= buff.value;
                //GetComponent<Button>().interactable = false;
                break;
            case BuffData.BuffEffect.Speed:
                speedRate *= buff.value;
                break;
            case BuffData.BuffEffect.Invincible:
                isInvincible = true;
                break;
        }
    }





}
