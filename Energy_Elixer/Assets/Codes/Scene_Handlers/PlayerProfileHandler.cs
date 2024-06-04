using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class PlayerProfileHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text userName;

    [SerializeField] private TMP_InputField firstNameField;
    [SerializeField] private TMP_InputField lastNameField;
    [SerializeField] private TMP_InputField NICField;
    [SerializeField] private TMP_InputField phoneField;
    [SerializeField] private TMP_InputField emailField;


    [SerializeField] private Button backBtn;
    [SerializeField] private Button saveBtn;
    [SerializeField] private Button resetBtn;

    private UserProfile userProfile;


    private void Awake() {
        backBtn.onClick.AddListener(OnBackBtnClick);
        saveBtn.onClick.AddListener(OnSaveBtnClick);
        resetBtn.onClick.AddListener(OnResetBtnClick);
    }


    // Start is called before the first frame update
    void Start()
    {
        userProfile = PlayerManager.Instance.GetUserProfile();
        SetText();

    }

    private void SetText() {
        userName.text = userProfile.user.username;
        firstNameField.text = userProfile.user.firstname;
        lastNameField.text = userProfile.user.lastname;
        NICField.text = userProfile.user.nic;
        phoneField.text = userProfile.user.phoneNumber;
        emailField.text = userProfile.user.email;
    }

    public void OnBackBtnClick() {
        SoundManager.Instance.PlaySFX("button_clicked");
        SceneLoader.Instance.LoadMainMenuScene();
    }
    public void OnSaveBtnClick() {
        SoundManager.Instance.PlaySFX("button_clicked");
        UserProfile tempUser = new();


/*        Debug.Log(userName.text);
        Debug.Log(firstNameField.text);
        Debug.Log(lastNameField.text);
        Debug.Log(NICField.text);
        Debug.Log(phoneField.text);
        Debug.Log(emailField.text);*/

        tempUser.user.username = userName.text;
        tempUser.user.firstname = firstNameField.text;
        tempUser.user.lastname = lastNameField.text;
        tempUser.user.nic = NICField.text;
        tempUser.user.phoneNumber = phoneField.text;
        tempUser.user.email = emailField.text;

        APIHandler.Instance.UpdatePlayerProfile(tempUser);
    }

    public void OnResetBtnClick() {
        SoundManager.Instance.PlaySFX("button_clicked");
        SetText();
    }


}
