using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestSceneHandler : MonoBehaviour
{
    // [SerializeField] private TMP_Text Text;
    [SerializeField] private TMP_InputField inputField;

    // Start is called before the first frame update
    void Start()
    {
        // PlayerManager.Instance.OnPlayerEnvironmentChanged += OnPlayerEnvironmentChanged;

        APIHandler.Instance.Authenticate("NjVjNjA0MGY0Njc3MGQ1YzY2MTcyMmNkOjY1YzYwNDBmNDY3NzBkNWM2NjE3MjJjMw");
        // DatabaseHandler.Instance.DisplayPlayerScores();
    }

    // Update is called once per frame
    void Update()
    {
  
    }
//Buttons for following tasks
// View Player List	
// View Yearly Power Consumption	
// View Power Consumption by Specific Month	
// View Power Consumption by Current Month	
// View Daily Power Consumption by Specific Month	
// View Daily Power Consumption by Current Month	
// View All Power Consumption	


    public void OnViewAllPlayersClick() {
        // SoundManager.Instance.PlaySFX("button_clicked");
        APIHandler.Instance.DisplayPlayerList();
    }

    public void OnViewYearlyPowerConsumptionClick() {
        // SoundManager.Instance.PlaySFX("button_clicked");
        int year = 2023;
        int.TryParse(inputField.text, out year);
        APIHandler.Instance.DisplayYearlyPowerConsumption(year);
    }

    public void OnViewPowerConsumptionBySpecificMonthClick() {
        // SoundManager.Instance.PlaySFX("button_clicked");
        int year = 2023;
        string month = "MAY";
        int.TryParse(inputField.text, out year);
        // Debug.Log("Not implemented yet");
        APIHandler.Instance.DisplayPowerConsumptionBySpecificMonth(year, month);
    }

    public void OnViewPowerConsumptionByCurrentMonthClick() {
        // SoundManager.Instance.PlaySFX("button_clicked");
        // Debug.Log("Not implemented yet");
        APIHandler.Instance.DisplayPowerConsumptionByCurrentMonth();
    }

    public void OnViewDailyPowerConsumptionBySpecificMonthClick() {
        // SoundManager.Instance.PlaySFX("button_clicked");
        int year = 2023;
        string month = "MAY";
        int.TryParse(inputField.text, out year);
        // Debug.Log("Not implemented yet");
        APIHandler.Instance.DisplayDailyPowerConsumptionBySpecificMonth(year, month);
    }

    public void OnViewDailyPowerConsumptionByCurrentMonthClick() {
        // SoundManager.Instance.PlaySFX("button_clicked");
        // Debug.Log("Not implemented yet");
        APIHandler.Instance.DisplayDailyPowerConsumptionByCurrentMonth();
    }

    public void OnViewAllPowerConsumptionClick() {
        // SoundManager.Instance.PlaySFX("button_clicked");
        APIHandler.Instance.DisplayAllPowerConsumption();
    }

    public void OnViewCurrentPowerConsumption() {
        // SoundManager.Instance.PlaySFX("button_clicked");
        APIHandler.Instance.DisplayCurrentPowerConsumption();
    }
}
