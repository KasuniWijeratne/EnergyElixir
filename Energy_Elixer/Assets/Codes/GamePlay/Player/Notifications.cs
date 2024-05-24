using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notifications : MonoBehaviour
{
    private Dictionary<string, string> notificationsList = new Dictionary<string, string>()
    {
        {"OnWaterTurbine", "The water turbine is now on.You are generating 14kW per hour"},
        {"OffWaterTurbine", "The rain is being wasted. Get the cloud above the turbine to generate some hydro power"},
        {"OnWindTurbine", "The wind turbine is now on. You are generating 12kW per hour"},
        {"OffWindTurbine", "The wind turbine is off. Please switch it on by pressing Enter"}
    };

    public void SendNotification(string key)
    {
        if (notificationsList.ContainsKey(key))
        {
            Debug.Log(notificationsList[key]);
            NotifyPlayer(notificationsList[key]);

        }
        else
        {
            Debug.Log("Notification not found.");
        }
    }
    private void NotifyPlayer(string msg)
    {
        if (!(msg == null))
        {
            Debug.Log(msg);
        }
        else
        {
            Debug.Log("Notification msg not found.");
        }
    }

}
