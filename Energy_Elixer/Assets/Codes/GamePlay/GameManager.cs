using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool isGameOver;
    public GameObject gameOverPanel;
    private void Awake(){
        isGameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isGameOver){
            gameOverPanel.SetActive(true);
        }
    }

    public void Restart(){
        isGameOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}
