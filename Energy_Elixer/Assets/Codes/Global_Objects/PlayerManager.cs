using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager instance; // Singleton instance
    public UserProfile userProfile;
    private string playerId;
    private bool isPlayerQuestionnaireCompleted;
    public event EventHandler<int> OnPlayerEnvironmentChanged;

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

    public bool IsPlayerQuestionnaireCompleted {
        get {
            return isPlayerQuestionnaireCompleted;
        }
        set {
            isPlayerQuestionnaireCompleted = value;
        }
    }


    void Awake() {
        if (instance == null) {
            instance = this;
            GameObject playerManagerObject = this.gameObject;

            // Ensure it's a root GameObject
            playerManagerObject.transform.parent = null;

            DontDestroyOnLoad(playerManagerObject); // Make it persistent across scenes
        }
        else if (instance != this) {
            Destroy(gameObject);
        }

        StartCoroutine(ChangeEnvironmentStatusAsync());
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


    public void TriggerPlayerEnvironmentChanged(int environmentStatus) {
        if(OnPlayerEnvironmentChanged != null)
            OnPlayerEnvironmentChanged?.Invoke(this, environmentStatus);
    }

    private IEnumerator ChangeEnvironmentStatusAsync()
    {
        int environmentStatus = 0;
        while (true)
        {
            // Generate a random environment status
            // int environmentStatus = UnityEngine.Random.Range(0, 3);
            environmentStatus = (environmentStatus + 1) % 4 +1;

            // Trigger the player environment changed event
            TriggerPlayerEnvironmentChanged(environmentStatus);

            // Wait for a random time interval between 1 and 5 seconds
            // float waitTime = UnityEngine.Random.Range(10f, 50f);
            // float waitTime = 5f;
            
            yield return new WaitForSeconds(10);
        }
    }


}
