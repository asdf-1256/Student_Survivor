using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class CharacterStore : MonoBehaviour
{
    [SerializeField] private GameObject[] notPurchasedCharacterButtons; //�������� ���� ĳ���Ϳ� ���� �ؿ� ���� ǥ�ð� ���ִ� ��ư, �⺻ ĳ���ʹ� ���ܵ�
    //index 0~2
    [SerializeField] private GameObject[] pressedCharacterButtons; //ȸ��, �����ִ� ������ ��ư
    //index 0~3
    [SerializeField] private GameObject[] purchaseButtonWindows; //�����Ͻðڽ��ϱ� yes or no ��ư�� ���ִ� ȭ��, �⺻ ĳ���ʹ� ���ܵ�
    //index 0~2
    [SerializeField] private GameObject[] unpressedCharacterButtons; //���ŵ� ĳ���� �� ���õ��� ���� ������ ��ư.
    //index 0~3
    [SerializeField] private GameObject[] selectedCharacterWindows; //���õ� ĳ���Ͱ� ǥ�õǴ� â
    //index 0~3

    private readonly int[] prices = { 0, 200, 300, 400 };

    //���ŵȰ� ĳ�� ���� ��ư�� �߰�.
    //���� �� �Ȱ� ���� ǥ�� �ִ� ��ư�� �ߴµ�, ���� ������ ��ư �� ������ ����
    //���� ��ư ������ �����Ѵ�.
    //No�� ������ ���õ� ĳ���� ǥ�ý�Ű��, ĳ���� ���� ���¿� �رݻ��� ���� �ٽ� ǥ��

    private Button[] notPurchasedCharacterButtonComponents;
    private Text[] notPurchasedCharacterTexts;
    private Text[] selectedCharacterWindowTexts;

    public string[] characterAbilityDescribes;
    private void Awake()
    {
        notPurchasedCharacterButtonComponents = new Button[notPurchasedCharacterButtons.Length];
        notPurchasedCharacterTexts = new Text[notPurchasedCharacterButtons.Length];
        selectedCharacterWindowTexts = new Text[selectedCharacterWindows.Length];

        for (int i = 0; i < notPurchasedCharacterButtons.Length; i++)
        {
            selectedCharacterWindowTexts[i] = selectedCharacterWindows[i].GetComponentInChildren<Text>(true);
            selectedCharacterWindowTexts[i].text = characterAbilityDescribes[i];

            if (notPurchasedCharacterButtons[i] == null)
            {
                notPurchasedCharacterButtonComponents[i] = null;
                notPurchasedCharacterTexts[i] = null;
                continue;
            }
            notPurchasedCharacterButtonComponents[i] = notPurchasedCharacterButtons[i].GetComponent<Button>();
            notPurchasedCharacterTexts[i] = notPurchasedCharacterButtons[i].GetComponentInChildren<Text>();
            notPurchasedCharacterTexts[i].text = prices[i].ToString();
        }
        Init();
    }
    private void OnEnable()
    {
        Init();
    }
    private void Update()
    {
        for (int i = 0; i < notPurchasedCharacterButtons.Length; i++)
        {
            if (notPurchasedCharacterButtonComponents[i] == null)
                continue;
            if (!DataManager.Instance.CheckMoney(prices[i]))
                notPurchasedCharacterButtonComponents[i].interactable = false;
            else
                notPurchasedCharacterButtonComponents[i].interactable = true;
        }
    }
    public void CharacterSelect(int characterId)
    {
        DisableAll();
        DataManager.Instance.SetSelectedCharacter(characterId);
        selectedCharacterWindows[characterId].SetActive(true);
        pressedCharacterButtons[characterId].SetActive(true);

        for (int i = 0; i < DataManager.Instance.isUnlockCharacters.Length; i++)
        {
            if (i == characterId)
                continue;
            if (unpressedCharacterButtons[i] != null)
                unpressedCharacterButtons[i].SetActive(DataManager.Instance.isUnlockCharacters[i]);
            if (notPurchasedCharacterButtons[i] != null)
                notPurchasedCharacterButtons[i].SetActive(!DataManager.Instance.isUnlockCharacters[i]);
        }
    }
    public void Init()
    {
        CharacterSelect(DataManager.Instance.selectedCharacterId);
    }
    public void ClickNotPurchasedCharBtn(int characterId) 
    {
        foreach (var obj in selectedCharacterWindows)
        {
            if (obj == null)
                continue;
            obj.SetActive(false);
        }
        foreach (var obj in purchaseButtonWindows)
        {
            if (obj == null)
                continue;
            obj.SetActive(false);
        }

        purchaseButtonWindows[characterId].SetActive(true);
    }
    private void DisableAll()
    {
        foreach (var obj in notPurchasedCharacterButtons)
        {
            if (obj == null)
                continue;
            obj.SetActive(false);
        }
        foreach (var obj in pressedCharacterButtons)
        {
            if (obj == null)
                continue;
            obj.SetActive(false);
        }
        foreach (var obj in purchaseButtonWindows)
        {
            if (obj == null)
                continue;
            obj.SetActive(false);
        }
        foreach (var obj in unpressedCharacterButtons)
        {
            if (obj == null)
                continue;
            obj.SetActive(false);
        }
        foreach (var obj in selectedCharacterWindows)
        {
            if (obj == null)
                continue;
            obj.SetActive(false);
        }
    }
    public void ClickBuyButton(int characterId)
    {
        if (DataManager.Instance.CheckMoney(prices[characterId]))
        {
            DataManager.Instance.UnlockCharacter(characterId, prices[characterId]);
            DataManager.Instance.SetSelectedCharacter(characterId);
        }
        else
        {
            UIManager.Instance.Notice("���� �����մϴ�!!");
        }

        Init();
    }
}
