using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Required when Using SceneManagement


public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pnlActiveGame;
    [SerializeField] private GameObject pnlInactiveGame;

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
        SceneLoader.Instance.LoadGameScene();
    }
    public void OnLoadGameBtnClicked() {
        Debug.Log("Load Game Button Clicked");
        //implement loading of saved game
    }
}
