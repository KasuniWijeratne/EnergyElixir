using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI placeName;

    // Called when the mouse enters the collider
    public void OnMouseEnterCollider(string colliderName) {
        if (placeName != null) {
            placeName.text = colliderName;
        }
    }

    // Called when the mouse exits the collider
    public void OnMouseExitCollider() {
        if (placeName != null) {
            placeName.text = "";
        }
    }

    // Called when the mouse clicks the collider
    public void OnMouseClickCollider(string colliderName) {
        if (placeName != null) {
            placeName.text = "Mouse Clicked: " + colliderName;

            // Start the coroutine to clear the text after 1 second
            StartCoroutine(ClearText());
        }
    }

    private IEnumerator ClearText() {
        yield return new WaitForSeconds(1f); // Wait for 1 second
        if (placeName != null) {
            placeName.text = "";
        }
    }

}
