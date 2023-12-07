using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class CharacterStore : MonoBehaviour
{
    [SerializeField] private GameObject[] notPurchasedCharacterButtons; //구매하지 않은 캐릭터에 대해 밑에 코인 표시가 떠있는 버튼, 기본 캐릭터는 제외됨
    //index 0~2
    [SerializeField] private GameObject[] pressedCharacterButtons; //회색, 눌려있는 상태의 버튼
    //index 0~3
    [SerializeField] private GameObject[] purchaseButtonWindows; //구매하시겠습니까 yes or no 버튼이 떠있는 화면, 기본 캐릭터는 제외됨
    //index 0~2
    [SerializeField] private GameObject[] unpressedCharacterButtons; //구매된 캐릭터 중 선택되지 않은 상태의 버튼.
    //index 0~3
    [SerializeField] private GameObject[] selectedCharacterWindows; //선택된 캐릭터가 표시되는 창
    //index 0~3

    private readonly int[] prices = { 0, 200, 300, 400 };

    //구매된건 캐릭 선택 버튼이 뜨고.
    //구매 안 된건 코인 표시 있는 버튼이 뜨는데, 돈이 없으면 버튼 못 누르게 막기
    //구매 버튼 누르면 구매한다.
    //No를 누르면 선택된 캐릭터 표시시키고, 캐릭터 눌린 상태에 해금상태 따라서 다시 표시

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
            UIManager.Instance.Notice("돈이 부족합니다!!");
        }

        Init();
    }
}
