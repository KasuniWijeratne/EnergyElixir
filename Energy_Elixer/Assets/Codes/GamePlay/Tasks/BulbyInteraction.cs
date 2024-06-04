using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulbyInteraction : MonoBehaviour
{
    private bool playerInRange;

    private int previousScore = 0;
    [SerializeField] int startPoint = 0;
    [SerializeField] VisualNovelHandler visualNovelHandler;
    [SerializeField] SpriteRenderer dialogueBox;

    void Awake()
    {
        visualNovelHandler.SetReturnDataMethod(ReturnDataVN);
        visualNovelHandler.UpdateScript("Assets/Resources/VNScripts/House_Appliaces.csv");
        visualNovelHandler.SetCharacterLocationFilePath("Assets/Resources/VNScripts/1st_scene_characters.csv");
    }



    void Start()
    {
        dialogueBox.enabled = false;

    }

    void Update()
    {
        if (playerInRange && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)))
        {
            //Stop the player from moving
            visualNovelHandler.StartVisualNovel(startPoint);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //notification.getNotificationMessage("Bulby_Hi", True); // or a DialogueBox over bulby
            Debug.Log("Player in range");
            dialogueBox.enabled = true;
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //notification.getNotificationMessage("Bulby_Hi", False); // or a DialogueBox over bulby
            Debug.Log("Player out of range");
            dialogueBox.enabled = false;
            playerInRange = false;
        }
    }

    void OnBulbyInteraction()
    {

    }

        IEnumerator TestCoroutineFunction(string parameter)
    {
        int score = 0;
        int status = 0;        
        //put any code to run asynchronusly here
        // Debug.Log("Visual Novel returned:" + parameter);
        if (parameter == "~P" || parameter == "~F")
        {
            // GameManager.score -= previousScore;
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
                    // Debug.Log("right answer");
                    score = 5;
                    break;
                case 2:
                    // Debug.Log("neutral answer");
                    score = 2;
                    break;
                case 3:
                    // Debug.Log("Wrong answer");
                    score = -3;
                    break;

                default:
                    // Debug.Log("default case");
                    score = 0;
                    break;
            }

            score *= multiplier;

            GameManager.score -= previousScore;
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
