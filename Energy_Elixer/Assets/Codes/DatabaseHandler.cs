using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DatabaseHandler : MonoBehaviour
{
    private static DatabaseHandler instance; // Singleton instance
    private const string baseUrl = "http://";
    
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
    }

    public static DatabaseHandler Instance {
        get {
            if (instance == null) {
                // Optionally log error or create a new GameObject with DatabaseHandler
                Debug.LogError("DatabaseHandler is not instantiated yet.");
            }
            return instance;
        }
    }


    //append database handeling methods here..............






}
