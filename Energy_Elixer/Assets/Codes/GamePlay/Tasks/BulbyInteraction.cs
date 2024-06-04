using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulbyInteraction : MonoBehaviour
{
    private bool playerInRange;
    [SerializeField] int startPoint = 0;
    [SerializeField] VisualNovelHandler visualNovelHandler;

    void Awake()
    {
        visualNovelHandler.SetReturnDataMethod(ReturnDataVN);
        visualNovelHandler.UpdateScript("Assets/Resources/VNScripts/House_Appliaces.csv");
        visualNovelHandler.SetCharacterLocationFilePath("Assets/Resources/VNScripts/1st_scene_characters.csv");
    }



    void Start()
    {

    }

    void Update()
    {
        if (playerInRange && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)))
        {
            visualNovelHandler.StartVisualNovel(startPoint);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player in range");
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player out of range");
            playerInRange = false;
        }
    }

    void OnBulbyInteraction()
    {

    }

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
