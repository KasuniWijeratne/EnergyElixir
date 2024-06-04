using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] Image background; 

    public void ChangeBackground(string backgroundName) {
        Sprite temp = Resources.Load<Sprite>(backgroundName);
        background.sprite = temp;
    }

    //remove background
    public void EnableBackground(bool enable = false) {
        background.enabled = enable;
    }

    public void UpdateBackground(string backgroundName, bool enable = false) {
        ChangeBackground(backgroundName);
        EnableBackground(enable);
    }

}
