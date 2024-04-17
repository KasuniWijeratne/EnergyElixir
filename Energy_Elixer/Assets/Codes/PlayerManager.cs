using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager instance; // Singleton instance
    private const string baseUrl = "http://";
    public UserProfile userProfile;
    private string playerId;


    public event System.Action OnPlayerProfileLoaded;

    private bool isPlayerProfileCompleted;
    public bool IsPlayerProfileCompleted {
        get {
            return isPlayerProfileCompleted;
        }
        set {
            isPlayerProfileCompleted = value;
        }
    }


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

    public static PlayerManager Instance {
        get {
            if (instance == null) {
                // Create a new GameObject 
                instance = new GameObject("PlayerManager").AddComponent<PlayerManager>();
                
            }
            return instance;
        }
    }

    public void SetPlayerId(string id) {
        playerId = id;
    }

    public string GetPlayerId() {
        return playerId;
    }

    public UserProfile GetUserProfile() {
        return userProfile;
    }
    public void SetUserProfile(UserProfile profile) {
        userProfile = profile;
    }


    public void GetPlayerProfile() {
        GetPlayerProfile(OnSuccessfulProfileFetched, OnFailedProfileFetched);
    }

    private void GetPlayerProfile( System.Action<string> onSuccess, System.Action<string> onError) {
        APIHandler.Instance.FetchPlayerProfile(onSuccess, onError);
    }

    private void OnSuccessfulProfileFetched(string result) {
        //parse the result and set the player profile
        userProfile = JsonUtility.FromJson<UserProfile>(result);

        if (userProfile.IsProfileCompleted()) {
            isPlayerProfileCompleted = true;
        } else {
            isPlayerProfileCompleted = false;
        }

        Debug.Log(result);

        //invoke event to go to the next scene
        OnPlayerProfileLoaded?.Invoke();
    }

    private void OnFailedProfileFetched(string errorMsg) {
        Debug.Log(errorMsg);
    }

}
