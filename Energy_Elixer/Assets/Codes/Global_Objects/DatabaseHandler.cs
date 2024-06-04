using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.Networking;
//using Newtonsoft.Json.Linq;
using UnityEngine;

public class DatabaseHandler : MonoBehaviour
{
    private static DatabaseHandler instance; // Singleton instance
    private const string baseUrl = "http://localhost:8080";

    public event System.Action OnPlayerInfoRetrived;

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
    private IEnumerator SendGetRequest(string path, System.Action<string> onSuccess, System.Action<string> onError) {
 

        string url = $"{baseUrl}/{path}";
        using (UnityWebRequest request = UnityWebRequest.Get(url)) {
            yield return request.SendWebRequest();  // Wait for the request to complete

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError) {
                Debug.Log("Error in GET request: " + request.error);
                onError?.Invoke(request.error);
            }
            else {
                onSuccess?.Invoke(request.downloadHandler.text);  // Handle the successful response
            }
        }
    }

    public void GetPlayerInformation() {
        StartCoroutine(SendGetRequest("player/getPlayer", HandlePlayerInfoSuccess, HandlePlayerInfoError));
    }

    private void HandlePlayerInfoSuccess(string response) {
        Debug.Log("Player data successfully fetched: " + response);
        //check wheather the json response has a body
        if (IsJsonObjectEmpty(response)){
            PlayerManager.Instance.IsPlayerQuestionnaireCompleted = false;
        }
        else {
            PlayerManager.Instance.IsPlayerQuestionnaireCompleted = true;
        }
        OnPlayerInfoRetrived?.Invoke();
    }

    private static bool IsJsonObjectEmpty(string json) {
        json = json.Trim(); // Make sure to trim whitespace which might affect the check
        if (json.Equals("{}") || string.IsNullOrEmpty(json)) {
            return true;
        }
        // Optionally add more complex checks here
        return true; // change this
    }

    private void HandlePlayerInfoError(string error) {
        Debug.Log("Error fetching profile: " + error);
        PlayerManager.Instance.IsPlayerQuestionnaireCompleted = true; // For testing purposes
        OnPlayerInfoRetrived?.Invoke();
    }

}
