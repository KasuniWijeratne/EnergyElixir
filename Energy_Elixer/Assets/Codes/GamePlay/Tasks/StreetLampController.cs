using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetLampController : MonoBehaviour
{
    public GameObject spotlight; // Assign the spotlight GameObject in the inspector
    private bool pointsIncreased = false;
    public Notifications notification;

    void Start()
    {
        notification = FindObjectOfType<Notifications>();
    }

    public void ToggleLight()
    {
        if (spotlight != null)
        {
            bool isActive = !spotlight.activeSelf;
            spotlight.SetActive(isActive);

            // Log the state of the light
            if (isActive && pointsIncreased)
            {
                Debug.Log("Street light has been turned ON.");
                if (notification != null)
                {
                    notification.getNotificationMessage("StreetLamps", isActive);
                }
                GameManager.score -= 4;
                pointsIncreased = false;
            }
            else if (!isActive && !pointsIncreased)
            {
                Debug.Log("Street light has been turned OFF.");
                if (notification != null)
                    {
                        notification.getNotificationMessage("StreetLamps", isActive);
                    }
                    GameManager.score += 4;
                    pointsIncreased = true;
            }
        }
    }
}