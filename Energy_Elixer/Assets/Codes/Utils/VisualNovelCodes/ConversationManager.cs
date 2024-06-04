using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using Unity.VisualScripting;

public class ConversationManager : MonoBehaviour
{
    [SerializeField] Image leftCharacterImg;
    [SerializeField] Image rightCharacterImg;
    [SerializeField] Image middleCharacterImg;
    [SerializeField] TMPro.TextMeshProUGUI conversationText;
    [SerializeField] TMPro.TextMeshProUGUI nameText;
    [SerializeField] GameObject TextBox;

    private System.Action onNextBtnPushed;
    private Dictionary<string, string> characterLocations;  // key: character name, value: location
    // Start is called before the first frame update
    void Start()
    {
    }



    public void disableConversationManager()
    {
        UpdateCharacters();
        ChangeConversationStatus(false);
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void SetNextBtnPushedAction(System.Action action)
    {
        onNextBtnPushed = action;
    }

    public void OnNextPushed()
    {
        if (onNextBtnPushed != null)
            onNextBtnPushed?.Invoke();
        else
            throw new System.Exception("onNextBtnPushed is not set. Please set it in parent's awake function before using this class.");
    }
    public void SetCharacterLocations(Dictionary<string, string> characterL)
    {
        characterLocations = characterL;
    }
    public void UpdateConversation(string characterName, string conversation)
    {
        TextBox.SetActive(true);
        if (conversation != null && conversation != "_")
            conversationText.text = conversation;
        else
            conversationText.text = "";
        if (characterName != null && characterName != "_")
            nameText.text = characterName + ": ";
        else
            nameText.text = "";
    }

    public void ChangeConversationStatus(bool status)
    {
        TextBox.SetActive(status);
    }

    public void UpdateCharacters(string leftCharacter = null, string rightCharacter = null, string middleCharacter = null)
    {
        UpdateCharacter(leftCharacter, leftCharacterImg);
        UpdateCharacter(rightCharacter, rightCharacterImg);
        UpdateCharacter(middleCharacter, middleCharacterImg);
    }

    void UpdateCharacter(string characterName, Image characterImg)
    {
        if (characterName != null && characterName != "_")
        {
            Sprite characterSprite = Resources.Load<Sprite>(characterLocations[characterName]);
            if (characterSprite != null)
            {
                characterImg.sprite = characterSprite;
                characterImg.enabled = true;
            }
            else
            {
                characterImg.enabled = false;
                throw new System.Exception("Character sprite not found at: " + characterLocations[characterName]);
            }
        }
        else
            characterImg.enabled = false;
    }
}
