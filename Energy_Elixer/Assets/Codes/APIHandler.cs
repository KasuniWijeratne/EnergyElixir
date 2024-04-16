using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Collections;

[System.Serializable]
public class AuthRequest {
    public string apiKey;
}


[System.Serializable]
public class UserProfile {
    public User user;

    [System.Serializable]
    public class User {
        public string firstname;
        public string lastname;
        public string username;
        public string nic;
        public string phoneNumber;
        public string email;
    }
}



public class APIHandler : MonoBehaviour {
    private static APIHandler instance; // Singleton instance

    private const string baseUrl = "http://20.15.114.131:8080";
    private string jwtToken;

    private IEnumerator SendGetRequest(string path, System.Action<string> onSuccess, System.Action<string> onError) {
        if (string.IsNullOrEmpty(jwtToken)) {
            Debug.LogError("JWT token is empty.");
            onError?.Invoke("JWT token is empty.");
            yield break;
        }

        string url = $"{baseUrl}/{path}";
        using (UnityWebRequest request = UnityWebRequest.Get(url)) {
            request.SetRequestHeader("Authorization", $"Bearer {jwtToken}");
            yield return request.SendWebRequest();  // Wait for the request to complete

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError) {
                Debug.LogError("Error in GET request: " + request.error);
                onError?.Invoke(request.error);
            }
            else {
                onSuccess?.Invoke(request.downloadHandler.text);  // Handle the successful response
            }
        }
    }

    void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject); // Ensure that there is only one instance
        }
        else {
            instance = this;
            DontDestroyOnLoad(this.gameObject); // Make it persistent across scenes
        }
    }

    public static APIHandler Instance {
        get {
            if (instance == null) {
                // Optionally log error or create a new GameObject with APIHandler
                Debug.LogError("APIHandler is not instantiated yet.");
            }
            return instance;
        }
    }


    public IEnumerator Authenticate(string apiKey) {
        string authUrl = $"{baseUrl}/api/login";
        AuthRequest requestBody = new AuthRequest { apiKey = apiKey };
        string json = JsonUtility.ToJson(requestBody);

/*        Debug.Log("API Key: " + apiKey);
        Debug.Log("AuthUrl: " + authUrl);
        Debug.Log("JSON: " + json);*/

        using (UnityWebRequest request = UnityWebRequest.PostWwwForm(authUrl, "POST")) {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError) {
                Debug.LogError("Error: " + request.error);
            }
            else if (request.responseCode == 500) {
                Debug.LogError("Server Error: " + request.downloadHandler.text);
            }
            else {
                ApiAuthResponse response = JsonUtility.FromJson<ApiAuthResponse>(request.downloadHandler.text);
                jwtToken = response.token;
                /*Debug.Log("Authenticated successfully, token: " + jwtToken);*/
                FetchPlayerProfile();
            }
        }
    }

    public void FetchPlayerProfile() {
        StartCoroutine(SendGetRequest("api/user/profile/view", HandleProfileSuccess, HandleProfileError));
    }

    private void HandleProfileSuccess(string response) {
        Debug.Log("Profile data successfully fetched: " + response);
        // You can further process the response here, e.g., parse it into an object
        // For example:
        UserProfile profile = JsonUtility.FromJson<UserProfile>(response);
        Debug.Log("Player Name: " + profile.user.firstname + " " + profile.user.lastname);
        // Assume UserProfile is a class you have defined that matches the JSON structure of the profile data
    }

    private void HandleProfileError(string error) {
        Debug.LogError("Error fetching profile: " + error);
        // Handle the error appropriately, e.g., show an error message to the user
    }



    [Serializable]
    private class ApiAuthResponse {
        public string token;
    }
}
