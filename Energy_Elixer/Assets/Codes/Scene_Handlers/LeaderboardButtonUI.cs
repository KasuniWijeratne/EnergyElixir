using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaderboardButtonUI : MonoBehaviour
{
    [SerializeField] private string mainMenu = "MainMenu";

    public void BackButton(){
        SceneManager.LoadScene(mainMenu);
    }
}
