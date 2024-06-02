using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Choice : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI choiceText;
    [SerializeField] Image choiceImage;

    private Sprite defaultSprite;
    private int choiceIndex;
    System.Action<int> onButtonPushed;




    void Awake() {
        defaultSprite = choiceImage.sprite;
    }

    public void InitializeChoice(System.Action<int> onButtonPushAction, int index) {
        choiceIndex = index;
        onButtonPushed = onButtonPushAction;
    }


    void Start()
    {
        if(onButtonPushed == null) {
            throw new System.Exception("onButtonPushed is not set. Please set it in parent's awake function before using this class.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetChoiceText(string text) {
        choiceText.text = text;
    }
    public void SetChoiceImage(Sprite sprite) {
        choiceImage.sprite = sprite;
    }
    public void ResetChoiceImage() {
        choiceImage.sprite = defaultSprite;
    }

    public void SetChoiceLocation(Vector2 location) {
        choiceImage.rectTransform.anchoredPosition = location;
    }

    public void OnButtonPushed() {
        onButtonPushed?.Invoke(choiceIndex);
    }
}
