using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Notifications : MonoBehaviour
{
    public TextMeshProUGUI notificationText;
    private Dictionary<string, string> notificationsList = new Dictionary<string, string>()
    {
        { "empty", "" },
        { "OnWaterTurbine", "The water turbine is now on.You are generating 14kW per hour" },
        {
            "OffWaterTurbine",
            "The rain is being wasted. Get the cloud above the turbine to generate some hydro power"
        },
        { "OnWindTurbine", "The wind turbine is now on. You are generating 12kW per hour" },
        { "OffWindTurbine", "The wind turbine is off. Please switch it on by pressing Enter" },
        { "OnSolarPanel", "The Solar Panel is now on. You are generating 10kW per hour." },
        {
            "OffSolarPanel",
            "The cloud is blocking the solar panel again! Push away the cloud to generate power."
        },
        { "OnBioMass", "The BioMass plant is now on. You are generating 8kW per hour." },
        { "OffBioMass", "The BioMass plant is off. Please put the leaves for it to process" }
        { "OffStreetLamps", "Street light has been turned off. You are generating 4kW"},
        { "OnStreetLamps" , "Street light has been turned on again. Hit SHIFT" }
    };

    public void sendNotification(string key)
    {
        if (notificationsList.ContainsKey(key))
        {
            notificationText.text = notificationsList[key];
        }
        else
        {
            Debug.Log("Notification not found.");
        }
    }

    public void getNotificationMessage(string tag, bool trigger = false)
    {
        string message;
        if (tag == "WindPower" && trigger && !WindTurbine.isWindy)
        {
            message = "OffWindTurbine";
        }
        else if (tag == "HydroPower" && trigger && !WaterTurbine.isRaining)
        {
            message = "OffWaterTurbine";
        }
        else if (tag == "WindPower" && trigger && WindTurbine.isWindy)
        {
            message = "OnWindTurbine";
        }
        else if (tag == "HydroPower" && trigger && WaterTurbine.isRaining)
        {
            message = "OnWaterTurbine";
        }
        else if (tag == "SolarPower" && trigger && LightDetector.isBlocked)
        {
            message = "OffSolarPanel";
        }
        else if (tag == "SolarPower" && trigger && !LightDetector.isBlocked)
        { 
            message = "OnSolarPanel"
        }
        else if (tag == "StreetLamps" && trigger)
        {
            message = "OnStreetLamps";
        }
        else if (tag == "StreetLamps" && !trigger)
        {
            message = "OffStreetLamps";
        }

        else if (tag == "BioMassPower" && trigger && !BioMassTask.BioMassTaskComplete)
        {
            message = "OffBioMass";
        }
        else if (tag == "BioMassPower" && trigger && BioMassTask.BioMassTaskComplete)
        {
            message = "OnBioMass";
        }
        else
        {
            message = "empty";
        }
        sendNotification(message);
    }
}
