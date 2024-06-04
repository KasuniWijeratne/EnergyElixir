using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static bool isGameOver;
    public GameObject gameOverPanel;
    public TextMeshProUGUI scoreText;
    public static int score;
    private void Awake(){
        isGameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Energy: " + score;
        if(isGameOver){
            gameOverPanel.SetActive(true);
        }
    }

    public void Restart(){
        isGameOver = false;
        Debug.Log("Restarting Game");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        score = 0;

    }

}
