using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetLampInteraction : MonoBehaviour
{
    private bool playerInRange;
    private StreetLampController streetLampController;

    void Start()
    {
        streetLampController = GetComponent<StreetLampController>();
    }

    void Update()
    {
        if (playerInRange && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)))
        {
            streetLampController.ToggleLight();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
