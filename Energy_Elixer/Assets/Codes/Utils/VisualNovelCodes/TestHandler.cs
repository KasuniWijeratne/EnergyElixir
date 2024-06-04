using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestHandler : MonoBehaviour
{    
    [SerializeField] VisualNovelHandler visualNovelHandler;

    // private int testInt = 0;

    void Awake()
    {
        visualNovelHandler.SetReturnDataMethod(ReturnDataVN);
        // visualNovelHandler.UpdateScript("Assets/Resources/VNScripts/1st_scene_test.csv");
        visualNovelHandler.UpdateScript("Assets/Resources/VNScripts/House_Appliaces.csv");
        visualNovelHandler.SetCharacterLocationFilePath("Assets/Resources/VNScripts/1st_scene_characters.csv");
    }

    void Start()
    {
    }

    bool isRunning = false;
    // Update is called once per frame
    void Update()
    {
        if(!isRunning){
            visualNovelHandler.StartVisualNovel();
            isRunning = true;
        }
    }

    // Define a coroutine method that takes a string parameter
    //this function is used to process the data came from the visual novel handler asynchronously
    IEnumerator TestCoroutineFunction(string parameter)
    {
        //put any code to run asynchronusly here
        Debug.Log("Visual Novel returned:" + parameter);
        // testInt += int.Parse(parameter);
        // Debug.Log("\ntestInt: " + testInt);
        yield return new WaitForSeconds(1f);
    }    

    // Method to start the coroutine with the parameter
    //this function is used to receive data from the visual novel handler
    void ReturnDataVN(string parameter)
    {
        StartCoroutine(TestCoroutineFunction(parameter));
    }

}
