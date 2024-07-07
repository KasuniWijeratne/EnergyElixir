using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulbyInteraction : MonoBehaviour
{
    private bool playerInRange;

    private static Dictionary<string, bool> taskStatus = new Dictionary<string, bool>();

    private int previousScore = 0;
    private string objectName;
    [SerializeField] int startPoint = 0;
    [SerializeField] VisualNovelHandler visualNovelHandler;
    [SerializeField] SpriteRenderer dialogueBox;
    [SerializeField] Notifications notification;

    void Awake()
    {
        visualNovelHandler.SetReturnDataMethod(ReturnDataVN);
        visualNovelHandler.UpdateScript("Assets/Resources/VNScripts/House_Appliaces.csv");
        visualNovelHandler.SetCharacterLocationFilePath("Assets/Resources/VNScripts/1st_scene_characters.csv");
    }

    void Start()
    {
        dialogueBox.enabled = false;
        SoundManager.Instance.PlayMusic("platformer");
    }

    void Update()
    {
        if (playerInRange && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)))
        {
            //Stop the player from moving
            visualNovelHandler.StartVisualNovel(startPoint);
        }
    }

    public bool getTaskSuccess(string objectName)
    {
        if (taskStatus.ContainsKey(objectName))
        {
            return taskStatus[objectName];
            
        }
        else
        {
            return false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueBox.enabled = true;
            playerInRange = true;

            objectName = this.gameObject.name;
            Debug.Log("Object Name: " + objectName);
            if (!taskStatus.ContainsKey(objectName))
            {
                taskStatus.Add(objectName, false);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueBox.enabled = false;
            playerInRange = false;
        }
    }

    void updateStatus(bool status){
        taskStatus[objectName] = status;
        Debug.Log("Task Status: " + objectName);

        if (notification != null)
            {
                notification.getNotificationMessage(objectName, true);
            }
            else
            {
                Debug.LogError("Notification object is null. Cannot send notification.");
            }
    }

    IEnumerator TestCoroutineFunction(string parameter)
    {
        int score = 0;
        int status = 0;    
        //put any code to run asynchronusly here
        if (parameter == "~P" || parameter == "~F")
        {
            visualNovelHandler.StopVisualNovel();
        }
        
        if(int.TryParse(parameter, out status)){
            int multiplier = 1;
            if (parameter.Contains("_back")){
                multiplier = -1;
                parameter = parameter.Replace("_back", "");
            }else {
                multiplier = 1;
            }
                switch (status)
            {
                case 1:
                    score = 5;
                    updateStatus(true);
                    break;
                case 2:
                    score = 2;
                    updateStatus(true);
                    break;
                case 3:
                    score = -3;
                    // updateStatus(false);
                    break;

                default:
                    score = 0;
                    // updateStatus(false);
                    break;
            }

            score *= multiplier;

            GameManager.score += score;
            previousScore = score;
        }
        // Debug.Log("\ntestInt: " + testInt);
        yield return null;
    }    

    // Method to start the coroutine with the parameter
    //this function is used to receive data from the visual novel handler
    void ReturnDataVN(string parameter)
    {
        StartCoroutine(TestCoroutineFunction(parameter));
    }


}
