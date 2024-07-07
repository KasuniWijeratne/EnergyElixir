using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using System.Linq;
using Newtonsoft.Json;

#region JSON Classes
[System.Serializable]
public class User
{

    public User()
    {
        firstname = "";
        lastname = "";
        username = "";
        nic = "";
        phoneNumber = "";
        email = "";
    }

    public string firstname;
    public string lastname;
    public string username;
    public string nic;
    public string phoneNumber;
    public string email;
}
[System.Serializable]
public class UserProfile
{
    public User user;

    public UserProfile()
    {
        user = new User();
    }
    public bool IsProfileCompleted()
    {
        return user != null && !string.IsNullOrEmpty(user.firstname) && !string.IsNullOrEmpty(user.lastname) &&
               !string.IsNullOrEmpty(user.username) && !string.IsNullOrEmpty(user.nic) &&
               !string.IsNullOrEmpty(user.phoneNumber) && !string.IsNullOrEmpty(user.email);
    }
}
[System.Serializable]
public class UserProfileList
{
    public List<User> users;
}
[System.Serializable]
public class MonthlyUnits
{
    public float units;
}
[System.Serializable]
public class PowerConsumptionYearlyResponse
{
    public Dictionary<string, MonthlyUnits> units;
}
[System.Serializable]
public class YearlyPowerConsumptionView
{
    public int year;
    public Dictionary<string, MonthlyUnits> units;
}
[System.Serializable]
public class PowerConsumptionAllResponse
{
    public List<YearlyPowerConsumptionView> yearlyPowerConsumptionViews;
}
[System.Serializable]
public class PowerConsumptionMonthlyView
{
    public int year;
    public int month;
    public float units;
}
[System.Serializable]
public class PowerConsumptionMonthlyViewWrapper
{
    public PowerConsumptionMonthlyView monthlyPowerConsumptionView;
}
[System.Serializable]
public class PowerConsumptionDailyViewWrapper
{
    [JsonProperty("dailyPowerConsumptionView")]
    public PowerConsumptionDailyView DailyPowerConsumptionView { get; set; }
}
[System.Serializable]
public class PowerConsumptionDailyView
{
    [JsonProperty("year")]
    public int year { get; set; }

    [JsonProperty("month")]
    public int month { get; set; }

    [JsonProperty("dailyUnits")]
    public Dictionary<int, float> dailyUnits { get; set; }
}


// [System.Serializable]
// public class PowerConsumptionDailyView
// {
//     public int year;
//     public int month;
//     public Dictionary<int, float> dailyUnits;
// }
[System.Serializable]
public class CurrentPowerConsumption
{
    public float currentConsumption;
}
#endregion

public class APIHandler : MonoBehaviour
{
    private static APIHandler instance; // Singleton instance
    private const string baseUrl = "http://20.15.114.131:8080";
    private string jwtToken;

    [System.Serializable]
    private class ApiAuthResponse
    {
        public string token;
    }
    [System.Serializable]
    private class AuthRequest
    {
        public string apiKey;
    }

    private IEnumerator SendGetRequest(string path, System.Action<string> onSuccess, System.Action<string> onError)
    {
        // if (string.IsNullOrEmpty(jwtToken))
        // {
        //     Debug.LogError("JWT token is empty.");
        //     onError?.Invoke("JWT token is empty.");
        //     yield break;
        // }

        // string url = $"{baseUrl}/{path}";
        // using (UnityWebRequest request = UnityWebRequest.Get(url))
        // {
        //     request.SetRequestHeader("Authorization", $"Bearer {jwtToken}");
        //     yield return request.SendWebRequest();  // Wait for the request to complete

        //     if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        //     {
        //         Debug.LogError("Error in GET request: " + request.error);
        //         onError?.Invoke(request.error);
        //     }
        //     else
        //     {
        //         onSuccess?.Invoke(request.downloadHandler.text);  // Handle the successful response
        //     }
        // }
        return SendGetRequest(path, null, onSuccess, onError);
    }

    private IEnumerator SendGetRequest(string path, Dictionary<string, string> keyValuePairs, System.Action<string> onSuccess, System.Action<string> onError)
    {
        if (string.IsNullOrEmpty(jwtToken))
        {
            Debug.LogError("JWT token is empty.");
            onError?.Invoke("JWT token is empty.");
            yield break;
        }

        string url = $"{baseUrl}/{path}";
        if (keyValuePairs != null && keyValuePairs.Count > 0)
        {
            url += "?" + string.Join("&", keyValuePairs.Select(x => x.Key + "=" + x.Value).ToArray());
        }

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("Authorization", $"Bearer {jwtToken}");
            yield return request.SendWebRequest();  // Wait for the request to complete

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error in GET request: " + request.error);
                onError?.Invoke(request.error);
            }
            else
            {
                onSuccess?.Invoke(request.downloadHandler.text);  // Handle the successful response
            }
        }
    }

    private IEnumerator SendPutRequests(string path, string json, System.Action<string> onSuccess, System.Action<string> onError)
    {
        if (string.IsNullOrEmpty(jwtToken))
        {
            Debug.LogError("JWT token is empty.");
            onError?.Invoke("JWT token is empty.");
            yield break;
        }

        string url = $"{baseUrl}/{path}";
        using (UnityWebRequest request = UnityWebRequest.Put(url, json))
        {
            request.SetRequestHeader("Authorization", $"Bearer {jwtToken}");
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();  // Wait for the request to complete

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error in PUT request: " + request.error);
                Debug.LogError("Error in PUT request: " + request.downloadHandler.text);
                onError?.Invoke(request.error);
            }
            else
            {
                onSuccess?.Invoke(request.downloadHandler.text);  // Handle the successful response
            }
        }
    }
    private IEnumerator SendPostRequest(bool needAuth, string path, string json, System.Action<string> onSuccess, System.Action<string> onError)
    {
        string url = $"{baseUrl}/{path}";

        using (UnityWebRequest request = UnityWebRequest.PostWwwForm(url, "POST"))
        {
            if (needAuth)
            {
                if (string.IsNullOrEmpty(jwtToken))
                {
                    Debug.LogError("JWT token is empty.");
                    onError?.Invoke("JWT token is empty.");
                    yield break;
                }
                request.SetRequestHeader("Authorization", $"Bearer {jwtToken}");
            }

            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");


            yield return request.SendWebRequest();  // Wait for the request to complete

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error in POST request: " + request.error);
                onError?.Invoke(request.error);
            }
            else if (request.responseCode == 500)
            {
                onError?.Invoke("Server Error: " + request.downloadHandler.text);
            }
            else
            {
                onSuccess?.Invoke(request.downloadHandler.text);  // Handle the successful response
            }
        }
    }
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

    public static APIHandler Instance
    {
        get
        {
            if (instance == null)
            {
                // Optionally log error or create a new GameObject with APIHandler
                // Debug.LogError("APIHandler is not instantiated yet.");
                instance = new GameObject("APIHandler").AddComponent<APIHandler>();
            }
            return instance;
        }
    }

    public void Authenticate(string apiKey)
    {
        string path = "api/login";
        AuthRequest requestBody = new AuthRequest { apiKey = apiKey };
        string json = JsonUtility.ToJson(requestBody);
        StartCoroutine(SendPostRequest(false, path, json,
                    (successMsg) =>
                    {
                        // Debug.Log("Authentication Successful | " + successMsg);
                        jwtToken = JsonUtility.FromJson<ApiAuthResponse>(successMsg).token;
                        PlayerManager.Instance.GetPlayerProfile();
                    },
                    (errorMsg) =>
                    {
                        Debug.LogError("Error at Authentication | " + errorMsg);
                    }
                    ));
    }

    public void FetchPlayerProfile(System.Action<string> onSuccess, System.Action<string> onError)
    {
        StartCoroutine(SendGetRequest("api/user/profile/view", onSuccess, onError));
    }

    public void UpdatePlayerProfile(UserProfile user)
    {
        string json = JsonUtility.ToJson(user.user);

        Debug.Log("JSON: " + json);

        StartCoroutine(SendPutRequests(
            "api/user/profile/update",
            json,
            (successMsg) =>
            {
                UserProfile profile = JsonUtility.FromJson<UserProfile>(successMsg);
                PlayerManager.Instance.SetUserProfile(profile);
                PlayerManager.Instance.IsPlayerProfileCompleted = true;
            },
            (errorMsg) =>
            {
                Debug.LogError("Error updating profile: " + errorMsg);
            }
            ));
    }

    public void FetchPlayerList(System.Action<string> onSuccess, System.Action<string> onError)
    {
        StartCoroutine(SendGetRequest("api/user/profile/list", onSuccess, onError));
    }

    public void FetchYearlyPowerConsumption(int year, System.Action<string> onSuccess, System.Action<string> onError)
    {
        var parameters = new Dictionary<string, string> { { "year", year.ToString() } };
        StartCoroutine(SendGetRequest("api/power-consumption/yearly/view", parameters, onSuccess, onError));
    }

    public void FetchAllPowerConsumption(System.Action<string> onSuccess, System.Action<string> onError)
    {
        StartCoroutine(SendGetRequest("api/power-consumption/all/view", onSuccess, onError));
    }

    public void FetchPowerConsumptionBySpecificMonth(int year, string month, System.Action<string> onSuccess, System.Action<string> onError)
    {
        var parameters = new Dictionary<string, string>
        {
            { "year", year.ToString() },
            { "month", month }
        };
        StartCoroutine(SendGetRequest("api/power-consumption/month/view", parameters, onSuccess, onError));
    }

    public void FetchPowerConsumptionByCurrentMonth(System.Action<string> onSuccess, System.Action<string> onError)
    {
        StartCoroutine(SendGetRequest("api/power-consumption/current-month/view", onSuccess, onError));
    }

    public void FetchDailyPowerConsumptionBySpecificMonth(int year, string month, System.Action<string> onSuccess, System.Action<string> onError)
    {
        var parameters = new Dictionary<string, string>
        {
            { "year", year.ToString() },
            { "month", month }
        };
        StartCoroutine(SendGetRequest("api/power-consumption/month/daily/view", parameters, onSuccess, onError));
    }

    public void FetchDailyPowerConsumptionByCurrentMonth(System.Action<string> onSuccess, System.Action<string> onError)
    {
        StartCoroutine(SendGetRequest("api/power-consumption/current-month/daily/view", onSuccess, onError));
    }

    public void FetchCurrentPowerConsumption(System.Action<string> onSuccess, System.Action<string> onError)
    {
        StartCoroutine(SendGetRequest("api/power-consumption/current/view", onSuccess, onError));
    }

#region  Example Usage
    public void DisplayPlayerProfile()
    {
        FetchPlayerProfile(
            (successMsg) =>
            {
                Debug.Log("Profile data successfully fetched: " + successMsg);
                UserProfile profile = JsonConvert.DeserializeObject<UserProfile>(successMsg);
                Debug.Log("Player Name: " + profile.user.firstname + " " + profile.user.lastname);
            },
            (errorMsg) =>
            {
                Debug.LogError("Error fetching profile: " + errorMsg);
            }
            );
    }

    public void DisplayPlayerList()
    {
        FetchPlayerList(
            (successMsg) =>
            {
                Debug.Log("Player list successfully fetched: " + successMsg);
                UserProfileList profileList = JsonConvert.DeserializeObject<UserProfileList>(successMsg);
                if(profileList.users != null){
                foreach (var user in profileList.users)
                    {
                        Debug.Log("Player: " + user.firstname + " " + user.lastname);
                    }
                } else{
                    Debug.Log("No players found.");
                }
            },
            (errorMsg) =>
            {
                Debug.LogError("Error fetching player list: " + errorMsg);
            }
        );
    }

    public void DisplayYearlyPowerConsumption(int year)
    {

        FetchYearlyPowerConsumption(
            year,
            (successMsg) =>
            {
                Debug.Log("Yearly power consumption data successfully fetched: " + successMsg);
                PowerConsumptionYearlyResponse response = JsonConvert.DeserializeObject<PowerConsumptionYearlyResponse>(successMsg);
                

                if(response.units != null){    
                    foreach (var kvp in response.units)
                    {
                        Debug.Log($"{kvp.Key}: {kvp.Value.units} units");
                    }
                }else{
                    Debug.Log("No data found. Check the year.");
                }
            },
            (errorMsg) =>
            {
                Debug.LogError("Error fetching yearly power consumption: " + errorMsg);
            }
        );
    }

    public void DisplayAllPowerConsumption()
    {
        FetchAllPowerConsumption(
            (successMsg) =>
            {
                Debug.Log("All power consumption data successfully fetched: " + successMsg);
                PowerConsumptionAllResponse response = JsonConvert.DeserializeObject<PowerConsumptionAllResponse>(successMsg);
                foreach (var yearlyView in response.yearlyPowerConsumptionViews)
                {
                    Debug.Log($"Year: {yearlyView.year}");
                    foreach (var kvp in yearlyView.units)
                    {
                        Debug.Log($"{kvp.Key}: {kvp.Value.units} units");
                    }
                }
            },
            (errorMsg) =>
            {
                Debug.LogError("Error fetching all power consumption: " + errorMsg);
            }
        );
    }

    public void DisplayPowerConsumptionBySpecificMonth(int year, string month)
    {
        FetchPowerConsumptionBySpecificMonth(
            year, month,
            (successMsg) =>
            {
                Debug.Log("Power consumption data for specific month successfully fetched: " + successMsg);
                PowerConsumptionMonthlyViewWrapper responseWrapper = JsonConvert.DeserializeObject<PowerConsumptionMonthlyViewWrapper>(successMsg);
                PowerConsumptionMonthlyView response = responseWrapper.monthlyPowerConsumptionView;
                if(response != null){
                Debug.Log($"Year: {response.year}, Month: {response.month}, Units: {response.units}");
                }else{
                    Debug.Log("No data found. Check the year and month.");
                }
            },
            (errorMsg) =>
            {
                Debug.LogError("Error fetching power consumption for specific month: " + errorMsg);
            }
        );
    }

    public void DisplayPowerConsumptionByCurrentMonth()
    {
        FetchPowerConsumptionByCurrentMonth(
            (successMsg) =>
            {
                Debug.Log("Power consumption data for current month successfully fetched: " + successMsg);
                PowerConsumptionMonthlyViewWrapper responseWrapper = JsonConvert.DeserializeObject<PowerConsumptionMonthlyViewWrapper>(successMsg);
                PowerConsumptionMonthlyView response = responseWrapper.monthlyPowerConsumptionView;
                 if(response != null){
                Debug.Log($"Year: {response.year}, Month: {response.month}, Units: {response.units}");
                }else{
                    Debug.Log("No data found. Check the year and month.");
                }
            },
            (errorMsg) =>
            {
                Debug.LogError("Error fetching power consumption for current month: " + errorMsg);
            }
        );
    }

    public void DisplayDailyPowerConsumptionBySpecificMonth(int year, string month)
    {
        FetchDailyPowerConsumptionBySpecificMonth(
            year, month,
            (successMsg) =>
            {
                Debug.Log("Daily power consumption data for specific month successfully fetched: " + successMsg);
                PowerConsumptionDailyViewWrapper wrapper = JsonConvert.DeserializeObject<PowerConsumptionDailyViewWrapper>(successMsg);
                PowerConsumptionDailyView response = wrapper.DailyPowerConsumptionView;
                if(response.dailyUnits != null){
                    Debug.Log($"Year: {response.year}, Month: {response.month}");
                    foreach (var kvp in response.dailyUnits)
                    {
                        Debug.Log($"Day {kvp.Key}: {kvp.Value} units");
                    }
                }else{
                    Debug.Log("No data found. Check the year and month.");
                }
            },
            (errorMsg) =>
            {
                Debug.LogError("Error fetching daily power consumption for specific month: " + errorMsg);
            }
        );
    }

    public void DisplayDailyPowerConsumptionByCurrentMonth()
    {
        FetchDailyPowerConsumptionByCurrentMonth(
            (successMsg) =>
            {
                Debug.Log("Daily power consumption data for current month successfully fetched: " + successMsg);
                PowerConsumptionDailyViewWrapper wrapper = JsonConvert.DeserializeObject<PowerConsumptionDailyViewWrapper>(successMsg);
                PowerConsumptionDailyView response = wrapper.DailyPowerConsumptionView;
                if(response.dailyUnits != null){
                    Debug.Log($"Year: {response.year}, Month: {response.month}");
                    foreach (var kvp in response.dailyUnits)
                    {
                        Debug.Log($"Day {kvp.Key}: {kvp.Value} units");
                    }
                }else{
                    Debug.Log("No data found. Check the year and month.");
                }
            },
            (errorMsg) =>
            {
                Debug.LogError("Error fetching daily power consumption for current month: " + errorMsg);
            }
        );
    }

    public void DisplayCurrentPowerConsumption()
    {
        FetchCurrentPowerConsumption(
            (successMsg) =>
            {
                Debug.Log("Current power consumption successfully fetched: " + successMsg);
                CurrentPowerConsumption response = JsonConvert.DeserializeObject<CurrentPowerConsumption>(successMsg);
                if(response != null){
                    Debug.Log($"Current Consumption: {response.currentConsumption} Wh");
                }else{
                    Debug.Log("No data found.");
                }
            },
            (errorMsg) =>
            {
                Debug.LogError("Error fetching current power consumption: " + errorMsg);
            }
        );
    }
#endregion
}
