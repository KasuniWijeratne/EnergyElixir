using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Required when Using SceneManagement


public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pnlActiveGame;
    [SerializeField] private GameObject pnlInactiveGame;
    [SerializeField] private GameObject pnlMainMenu;
    [SerializeField] private GameObject PnlRedirect;

    private void Awake() {
        if (DatabaseHandler.Instance != null) {
            DatabaseHandler.Instance.OnPlayerInfoRetrived += OnPlayerInfoRetrieved;
        }else {
            Debug.LogError("DatabaseHandler is null");
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(PlayerManager.Instance.IsPlayerProfileCompleted);
        if (PlayerManager.Instance.IsPlayerProfileCompleted) {
            pnlActiveGame.SetActive(true);
            pnlInactiveGame.SetActive(false);
        }
        else {
            pnlActiveGame.SetActive(false);
            pnlInactiveGame.SetActive(true);
        }
        
    }

    public void OnProfileButtonClicked() {
        SceneLoader.Instance.LoadPlayerProfileScene();
    }

    public void OnQuitBtnClicked() {
        Application.Quit();
    }

    public void OnNewGameBtnClicked() {
        //check if the player has completed the questionnaire
        DatabaseHandler.Instance.GetPlayerInformation();
    }

    public void OnPlayerInfoRetrieved() {
        bool qnCompleted = PlayerManager.Instance.IsPlayerQuestionnaireCompleted;

        if (qnCompleted) {
            SceneLoader.Instance.LoadGameScene();
        }
        else {
            pnlMainMenu.SetActive(false);
            PnlRedirect.SetActive(true);
        }
    }


    public void OnLoadGameBtnClicked() {
        Debug.Log("Load Game Button Clicked");
        //implement loading of saved game
    }

    public void OnRedirectBtnClicked() {
        Application.OpenURL("http://localhost:5173/");
    }
}
