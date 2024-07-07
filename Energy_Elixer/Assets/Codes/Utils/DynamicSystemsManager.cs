using System.Collections;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using System.Linq;
using Unity.VisualScripting;

public class MonthDetails
{
    public static readonly ReadOnlyCollection<string> monthNames = new List<string> { "JANUARY", "FEBRUARY", "MARCH", "APRIL",
                                "MAY", "JUNE", "JULY", "AUGUST", "SEPTEMBER", "OCTOBER", "NOVEMBER", "DECEMBER"}.AsReadOnly();

    public static readonly Dictionary<int, int> daysPerMonth = new Dictionary<int, int>
    {
        { 1, 31 }, // January
        { 2, 28 }, // February
        { 3, 31 }, // March
        { 4, 30 }, // April
        { 5, 31 }, // May
        { 6, 30 }, // June
        { 7, 31 }, // July
        { 8, 31 }, // August
        { 9, 30 }, // September
        { 10, 31 }, // October
        { 11, 30 }, // November
        { 12, 31 } // December
    };
}




public interface IDynamicSystem 
{
    event EventHandler<float> ScoreChanged;
    float CurrentScore { get; }
    void InitializeScore();
    void UpdateScore();
    void AddDailyChallengePoints(float points);
    IEnumerator ChangeEnvironmentStatusAsync();
    int getCurrentEnvironmentStatus();
}
 

    /* 
    ** Current score will be assigned as follows:

    max 10 points,
    5 points if this month's power consumption rate is lower than the previous month's rate(power consumption per day)
    5 points if this month's power consumption rate is lower than the previous year's same month's rate

    max 10 points,
    5 points if yesterday's power consumption was +- 1 from the previous day's rate
    10 points if yesterday's power consumption was lower than (-2) from the previous day's rate

    max 25 points,
    25 points if the current power consumption rate is 20% lower than the previous month's avarage rate (monthlyPowerConsumption / (daysPerMonth * 16 * 60 * 60[only 16 hours are considered]))
    15 points if the current power consumption rate is 10% lower than the previous month's avarage rate
    5 points if the current power consumption rate is in the range of +- 5% of the previous month's avarage rate

    max 35 points,
    if the current time is between 17:00 and 21:00 and, 
    35 points if the current power consumption rate is 40% lower than the previous day's rate (dailyPowerConsumption / 16 * 60 * 60)
    25 points if the current power consumption rate is 20% lower than the previous day's rate
    15 points if the current power consumption rate is 10% lower than the previous day's rate
    5 points if the current power consumption rate is in the range of +- 5% of the previous day's rate

    max 20 points for daily challenges,
    20 points if the user completes the daily challenge with gold medal
    10 points if the user completes the daily challenge with silver medal
    5 points if the user completes the daily challenge with bronze medal
    */


public class DynamicSystemsManager : IDynamicSystem
{
    public event EventHandler<float> ScoreChanged;
    public float CurrentScore { get; private set; }

    private const float ScoreUpdateInterval = 10f;
    private const int MaxDailyChallengePoints = 20;
    private float dailyChallengePoints = 0;

    private bool monthlyPowerConsumptionFetched = false;
    private bool dailyPowerConsumptionFetched = false;

    private Dictionary<int, float> dailyPowerConsumption;
    private Dictionary<int, Dictionary<string, float>> monthlyPowerConsumption;

    private MonoBehaviour coroutineRunner;

    public DynamicSystemsManager()
    {
        dailyPowerConsumption = new Dictionary<int, float>();
        monthlyPowerConsumption = new Dictionary<int, Dictionary<string, float>>();
        coroutineRunner = new GameObject("DynamicSystemsManager").AddComponent<MonoBehaviour>();
        UnityEngine.Object.DontDestroyOnLoad(coroutineRunner.gameObject);
    }

    public void InitializeScore()
    {
        FetchInitialData();
        coroutineRunner.StartCoroutine(UpdateScorePeriodically());
        coroutineRunner.StartCoroutine(ResetDailyScoreAtMidnight());
    }

    private void FetchInitialData()
    {
        APIHandler.Instance.FetchAllPowerConsumption(OnAllPowerConsumptionFetched, OnAPIError);
        APIHandler.Instance.FetchDailyPowerConsumptionByCurrentMonth(OnDailyPowerConsumptionFetched, OnAPIError);
    }

    private void OnAllPowerConsumptionFetched(string response)
    {   
        monthlyPowerConsumptionFetched = true;
        PowerConsumptionAllResponse allConsumption = JsonConvert.DeserializeObject<PowerConsumptionAllResponse>(response);
        foreach (var yearlyView in allConsumption.yearlyPowerConsumptionViews)
        {
            monthlyPowerConsumption[yearlyView.year] = yearlyView.units.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.units
            );
        }

        if (dailyPowerConsumptionFetched)
            CalculateInitialScore();
    }

    private void OnDailyPowerConsumptionFetched(string response)
    {
        dailyPowerConsumptionFetched = true;
        PowerConsumptionDailyView dailyConsumption = JsonConvert.DeserializeObject<PowerConsumptionDailyView>(response);
        dailyPowerConsumption = dailyConsumption.dailyUnits;

        if (monthlyPowerConsumptionFetched)
            CalculateInitialScore();
    }

    private void OnAPIError(string error)
    {
        Debug.LogError($"API Error: {error}");
    }

    private void CalculateInitialScore()
    {
        float score = 0;

        int currentYear = DateTime.Now.Year;
        int currentMonth = DateTime.Now.Month;
        string currentMonthName = MonthDetails.monthNames[currentMonth - 1];

        if (monthlyPowerConsumption.ContainsKey(currentYear) && monthlyPowerConsumption.ContainsKey(currentYear - 1))
        {
            float currentMonthConsumption = monthlyPowerConsumption[currentYear][currentMonthName];
            float previousMonthConsumption = monthlyPowerConsumption[currentYear][MonthDetails.monthNames[(currentMonth - 2 + 12) % 12]];
            float previousYearSameMonthConsumption = monthlyPowerConsumption[currentYear - 1][currentMonthName];

            if (currentMonthConsumption < previousMonthConsumption)
                score += 5;
            if (currentMonthConsumption < previousYearSameMonthConsumption)
                score += 5;
        }

        if (dailyPowerConsumption.Count >= 2)
        {
            int today = DateTime.Now.Day;
            int yesterday = today - 1;

            if (dailyPowerConsumption.ContainsKey(today) && dailyPowerConsumption.ContainsKey(yesterday))
            {
                float todayConsumption = dailyPowerConsumption[today];
                float yesterdayConsumption = dailyPowerConsumption[yesterday];

                if (Math.Abs(todayConsumption - yesterdayConsumption) <= 1)
                    score += 5;
                else if (todayConsumption < yesterdayConsumption - 2)
                    score += 10;
            }
        }

        CurrentScore = score;
        ScoreChanged?.Invoke(this, CurrentScore);
    }

    private IEnumerator UpdateScorePeriodically()
    {   
        while (true)
        {
            yield return new WaitForSeconds(ScoreUpdateInterval);
            UpdateScore();
        }
    }

    public void UpdateScore()
    {
        APIHandler.Instance.FetchCurrentPowerConsumption(OnCurrentPowerConsumptionFetched, OnAPIError);
    }

    private void OnCurrentPowerConsumptionFetched(string response)
    {
        CurrentPowerConsumption currentConsumption = JsonConvert.DeserializeObject<CurrentPowerConsumption>(response);
        float consumptionRate = currentConsumption.currentConsumption / 3600f; // Convert to Wh/s

        float additionalScore = 0;

        float previousMonthAverageRate = GetPreviousMonthAverageRate();
        Debug.Log($"Previous month average rate: {previousMonthAverageRate}");
        if (consumptionRate < previousMonthAverageRate * 0.8f)
            additionalScore += 25;
        else if (consumptionRate < previousMonthAverageRate * 0.9f)
            additionalScore += 15;
        else if (Math.Abs(consumptionRate - previousMonthAverageRate) <= previousMonthAverageRate * 0.05f)
            additionalScore += 5;

        int currentHour = DateTime.Now.Hour;
        if (currentHour >= 17 && currentHour < 21)
        {
            float previousDayRate = GetPreviousDayRate();
            Debug.Log($"Previous day rate: {previousDayRate}");
            if (consumptionRate < previousDayRate * 0.6f)
                additionalScore += 35;
            else if (consumptionRate < previousDayRate * 0.8f)
                additionalScore += 25;
            else if (consumptionRate < previousDayRate * 0.9f)
                additionalScore += 15;
            else if (Math.Abs(consumptionRate - previousDayRate) <= previousDayRate * 0.05f)
                additionalScore += 5;
        }else{
            additionalScore += 15;
        }

        CurrentScore += additionalScore;
        ScoreChanged?.Invoke(this, CurrentScore);
    }

    private float GetPreviousMonthAverageRate()
    {
        int currentYear = DateTime.Now.Year;
        int currentMonth = DateTime.Now.Month;
        // string previousMonthName = MonthDetails.monthNames[(currentMonth - 2 + 12) % 12];
        string previousMonthName = MonthDetails.monthNames[currentMonth - 2];

        if (monthlyPowerConsumption.ContainsKey(currentYear) && monthlyPowerConsumption[currentYear].ContainsKey(previousMonthName))
        {
            float previousMonthConsumption = monthlyPowerConsumption[currentYear][previousMonthName];
            int daysInPreviousMonth = MonthDetails.daysPerMonth[currentMonth -2];
            // int daysInPreviousMonth = MonthDetails.daysPerMonth[(currentMonth - 1 + 12) % 12];
            return 1000f * previousMonthConsumption / (daysInPreviousMonth * 16f * 3600f); // Assuming 16 active hours per day
        }
        Debug.LogWarning("Previous month data not available");
        return 0.1f; // Default value if data is not available
    }

    private float GetPreviousDayRate()
    {
        int yesterday = DateTime.Now.AddDays(-1).Day;
        if (dailyPowerConsumption.ContainsKey(yesterday))
        {
            return 1000f * dailyPowerConsumption[yesterday] / (16f * 3600f); // Assuming 16 active hours per day
        }
        Debug.LogWarning("Previous day data not available");
        return 0.2f; // Default value if data is not available
    }

    public void AddDailyChallengePoints(float points)
    {
        dailyChallengePoints = Math.Min(dailyChallengePoints + points, MaxDailyChallengePoints);
    }

    private IEnumerator ResetDailyScoreAtMidnight()
    {
        while (true)
        {
            DateTime now = DateTime.Now;
            DateTime nextMidnight = now.Date.AddDays(1);
            TimeSpan timeUntilMidnight = nextMidnight - now;

            yield return new WaitForSeconds((float)timeUntilMidnight.TotalSeconds);

            CurrentScore = Math.Max(0, CurrentScore - dailyChallengePoints);
            CurrentScore += dailyChallengePoints;
            dailyChallengePoints = 0;

            ScoreChanged?.Invoke(this, CurrentScore);
        }
    }

    public IEnumerator ChangeEnvironmentStatusAsync()
    {
        // This method is not used in the current implementation
        yield break;
    }

    public int getCurrentEnvironmentStatus()
    {
        if (CurrentScore > 75.0f)
            return 3;
        else if (CurrentScore > 50.0f)
            return 2;
        else if (CurrentScore > 25.0f)
            return 1;
        else
            return 0;
    }
}