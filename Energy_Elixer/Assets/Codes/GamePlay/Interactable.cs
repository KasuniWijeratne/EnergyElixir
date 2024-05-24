using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public bool isInRange;
    public KeyCode interactKey;
    public UnityEvent interactAction;
    private Notifications notifications;
    // Start is called before the first frame update
    void Start()
    {
        notifications = FindObjectOfType<Notifications>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(interactKey) && isInRange){
            interactAction.Invoke();
        }
    }

    void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag == "Player"){
            isInRange = true;
            //if (!(notifications == null)) // chec
                notifications.SendNotification("OffWindTurbine"); // Call the NotifyPlayer method on the instance

       }
    }

    void OnTriggerExit2D(Collider2D collision){
        if(collision.tag == "Player"){
            isInRange = false;
        }
    }
}
