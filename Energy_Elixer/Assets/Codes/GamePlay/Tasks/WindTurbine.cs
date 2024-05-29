using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WindTurbine : MonoBehaviour
{
    public Animator animator;
    public static bool isWindy;

    private Notifications notification;
    // Start is called before the first frame update

void Start()
    {
        if (animator == null)
        {
            Debug.LogError("Animator component is not assigned in the Inspector.");
        }
        isWindy = false;

        notification = FindObjectOfType<Notifications>();
        if (notification == null)
        {
            Debug.LogError("No Notifications object found in the scene.");
            
        }
        
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (animator != null)
        {animator.SetBool("isWindy", isWindy);
    }

}
    public void SwitchOnTurbine(){
        isWindy = true;
        if (notification != null){
            notification.getNotificationMessage("WindPower", isWindy);}
        GameManager.score += 12;
    }
}
