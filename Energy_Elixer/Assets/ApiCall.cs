using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ApiCall : MonoBehaviour
{
    // API URL
    private string apiUrl = "http://your_api_endpoint_here";

    // Function to start the API call
    public void CallApi()
    {
        StartCoroutine(GetRequest(apiUrl));
    }

    // Coroutine to handle the web request
    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Send the request and wait for the response
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                // Handle the result
                Debug.Log(webRequest.downloadHandler.text);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
