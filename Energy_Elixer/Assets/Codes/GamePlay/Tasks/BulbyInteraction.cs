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

    void updateStatus(bool status , int score = 0){
        if (score == 5 || score == 2){  //if the task is successful and done for the second time
                if (taskStatus[objectName]){
                    score = 0;
                }
        }

        GameManager.score += score;

        taskStatus[objectName] = status; //update the task status

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
                    updateStatus(true , score);
                    break;
                case 2:
                    score = 2;
                    updateStatus(true , score);
                    break;
                case 3:
                    score = -3;
                    updateStatus(false , score);
                    break;

                default:
                    score = 0;
                    updateStatus(false, score);
                    break;
            }

            score *= multiplier;

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
