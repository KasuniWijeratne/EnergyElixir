using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BioMassTask : MonoBehaviour
{
    public static bool BioMassTaskComplete = false;
    private Notifications notification;

    void Start()
    {
        notification = FindObjectOfType<Notifications>();
        if (notification == null)
        {
            Debug.LogError("No Notifications object found in the scene.");
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Biomass factory")
        {
            GameManager.score += 8;
            BioMassTaskComplete = true;
            gameObject.SetActive(false);     
            notification.getNotificationMessage("BioMassPower", BioMassTaskComplete);
                   
        }
    }
}
