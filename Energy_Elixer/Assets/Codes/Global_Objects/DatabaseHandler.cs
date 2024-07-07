using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.Networking;
//using Newtonsoft.Json.Linq;
using UnityEngine;
using Newtonsoft.Json;
using System.Text;

#region JSON Classes
[System.Serializable]
public class DBPlayer
{
    public int id;
    public string nic;
    public int marks;
    public int questionNumber;
    public int level;
    public int coins;

    public DBPlayer()
    {
        id = 0;
        nic = "";
        marks = 0;
        questionNumber = 0;
        level = 0;
        coins = 0;
    }
}

[System.Serializable]
public class AddDBPlayer
{
    public string nic;
    // public int marks;
    public int questionNumber;
    // public int level;
    // public int coins;

    public AddDBPlayer()
    {
        nic = "";
        questionNumber = 0;
    }
}

[System.Serializable]
public class DBPlayerList
{
    public DBPlayer[] players;
}

#endregion


public class DatabaseHandler : MonoBehaviour
{
    private static DatabaseHandler instance; // Singleton instance
    private const string baseUrl = "http://localhost:8080";

    public event System.Action OnPlayerInfoRetrived;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            GameObject apiHandlerObj = this.gameObject;

            // Ensure it's a root GameObject
            apiHandlerObj.transform.parent = null;

            DontDestroyOnLoad(apiHandlerObj); // Make it persistent across scenes
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static DatabaseHandler Instance
    {
        get
        {
            if (instance == null)
            {
                // create a new GameObject with DatabaseHandler
                instance = new GameObject("DatabaseHandler").AddComponent<DatabaseHandler>();
            }
            return instance;
        }
    }

    private IEnumerator SendGetRequest(string path, System.Action<string> onSuccess, System.Action<string> onError)
    {
        string url = $"{baseUrl}/{path}";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();  // Wait for the request to complete

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("Error in GET request: " + request.error);
                onError?.Invoke(request.error);
            }
            else
            {
                onSuccess?.Invoke(request.downloadHandler.text);  // Handle the successful response
            }
        }
    }

    private IEnumerator SendPostRequest(string path, string json, System.Action<string> onSuccess, System.Action<string> onError)
    {
        string url = $"{baseUrl}/{path}";

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogWarning($"Error in POST request: {request.error}\nResponse Code: {request.responseCode}\nResponse: {request.downloadHandler.text}");
                onError?.Invoke($"{request.error} - {request.downloadHandler.text}");
            }
            else if (request.responseCode == 400)
            {
                Debug.LogWarning($"Bad Request: {request.downloadHandler.text}");
                onError?.Invoke($"Bad Request: {request.downloadHandler.text}");
            }
            else
            {
                onSuccess?.Invoke(request.downloadHandler.text);
            }
        }
    }

    public void GetPlayerInformation()
    {
        StartCoroutine(SendGetRequest("player/getPlayer", HandlePlayerInfoSuccess, HandlePlayerInfoError));
    }

    private void HandlePlayerInfoSuccess(string response)
    {
        Debug.Log("Player data successfully fetched: " + response);
        //check whether the json response has a body
        if (IsJsonObjectEmpty(response))
        {
            PlayerManager.Instance.IsPlayerQuestionnaireCompleted = false;
        }
        else
        {
            PlayerManager.Instance.IsPlayerQuestionnaireCompleted = true;
        }
        OnPlayerInfoRetrived?.Invoke();
    }

    private static bool IsJsonObjectEmpty(string json)
    {
        json = json.Trim(); // Make sure to trim whitespace which might affect the check
        if (json.Equals("{}") || string.IsNullOrEmpty(json))
        {
            return true;
        }
        // Optionally add more complex checks here
        return false;
    }

    private void HandlePlayerInfoError(string error)
    {
        Debug.Log("Error fetching profile: " + error);
        PlayerManager.Instance.IsPlayerQuestionnaireCompleted = true; // For testing purposes
        OnPlayerInfoRetrived?.Invoke();
    }

    #region Database API Methods

    public void UpdatePlayerQuestionNumber(DBPlayer player, System.Action<string> onSuccess, System.Action<string> onError)
    {
        string json = JsonUtility.ToJson(player);
        StartCoroutine(SendPostRequest("player/updateQuestionNumber", json, onSuccess, onError));
    }

    public void UpdatePlayerMarks(DBPlayer player, System.Action<string> onSuccess, System.Action<string> onError)
    {
        string json = JsonUtility.ToJson(player);
        StartCoroutine(SendPostRequest("player/updateMarks", json, onSuccess, onError));
    }

    public void UpdatePlayerLevel(DBPlayer player, System.Action<string> onSuccess, System.Action<string> onError)
    {
        string json = JsonUtility.ToJson(player);
        StartCoroutine(SendPostRequest("player/updateLevel", json, onSuccess, onError));
    }

    public void UpdatePlayerCoins(DBPlayer player, System.Action<string> onSuccess, System.Action<string> onError)
    {
        string json = JsonUtility.ToJson(player);
        StartCoroutine(SendPostRequest("player/updateCoins", json, onSuccess, onError));
    }

    public void DeletePlayer(DBPlayer player, System.Action<string> onSuccess, System.Action<string> onError)
    {
        string json = JsonUtility.ToJson(player);
        StartCoroutine(SendPostRequest("player/delete", json, onSuccess, onError));
    }

    public void AddPlayer(AddDBPlayer player, System.Action<string> onSuccess, System.Action<string> onError)
    {
        var wrapper = new { player = player };
        string json = JsonConvert.SerializeObject(player);
        Debug.Log("Player JSON: " + json); // For debugging purposes
        StartCoroutine(SendPostRequest("player/add", json, onSuccess, onError));
    }

    public void GetPlayerByNic(string nic, System.Action<string> onSuccess, System.Action<string> onError)
    {
        StartCoroutine(SendGetRequest($"player/{nic}", onSuccess, onError));
    }

    public void GetAllPlayers(System.Action<string> onSuccess, System.Action<string> onError)
    {
        StartCoroutine(SendGetRequest("player/getAll", onSuccess, onError));
    }

    #endregion

    #region Example Usage

    public void DisplayPlayerByNic(string nic)
    {
        GetPlayerByNic(nic,
            (successMsg) =>
            {
                Debug.Log("Player data successfully fetched: " + successMsg);
                DBPlayer player = JsonConvert.DeserializeObject<DBPlayer>(successMsg);
                Debug.Log($"Player NIC: {player.nic}, Marks: {player.marks}, Question Number: {player.questionNumber}, Level: {player.level}, Coins: {player.coins}");
            },
            (errorMsg) =>
            {
                Debug.LogWarning("Error fetching player data: " + errorMsg);
            }
        );
    }

    public void DisplayAllPlayers()
    {
        GetAllPlayers(
            (successMsg) =>
            {
                Debug.Log("All players data successfully fetched: " + successMsg);
                // DBPlayerList playerList = JsonConvert.DeserializeObject<DBPlayerList>(successMsg);
                List<DBPlayer> players = JsonConvert.DeserializeObject<List<DBPlayer>>(successMsg);
                foreach (var player in players)
                {
                    Debug.Log($"Player NIC: {player.nic}, Marks: {player.marks}, Question Number: {player.questionNumber}, Level: {player.level}, Coins: {player.coins}");
                }
            },
            (errorMsg) =>
            {
                Debug.LogWarning("Error fetching all players data: " + errorMsg);
            }
        );
    }

    public void UpdateAndDisplayPlayerQuestionNumber(DBPlayer player)
    {
        UpdatePlayerQuestionNumber(player,
            (successMsg) =>
            {
                Debug.Log("Player question number updated successfully: " + successMsg);
                DisplayPlayerByNic(player.nic);
            },
            (errorMsg) =>
            {
                Debug.LogWarning("Error updating player question number: " + errorMsg);
            }
        );
    }

    public void AddAndDisplayPlayer(AddDBPlayer player)
    {
        AddPlayer(player,
            (successMsg) =>
            {
                Debug.Log("Player added successfully: " + successMsg);
                DisplayPlayerByNic(player.nic);
            },
            (errorMsg) =>
            {
                Debug.LogWarning("Error adding player: " + errorMsg);
            }
        );
    }

    #endregion

    public void FetchPlayerScores(System.Action<string> onSuccess, System.Action<string> onError)
    {
        StartCoroutine(DummyFunction(onSuccess, onError));
    }

    private IEnumerator DummyFunction(System.Action<string> onSuccess, System.Action<string> onError)
    {
        yield return new WaitForSeconds(1);
        onSuccess?.Invoke(
            @"[
                {
                    ""id"": 4,
                    ""nic"": ""string"",
                    ""marks"": 0,
                    ""questionNumber"": 0,
                    ""level"": 0,
                    ""coins"": 0
                },
                {
                    ""id"": 5,
                    ""nic"": ""200889654"",
                    ""marks"": 0,
                    ""questionNumber"": 0,
                    ""level"": 0,
                    ""coins"": 66
                }
            ]");
    }
}