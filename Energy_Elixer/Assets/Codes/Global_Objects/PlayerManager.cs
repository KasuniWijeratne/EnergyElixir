using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager instance;
    public UserProfile userProfile;
    private string playerId;
    private bool isPlayerQuestionnaireCompleted;
    public event EventHandler<int> OnPlayerEnvironmentChanged;
    public event System.Action OnPlayerProfileLoaded;

    private bool isPlayerProfileCompleted;
    public IDynamicSystem dynamicSystemsManager;

    private int playerCurrentConsumptionScore = 0;
    private int playerPreviousConsumptionScore = 0;
    public int PlayerCurrentConsumptionScore {
        get { return playerCurrentConsumptionScore; }
    }

    public bool IsPlayerProfileCompleted {
        get { return isPlayerProfileCompleted; }
        set { isPlayerProfileCompleted = value; }
    }

    public bool IsPlayerQuestionnaireCompleted {
        get { return isPlayerQuestionnaireCompleted; }
        set { isPlayerQuestionnaireCompleted = value; }
    }

    void Awake() {
        if (instance == null) {
            instance = this;
            GameObject playerManagerObject = this.gameObject;
            playerManagerObject.transform.parent = null;
            DontDestroyOnLoad(playerManagerObject);
        }
        else if (instance != this) {
            Destroy(gameObject);
        }

        dynamicSystemsManager = new DynamicSystemsManager();
        (dynamicSystemsManager as DynamicSystemsManager).ScoreChanged += OnScoreChanged;
        dynamicSystemsManager.InitializeScore();

        StartCoroutine(ChangeEnvironmentStatusAsync());
    }

    public static PlayerManager Instance {
        get {
            if (instance == null) {
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

    private void GetPlayerProfile(System.Action<string> onSuccess, System.Action<string> onError) {
        APIHandler.Instance.FetchPlayerProfile(onSuccess, onError);
    }

    private void OnSuccessfulProfileFetched(string result) {
        userProfile = JsonUtility.FromJson<UserProfile>(result);

        if (userProfile.IsProfileCompleted()) {
            isPlayerProfileCompleted = true;
        } else {
            isPlayerProfileCompleted = false;
        }

        Debug.Log(result);
        OnPlayerProfileLoaded?.Invoke();
    }

    private void OnFailedProfileFetched(string errorMsg) {
        Debug.Log(errorMsg);
    }

    public void TriggerPlayerEnvironmentChanged(int environmentStatus) {
        OnPlayerEnvironmentChanged?.Invoke(this, environmentStatus);
    }

    private IEnumerator ChangeEnvironmentStatusAsync()
    {
        int environmentStatus = 0, previousEnvStatus = 0;
        while (true)
        {
            environmentStatus = dynamicSystemsManager.getCurrentEnvironmentStatus();
            if (environmentStatus != previousEnvStatus)
                TriggerPlayerEnvironmentChanged(environmentStatus);
            previousEnvStatus = environmentStatus;
            yield return new WaitForSeconds(UnityEngine.Random.Range(10f, 20f));
        }
    }

    private void OnScoreChanged(object sender, float newScore)
    {
        playerPreviousConsumptionScore = playerCurrentConsumptionScore;
        playerCurrentConsumptionScore = (int)newScore;
        // if (playerCurrentConsumptionScore != playerPreviousConsumptionScore)
        // {
        //     TriggerPlayerEnvironmentChanged(dynamicSystemsManager.getCurrentEnvironmentStatus());
        // }
    }

    public void AddDailyChallengePoints(float points)
    {
        dynamicSystemsManager.AddDailyChallengePoints(points);
    }
}