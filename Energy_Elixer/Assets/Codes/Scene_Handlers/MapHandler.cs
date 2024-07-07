using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI placeName;
    [SerializeField] VisualNovelHandler visualNovelHandler;

    void Awake()
    {
        visualNovelHandler.SetReturnDataMethod(ReturnDataVN);
        // visualNovelHandler.UpdateScript("Assets/Resources/VNScripts/1st_scene_test.csv");
        visualNovelHandler.UpdateScript("Assets/Resources/VNScripts/1st_scene_test.csv");
        visualNovelHandler.SetCharacterLocationFilePath("Assets/Resources/VNScripts/1st_scene_characters.csv");
    }

    void Start()
    {
        if(PlayerManager.Instance.playerInfo.coins == 0){
        // if(false){
            visualNovelHandler.StartVisualNovel();
            SoundManager.Instance.PlayMusic("visual_novel");
        }else{
            SoundManager.Instance.PlayMusic("map");
        }
        
    }

    private void ReturnDataVN(string obj)
    {
        return;
    }

    // Called when the mouse enters the collider
    public void OnMouseEnterCollider(string colliderName) {
        if (placeName != null) {
            placeName.text = colliderName;
        }
    }

    // Called when the mouse exits the collider
    public void OnMouseExitCollider() {
        
        if(UnityEngine.Random.Range(0, 4) == 1){
            StartCoroutine(ClearText());
        }else{
            if (placeName != null) {
                placeName.text = "";
            }
        }
        
    }

    // Called when the mouse clicks the collider
    public void OnMouseClickCollider(string colliderName) {
        if (placeName != null) {
            // placeName.text = "Mouse Clicked: " + colliderName;
            // // Start the coroutine to clear the text after 1 second
            // StartCoroutine(ClearText());

            if(colliderName == "Village"){
                SceneLoader.Instance.LoadGameScene(1);
            }else if(colliderName == "Town"){
                SceneLoader.Instance.LoadGameScene(2);
            }else{
                placeName.text = colliderName + " is not available yet!"; 
            }
        }
    }

    private IEnumerator ClearText() {
        yield return new WaitForSeconds(0.3f); // Wait for 1 second
        if (placeName != null) {
            placeName.text = "Press arrow keys or WASD to move around the map";
            yield return new WaitForSeconds(2f); // Wait for 1 second
            if (placeName != null) {
                placeName.text = "";
            }
        }
    }

}
