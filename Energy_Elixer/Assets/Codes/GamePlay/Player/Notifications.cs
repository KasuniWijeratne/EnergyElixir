using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Notifications : MonoBehaviour
{
    public TextMeshProUGUI notificationText;

    private BulbyInteraction BulbyInteraction;
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
        { "OffBioMass", "The BioMass plant is off. Please put the leaves for it to process" },
        { "OffStreetLamps", "Street light has been turned off. You are generating 4kW"},
        { "OnStreetLamps" , "Street light has been turned on again. Hit SHIFT" },
        { "Bulby_Interact", "This is Bulby. He can help you with your tasks. Press Shift to interact with him when he says Hi."},
        { "Bulby_Laundry", "Bulby has successfully completed the laundry task. You have helped the world to save energy."},
        { "Bulby_Laundry_Fail", "Bulby has failed to complete the laundry task. Energy is being wasted."},
        { "Bulby_Iron", "Bulby has successfully completed the ironing task. You have helped the world to save energy."},
        { "Bulby_Iron_Fail", "Bulby has failed to complete the ironing task. Energy is being wasted."},
        { "Bulby_Vaccum", "Bulby has successfully completed the vaccum task. You have helped the world to save energy."},
        { "Bulby_Vaccum_Fail", "Bulby has failed to complete the vaccum task. Energy is being wasted."},
        { "Bulby_Light", "Bulby has successfully completed the light task. You have helped the world to save energy."},
        { "Bulby_Light_Fail", "Bulby has failed to complete the light task. Energy is being wasted."},
        { "Bulby_Fridge", "Bulby has successfully completed the fridge task. You have helped the world to save energy."},
        { "Bulby_Fridge_Fail", "Bulby has failed to complete the fridge task. Energy is being wasted."}
    };

    void Start()
    {
        BulbyInteraction = FindObjectOfType<BulbyInteraction>();
        if (BulbyInteraction == null)
        {
            Debug.LogError("BulbyInteraction object not found!");
        }
    }

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
            message = "OnSolarPanel";
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
        else if (tag == "Bulby_Intro" && trigger)
        {
            message = "Bulby_Interact";
        }
        else if (tag == "Bulby_Laundry" && trigger && BulbyInteraction.getTaskSuccess(tag)){
            message = "Bulby_Laundry";
        }
        else if (tag == "Bulby_Laundry" && trigger && !BulbyInteraction.getTaskSuccess(tag)){
            message = "Bulby_Laundry_Fail";
        }
        else if (tag == "Bulby_Iron" && trigger && BulbyInteraction.getTaskSuccess(tag)){
            message = "Bulby_Iron";
        }
        else if (tag == "Bulby_Iron" && trigger && !BulbyInteraction.getTaskSuccess(tag)){ 
            message = "Bulby_Iron_Fail";
        }
        else if (tag == "Bulby_Vaccum" && trigger && BulbyInteraction.getTaskSuccess(tag)){
            message = "Bulby_Vaccum";
        }
        else if (tag == "Bulby_Vaccum" && trigger && !BulbyInteraction.getTaskSuccess(tag)){
            message = "Bulby_Vaccum_Fail";
        }
        else if (tag == "Bulby_Light" && trigger && BulbyInteraction.getTaskSuccess(tag)){
            message = "Bulby_Light";
        }
        else if (tag == "Bulby_Light" && trigger && !BulbyInteraction.getTaskSuccess(tag)){
            message = "Bulby_Light_Fail";
        }
        else if (tag == "Bulby_Fridge" && trigger && BulbyInteraction.getTaskSuccess(tag)){
            message = "Bulby_Fridge";
        }
        else if (tag == "Bulby_Fridge" && trigger && !BulbyInteraction.getTaskSuccess(tag)){
            message = "Bulby_Fridge_Fail";
        } 
        else
        {
            Debug.Log(tag);
            message = "empty";
        }
        sendNotification(message);
    }
}
