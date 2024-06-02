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



    public void disableConversationManager() {
        UpdateCharacters();
        ChangeConversationStatus(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetNextBtnPushedAction (System.Action action) {
        onNextBtnPushed = action;
    }

    public void OnNextPushed() {
        if (onNextBtnPushed != null)
            onNextBtnPushed?.Invoke();
        else 
            throw new System.Exception("onNextBtnPushed is not set. Please set it in parent's awake function before using this class.");
    }
    public void SetCharacterLocations(Dictionary<string, string> characterL) {
        characterLocations = characterL;
    }
    private Sprite LoadCharacter(string characterName) {
        if (characterName == null || characterName == "") return null;
        string characterLocation = characterLocations[characterName];
        Sprite characterSprite = Resources.Load<Sprite>(characterLocation);
        if (characterSprite == null) {
            throw new System.Exception("Character sprite not found. Please check the character name and the character location in the character location file.");
        }
        return characterSprite;
    }
    public void UpdateConversation(string characterName, string conversation) {
        TextBox.SetActive(true);
        if(conversation != null && conversation != "_")
            conversationText.text = conversation;
        else 
            conversationText.text = "";
        if(characterName != null && characterName != "_")
            nameText.text = characterName + ": ";
        else
            nameText.text = "";
    }

    public void ChangeConversationStatus(bool status) {
        TextBox.SetActive(status);
    }

    public void UpdateCharacters(string leftCharacter = null, string rightCharacter = null, string middleCharacter = null)
    {
        UpdateCharacter(leftCharacter, leftCharacterImg);
        UpdateCharacter(rightCharacter, rightCharacterImg);
        UpdateCharacter(middleCharacter, middleCharacterImg);
        void UpdateCharacter(string middleCharacter, Image characterImg)
        {
            if (middleCharacter != null && middleCharacter != "_"){
                characterImg.enabled = true;
                characterImg.sprite = LoadCharacter(middleCharacter);
            }
            else
                characterImg.enabled = false;
        }
    }
}
