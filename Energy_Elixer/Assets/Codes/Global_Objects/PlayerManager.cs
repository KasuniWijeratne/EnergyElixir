using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager instance;
    public UserProfile userProfile;

    public DBPlayer playerInfo;

    [SerializeField] private IDynamicSystem dynamicSystemsManager;
    private string playerId;

    // public string PlayerId {get {return playerId;}}
    private bool isPlayerQuestionnaireCompleted;
    public event EventHandler<int> OnPlayerEnvironmentChanged;
    public event System.Action OnPlayerProfileLoaded;

    private bool isPlayerProfileCompleted;

    //variables for player score and environment status
    private int playerTotalCoins = 0; //initial score and the marks from the questionnaire
    // private int playerTotalCoins = 0; //total score of the player,     
    private int playerCurrentConsumptionScore = 0; // 0-100
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

    public void addScore(int deltaCoins)
    {
        playerTotalCoins += playerCurrentConsumptionScore * deltaCoins / 100;

        playerInfo.coins = playerTotalCoins;
        Debug.Log("Player total score: " + playerTotalCoins);

        DatabaseHandler.Instance.UpdatePlayerCoins(
            playerInfo, 
            (response) => Debug.Log("Score updated successfully:" + response),
            (response) => Debug.Log("Failed to update score: " + response)
        );
    }

    private void GetPlayerDatabaseInfo(string nic)
    {
        DatabaseHandler.Instance.GetPlayerByNic(nic, OnPlayerInfoRetrieved, OnPlayerInfoRetrievedFailed);
    }

    private void OnPlayerInfoRetrievedFailed(string response)
    {
        isPlayerQuestionnaireCompleted = true; // For testing purposes
        Debug.LogWarning("Failed to retrieve player info--------");
    }

    private void OnPlayerInfoRetrieved(string response)
    {
        playerInfo = JsonUtility.FromJson<DBPlayer>(response);

        playerTotalCoins = playerInfo.coins;

        if (playerInfo != null)
        {
            isPlayerQuestionnaireCompleted = true;
        }
        else
        {
            isPlayerQuestionnaireCompleted = false;
        }
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

        // Get the player info from the database
        GetPlayerDatabaseInfo(userProfile.user.nic);

        //start the dynamic systems manager after the player profile is fetched
        // dynamicSystemsManager = new DynamicSystemsManager();
        (dynamicSystemsManager as DynamicSystemsManager).ScoreChanged += OnScoreChanged;
        dynamicSystemsManager.InitializeScore();
        StartCoroutine(ChangeEnvironmentStatusAsync());



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
            environmentStatus = getCurrentEnvironmentStatus();
            if (environmentStatus != previousEnvStatus)
                TriggerPlayerEnvironmentChanged(environmentStatus);
            previousEnvStatus = environmentStatus;
            yield return new WaitForSeconds(UnityEngine.Random.Range(10f, 20f));
        }
    }

    private int getCurrentEnvironmentStatus()
    {
        if (playerCurrentConsumptionScore <= 60){
            return 3;
        }else if(playerCurrentConsumptionScore <= 40 ){
            return 2;
        }else if(playerCurrentConsumptionScore <= 20){
            return 1;
        }else{
            return 0;
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

    public void AddChallengePoints(float points)
    {
        dynamicSystemsManager.AddQuestionnairePoints(points);
    }
}