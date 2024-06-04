using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor; // Required for using SceneAsset in the editor
#endif

public class SceneLoader : MonoBehaviour {

    private static SceneLoader instance; // Singleton instance
                                         // Use SceneAsset for editor-time scene references
#if UNITY_EDITOR
    [SerializeField] private SceneAsset startGameSceneAsset;
    [SerializeField] private SceneAsset mainMenuSceneAsset;
    [SerializeField] private SceneAsset playerProfileSceneAsset;
    [SerializeField] private SceneAsset[] gameSceneAsset;
    
#endif

    // String to hold the path to the scene for runtime use
    private string startGameScenePath = "Assets/Scenes/StartPage.unity";
    private string mainMenuScenePath = "Assets/Scenes/MainMenu.unity";
    private string playerProfileScenePath = "Assets/Scenes/Player Profile.unity";
    private string[] gameScenePaths = {
        "Assets/Scenes/Game Env/GameEnv1.unity", // level 1
        "Assets/Scenes/Game Env/GameEnv2.unity" // level 2
        };
    private string mapScenePath = "Assets/Scenes/MapScene.unity";
    



    void Awake() {
        if (instance == null) {
            instance = this;
            GameObject apiHandlerObj = this.gameObject;

            // Ensure it's a root GameObject
            apiHandlerObj.transform.parent = null;

            DontDestroyOnLoad(apiHandlerObj); // Make it persistent across scenes
        }
        else if (instance != this) {
            Destroy(gameObject);
        }

        // Convert SceneAsset to path strings that can be used at runtime
#if UNITY_EDITOR
        if (startGameSceneAsset != null)
            startGameScenePath = AssetDatabase.GetAssetPath(startGameSceneAsset);
        if (mainMenuSceneAsset != null)
            mainMenuScenePath = AssetDatabase.GetAssetPath(mainMenuSceneAsset);
        if (playerProfileSceneAsset != null)
            playerProfileScenePath = AssetDatabase.GetAssetPath(playerProfileSceneAsset);
        if(gameSceneAsset != null)
        {
            gameScenePaths = new string[gameSceneAsset.Length];
            for (int i = 0; i < gameSceneAsset.Length; i++)
            {
                gameScenePaths[i] = AssetDatabase.GetAssetPath(gameSceneAsset[i]);
            }
        }

#endif
    }

    void OnDestroy() {
        Debug.Log("SceneLoader Destroyed");
        if (instance == this) {
            instance = null;
        }
    }

    public static SceneLoader Instance {
        get {
            if (instance == null) {
                // Optionally log error or create a new GameObject with SceneLoader
                Debug.LogError("SceneLoader is not instantiated yet.");
            }
            return instance;
        }
    }

    public void LoadScene(string sceneName) {
        try {
            SceneManager.LoadScene(sceneName);
        }
        catch (System.Exception e) {
            Debug.LogError("Scene not found: " + e.Message+" | Scene Path given:"+ sceneName);
        }
    }
    public void LoadStartGameScene() {
        SceneManager.LoadScene(startGameScenePath);
    }
    public void LoadMainMenuScene() {
        SceneManager.LoadScene(mainMenuScenePath);
    }
    public void LoadPlayerProfileScene() {
        SceneManager.LoadScene(playerProfileScenePath);
    }

    public void LoadGameScene(int level) {
        SceneManager.LoadScene(gameScenePaths[level - 1]);
    }

    public void LoadmapScene() {
        SceneManager.LoadScene(mapScenePath);
    }
}
