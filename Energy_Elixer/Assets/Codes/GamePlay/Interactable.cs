using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public bool isInRange;
    public KeyCode interactKey;
    public UnityEvent interactAction;
    private Notifications notification;

    void Start()
    {
        notification = FindObjectOfType<Notifications>();
        if (notification == null)
        {
            Debug.LogError("No Notifications object found in the scene.");
        }
        else
        {
            Debug.Log(notification.name.ToString());
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(interactKey) && isInRange && interactAction != null)
        {
            interactAction.Invoke();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        try
        {
            if (collision.attachedRigidbody.name == null)
                return;

            isInRange = true;
            triggerNotification(collision);
        }
        catch (System.NullReferenceException)
        {
            Debug.Log("RigidBody is initializing...."); //to handle the exception due to funciton being called before initialization
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            notification.getNotificationMessage("empty");
            isInRange = false;
        }
    }

    void triggerNotification(Collider2D collision)
    {
        try
        {
            if (collision.attachedRigidbody.name == "WindPower")
            {
                notification.getNotificationMessage("WindPower", isInRange);
            }
            else if (collision.attachedRigidbody.name == "HydroPower")
            {
                notification.getNotificationMessage("HydroPower", isInRange);
            }
            else if (collision.attachedRigidbody.name == "solar_panel")
            {
                notification.getNotificationMessage("SolarPower", isInRange);
            }
            else if (collision.attachedRigidbody.name == "biomass")
            {
                notification.getNotificationMessage("BioMassPower", isInRange);
            }
            else if (collision.attachedRigidbody.name == "home")
            {
                notification.getNotificationMessage("NaturalLight", isInRange);
            }
            else
            {
                // Debug.Log(collision.attachedRigidbody.name);
            }
        }
        catch (System.NullReferenceException e)
        {
            Debug.Log("Sending notification fails due to missing rigidBody name" + e);
        }
    }
}
