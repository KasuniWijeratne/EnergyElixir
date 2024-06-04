using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceManager : MonoBehaviour
{
    [SerializeField] Choice[] choices;
    [SerializeField] Image choicesBackground;

    private Sprite defaultChoiceBackground;
    private string[][] choiceDetails;
    private System.Action<string> OnSelection;



    private void Awake() {
        defaultChoiceBackground = choicesBackground.sprite;
        for (int i = 0; i < choices.Length; i++) {
            choices[i].InitializeChoice(OnSelectionMade, i);
        }
    }

    public void disableChoiceManager() {
        choicesBackground.gameObject.SetActive(false);
        for (int i = 0; i < choices.Length; i++) {
            choices[i].gameObject.SetActive(false);
        }
    }

    private void Start() {
    }

    private void Update() {
    }

    //Choice Background Image
    public void SetChoiceBackground(Sprite sprite) {
        choicesBackground.sprite = sprite;
    }
    public void SetChoiceBackground() {
        choicesBackground.sprite = defaultChoiceBackground;
    }
    public void SetBackgroundHeight(float height) {
        choicesBackground.rectTransform.sizeDelta = new Vector2(choicesBackground.rectTransform.sizeDelta.x, height);
    }
    public void SetBackgroundWidth(float width) {
        choicesBackground.rectTransform.sizeDelta = new Vector2(width, choicesBackground.rectTransform.sizeDelta.y);
    }

    //Choice Button functions
    private void SetChoiceActive(Choice choice, string text, Vector2 location =default, Sprite sprite = null) {
        choice.gameObject.SetActive(true);
        choice.SetChoiceText(text);
        choice.SetChoiceLocation(location);
        if(sprite != null) {
            choice.SetChoiceImage(sprite);
        }
    }


    public void UpdateChoices(string[][] choiceUpdate, string backgroundLocation = "")
    { // [choice number][text, sprite]

        choiceDetails = choiceUpdate;

        UpdateBackground(choiceUpdate.Length, backgroundLocation);

        for (int i = 0; i < choiceUpdate.Length; i++)
        {
            Choice choice = choices[i];
            float height = 65 * (choices.Length + choiceUpdate.Length - 2 * i - 8);

            if (choiceUpdate[i].Length == 2)
            {
                SetChoiceActive(choice, choiceUpdate[i][0], new Vector2(0, height), Resources.Load<Sprite>(choiceUpdate[i][1]));
            }
            else
            {
                SetChoiceActive(choice, choiceUpdate[i][0], new Vector2(0, height));
            }
        }

        for (int i = choiceUpdate.Length; i < choices.Length; i++)
        {
            Choice choice = choices[i];
            choice.gameObject.SetActive(false);
        }
    }

    private void UpdateBackground(int backgroundLength, string backgroundLocation)
    {
        if (backgroundLocation != "")
        {
            SetChoiceBackground(Resources.Load<Sprite>(backgroundLocation));
        }
        choicesBackground.gameObject.SetActive(true);

        SetBackgroundHeight(130 * backgroundLength + 70);
    }

    public void SetOnselectionMade(System.Action<string> onSelection) {
        OnSelection = onSelection;
    }

    public void OnSelectionMade(int choiceIndex) {
        //Debug.Log("Choice " + choiceIndex + " is selected.");
        if(OnSelection != null) {
            OnSelection.Invoke(choiceDetails[choiceIndex][2]);
        }
        else {
            throw new System.Exception("OnSelection is not set. Please set it in parent's awake function before using this class.");
        }
    }

    public void Disable() {
        bool enabled = false;
        choicesBackground.gameObject.SetActive(enabled);
        for (int i = 0; i < choices.Length; i++) {
            choices[i].gameObject.SetActive(enabled);
        }
    }
}
